using BankAccount.BusinessLogic.AccountTransactions.DTOs;
using BankAccount.Domain.Entities;

namespace BankAccount.BusinessLogic.AccountTransactions.Mappers;

public static class TransactionMapper
{
    public static TransactionResponse ToResponse(AccountTransaction transaction) =>
        new()
        {
            Id = transaction.Id,
            AccountId = transaction.AccountId,
            CounterpartyAccountId = transaction.CounterpartyAccountId,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            Type = transaction.Type,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        };

    public static List<TransactionResponse> ToResponseList(IEnumerable<AccountTransaction> transactions) =>
        transactions.Select(ToResponse).ToList();
}