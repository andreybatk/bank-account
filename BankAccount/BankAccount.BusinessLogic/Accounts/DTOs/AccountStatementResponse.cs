using BankAccount.BusinessLogic.AccountTransactions.DTOs;
using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.Accounts.DTOs
{
    public class AccountStatementResponse
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public AccountType Type { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public decimal? InterestRate { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public List<TransactionResponse> Transactions { get; set; } = [];
    }
}