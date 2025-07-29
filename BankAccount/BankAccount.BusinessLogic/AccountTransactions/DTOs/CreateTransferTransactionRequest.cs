namespace BankAccount.BusinessLogic.AccountTransactions.DTOs;

public class CreateTransferTransactionRequest
{
    public Guid AccountIdFrom { get; init; }
    public Guid AccountIdTo { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
