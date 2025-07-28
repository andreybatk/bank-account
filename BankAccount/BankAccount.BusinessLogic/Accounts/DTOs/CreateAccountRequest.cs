namespace BankAccount.BusinessLogic.Accounts.DTOs
{
    public class CreateAccountRequest
    {
        public Guid OwnerId { get; set; }
        public Domain.Enums.AccountType Type { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
        public decimal? InterestRate { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
    }
}