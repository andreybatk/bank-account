using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.Accounts.DTOs;

public class AccountResponse
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public AccountType Type { get; init; }
    public string Currency { get; init; } = string.Empty;
    public decimal Balance { get; init; }
    public decimal? InterestRate { get; init; }
    public DateTime OpenDate { get; init; }
    public DateTime? CloseDate { get; init; }
}