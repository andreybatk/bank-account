using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.AccountTransactions.Commands;
using BankAccount.BusinessLogic.AccountTransactions.DTOs;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Enums;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BankAccount.BusinessLogic.AccountTransactions.Handlers;

public class CreateTransferTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    IAccountRepository accountRepository,
    ICurrencyService currencyService,
    ILogger<CreateTransferTransactionCommandHandler> logger)
    : ICommandHandler<CreateTransferTransactionCommand, TransferTransactionResponse>
{
    public async Task<TransferTransactionResponse> Handle(CreateTransferTransactionCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        var accountFrom = await accountRepository.GetByIdAsync(request.AccountIdFrom);

        if (accountFrom is null)
        {
            errors.Add("Указанный счёт отправителя не существует.");
            logger.LogError("Указанный счёт отправителя '{accountIdFrom}' не существует.", request.AccountIdFrom);
        }

        var accountTo = await accountRepository.GetByIdAsync(request.AccountIdTo);

        if (accountTo is null)
        {
            errors.Add("Указанный счёт получателя не существует.");
            logger.LogError("Указанный счёт получателя '{accountIdTo}' не существует.", request.AccountIdTo);
        }

        if (errors.Count != 0)
            throw new EntityNotFoundException(errors);

        if (!await currencyService.IsCurrencySupportedAsync(request.Currency))
        {
            logger.LogError("Валюта '{currency}' не поддерживается.", request.Currency);
            throw new ValidationException("Валюта не поддерживается.");
        }

        if (accountFrom!.Balance < request.Amount)
        {
            logger.LogError("Недостаточно средств на счёте '{accountIdFrom}' для списания {amount} {currency}.", request.AccountIdFrom, request.Amount, request.Currency);
            throw new BadRequestException("Недостаточно средств для списания.");
        }

        if (accountFrom.Currency != accountTo!.Currency)
        {
            logger.LogError("Не совпадают типы счетов '{accountFrom.Currency}' и '{accountTo.Currency}' для перевода.", accountFrom.Currency, accountTo.Currency);
            throw new BadRequestException("Не совпадают типы счетов.");
        }

        var debitTransaction = new AccountTransaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountIdTo,
            CounterpartyAccountId = request.AccountIdFrom,
            Amount = request.Amount,
            Currency = request.Currency,
            Type = ETransactionType.Debit,
            Description = request.Description,
            CreatedAt = request.CreatedAt
        };

        var creditTransaction = new AccountTransaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountIdFrom,
            CounterpartyAccountId = request.AccountIdTo,
            Amount = request.Amount,
            Currency = request.Currency,
            Type = ETransactionType.Credit,
            Description = request.Description,
            CreatedAt = request.CreatedAt
        };

        await transactionRepository.RegisterTransactionAsync(debitTransaction);
        await transactionRepository.RegisterTransactionAsync(creditTransaction);

        accountTo.Balance += request.Amount;
        accountFrom.Balance -= request.Amount;

        accountTo.Transactions.Add(debitTransaction);
        accountFrom.Transactions.Add(creditTransaction);

        await accountRepository.UpdateAsync(accountTo);
        await accountRepository.UpdateAsync(accountFrom);

        return new TransferTransactionResponse { DebitTransactionId = debitTransaction.Id, CreditTransactionId = creditTransaction.Id };
    }
}
