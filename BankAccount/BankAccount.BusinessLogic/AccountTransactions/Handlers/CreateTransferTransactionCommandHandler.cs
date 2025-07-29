using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.AccountTransactions.Commands;
using BankAccount.BusinessLogic.AccountTransactions.DTOs;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Enums;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.AccountTransactions.Handlers;

public class CreateTransferTransactionCommandHandler : ICommandHandler<CreateTransferTransactionCommand, TransferTransactionResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyService _currencyService;

    public CreateTransferTransactionCommandHandler(ITransactionRepository transactionRepository, IAccountRepository accountRepository, ICurrencyService currencyService)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _currencyService = currencyService;
    }

    public async Task<TransferTransactionResponse> Handle(CreateTransferTransactionCommand request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (!await _accountRepository.ExistsByIdAsync(request.AccountIdFrom))
            errors.Add(nameof(request.AccountIdFrom), [$"Счёт отправителя '{request.AccountIdFrom}' не существует."]);

        if (!await _accountRepository.ExistsByIdAsync(request.AccountIdTo))
            errors.Add(nameof(request.AccountIdTo), [$"Счёт получателя '{request.AccountIdTo}' не существует."]);

        if (!await _currencyService.IsCurrencySupportedAsync(request.Currency))
            errors.Add(nameof(request.Currency), [$"Валюта '{request.Currency}' не поддерживается."]);

        var accountFrom = await _accountRepository.GetByIdAsync(request.AccountIdFrom);
        var accountTo = await _accountRepository.GetByIdAsync(request.AccountIdTo);

        if (accountFrom.Balance < request.Amount)
            errors.Add(nameof(request.Amount), [$"Недостаточно средств на счёте '{request.AccountIdFrom}' для перевода."]);

        if (accountFrom.Currency != accountTo.Currency)
            errors.Add(nameof(request.Currency), [$"Не совпадают типы счетов '{accountFrom.Currency}' и '{accountTo.Currency}' для перевода."]);

        if (errors.Count != 0)
            throw new ValidationException(errors);

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

        
        await _transactionRepository.RegisterTransactionAsync(debitTransaction);
        await _transactionRepository.RegisterTransactionAsync(creditTransaction);

        return new TransferTransactionResponse { DebitTransactionId = debitTransaction.Id, CreditTransactionId = creditTransaction.Id };
    }
}
