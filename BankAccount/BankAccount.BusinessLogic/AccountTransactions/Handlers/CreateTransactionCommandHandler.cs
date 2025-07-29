using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.AccountTransactions.Commands;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Enums;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.AccountTransactions.Handlers;

public class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, Guid>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrencyService _currencyService;
    private readonly IAccountRepository _accountRepository;

    public CreateTransactionCommandHandler(ITransactionRepository transactionRepository, ICurrencyService currencyService, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _currencyService = currencyService;
        _accountRepository = accountRepository;
    }

    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if(!await _accountRepository.ExistsByIdAsync(request.AccountId))
            errors.Add(nameof(request.AccountId), [$"Указанный счёт '{request.AccountId}' не существует."]);

        if (request.CounterpartyAccountId != null && !await _accountRepository.ExistsByIdAsync(request.CounterpartyAccountId.Value))
            errors.Add(nameof(request.CounterpartyAccountId), [$"Указанный счёт '{request.CounterpartyAccountId}' не существует."]);

        if (!await _currencyService.IsCurrencySupportedAsync(request.Currency))
            errors.Add(nameof(request.Currency), [$"Валюта '{request.Currency}' не поддерживается."]);

        if (errors.Count != 0)
            throw new ValidationException(errors);

        if (request.Type == TransactionType.Debit)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId);

            if (account.Balance < request.Amount)
            {
                errors.Add(nameof(request.Amount), [$"Недостаточно средств на счёте '{request.AccountId}' для списания {request.Amount} {request.Currency}."]);
            }
        }

        var transaction = new AccountTransaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            CounterpartyAccountId = request.CounterpartyAccountId,
            Amount = request.Amount,
            Currency = request.Currency,
            Type = request.Type,
            Description = request.Description,
            CreatedAt = request.CreatedAt
        };

        return await _transactionRepository.RegisterTransactionAsync(transaction);
    }
}
