using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.AccountTransactions.DTOs;

public class CreateTransactionRequest
{
    public Guid AccountId { get; init; }
    public Guid? CounterpartyAccountId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public TransactionType Type { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}