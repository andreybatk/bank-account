using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.Accounts.DTOs
{
    public class UpdateAccountRequest
    {
        public Guid OwnerId { get; set; }
        public AccountType Type { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
        public decimal? InterestRate { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
    }
}
