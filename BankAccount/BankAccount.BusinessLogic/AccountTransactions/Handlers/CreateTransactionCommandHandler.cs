using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.AccountTransactions.Commands;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Enums;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.AccountTransactions.Handlers;

public class CreateTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    ICurrencyService currencyService,
    IAccountRepository accountRepository)
    : ICommandHandler<CreateTransactionCommand, Guid>
{
    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        var account = await accountRepository.GetByIdAsync(request.AccountId);

        if (account is null)
            errors.Add($"Указанный счёт '{request.AccountId}' не существует.");

        if (request.CounterpartyAccountId != null && !await accountRepository.ExistsByIdAsync(request.CounterpartyAccountId.Value))
            errors.Add($"Указанный счёт '{request.CounterpartyAccountId}' не существует.");

        if (errors.Count != 0)
            throw new EntityNotFoundException(errors);

        if (!await currencyService.IsCurrencySupportedAsync(request.Currency))
            throw new ValidationException($"Валюта '{request.Currency}' не поддерживается.");

        if (request.Type == TransactionType.Debit)
        {
            if (account!.Balance < request.Amount)
                throw new BadRequestException($"Недостаточно средств на счёте '{request.AccountId}' для списания {request.Amount} {request.Currency}.");
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

        return await transactionRepository.RegisterTransactionAsync(transaction);
    }
}
