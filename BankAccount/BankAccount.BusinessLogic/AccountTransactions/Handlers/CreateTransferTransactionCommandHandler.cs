using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.AccountTransactions.Commands;
using BankAccount.BusinessLogic.AccountTransactions.DTOs;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Enums;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.AccountTransactions.Handlers;

public class CreateTransferTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    IAccountRepository accountRepository,
    ICurrencyService currencyService)
    : ICommandHandler<CreateTransferTransactionCommand, TransferTransactionResponse>
{
    public async Task<TransferTransactionResponse> Handle(CreateTransferTransactionCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        var accountFrom = await accountRepository.GetByIdAsync(request.AccountIdFrom);

        if (accountFrom is null)
            errors.Add($"Указанный счёт '{request.AccountIdFrom}' не существует.");

        var accountTo = await accountRepository.GetByIdAsync(request.AccountIdTo);

        if (accountTo is null)
            errors.Add($"Указанный счёт '{request.AccountIdTo}' не существует.");

        if (errors.Count != 0)
            throw new EntityNotFoundException(errors);

        if (!await currencyService.IsCurrencySupportedAsync(request.Currency))
            throw new ValidationException($"Валюта '{request.Currency}' не поддерживается.");

        if (accountFrom!.Balance < request.Amount)
            throw new BadRequestException($"Недостаточно средств на счёте '{request.AccountIdFrom}' для списания {request.Amount} {request.Currency}.");

        if (accountFrom.Currency != accountTo!.Currency)
            throw new BadRequestException($"Не совпадают типы счетов '{accountFrom.Currency}' и '{accountTo.Currency}' для перевода.");

        var debitTransaction = new AccountTransaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountIdFrom,
            CounterpartyAccountId = request.AccountIdTo,
            Amount = request.Amount,
            Currency = request.Currency,
            Type = TransactionType.Debit,
            Description = request.Description,
            CreatedAt = request.CreatedAt
        };

        var creditTransaction = new AccountTransaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountIdTo,
            CounterpartyAccountId = request.AccountIdFrom,
            Amount = request.Amount,
            Currency = request.Currency,
            Type = TransactionType.Credit,
            Description = request.Description,
            CreatedAt = request.CreatedAt
        };

        
        await transactionRepository.RegisterTransactionAsync(debitTransaction);
        await transactionRepository.RegisterTransactionAsync(creditTransaction);

        return new TransferTransactionResponse { DebitTransactionId = debitTransaction.Id, CreditTransactionId = creditTransaction.Id };
    }
}
