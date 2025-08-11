using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.AccountTransactions.Commands;
using BankAccount.DataAccess;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Enums;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankAccount.BusinessLogic.AccountTransactions.Handlers;

public class CreateTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    ICurrencyService currencyService,
    IAccountRepository accountRepository,
    ILogger<CreateTransactionCommandHandler> logger,
    ApplicationDbContext context)
    : ICommandHandler<CreateTransactionCommand, Guid>
{
    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        var account = await accountRepository.GetByIdAsync(request.AccountId);

        if (account is null)
        {
            errors.Add("Указанный счёт не существует.");
            logger.LogError("Указанный счёт '{accountId}' не существует.", request.AccountId);
        }

        if (request.CounterpartyAccountId != null && !await accountRepository.ExistsByIdAsync(request.CounterpartyAccountId.Value))
        {
            errors.Add("Указанный счёт контрагента не существует.");
            logger.LogError("Указанный счёт контрагента '{counterpartyAccountId}' не существует.", request.CounterpartyAccountId);
        }

        if (errors.Count != 0)
            throw new EntityNotFoundException(errors);

        if (!await currencyService.IsCurrencySupportedAsync(request.Currency))
        {
            logger.LogError("Валюта '{currency}' не поддерживается.", request.Currency);
            throw new ValidationException("Валюта не поддерживается.");
        }

        if (request.Type == ETransactionType.Credit)
        {
            if (account!.Balance < request.Amount)
            {
                logger.LogError("Недостаточно средств на счёте '{accountId}' для списания {amount} {currency}.", request.AccountId, request.Amount, request.Currency);
                throw new BadRequestException("Недостаточно средств для списания.");
            }
        }

        await using var tx = await context.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.Serializable, cancellationToken);

        try
        {
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

            var transactionId = await transactionRepository.RegisterTransactionAsync(transaction);

            account!.Balance = transaction.Type == ETransactionType.Credit
                ? account.Balance -= transaction.Amount
                : account.Balance += transaction.Amount;

            await accountRepository.UpdateAsync(account);
            await tx.CommitAsync(cancellationToken);

            return transactionId;
        }
        catch (Exception ex)
        {
            logger.LogError("Ошибка при создании транзакции. {message}", ex.Message);
            await tx.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
