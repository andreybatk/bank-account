namespace BankAccount.BusinessLogic.AccountTransactions.DTOs;

public class TransferTransactionResponse
{
    public Guid DebitTransactionId { get; init; }
    public Guid CreditTransactionId { get; init; }
}