using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.Accounts.Commands
{
    public sealed record UpdateAccountCommand(
    Guid AccountId,
    Guid OwnerId,
    AccountType Type,
    string Currency,
    decimal InitialBalance,
    decimal? InterestRate,
    DateTime OpenDate,
    DateTime? CloseDate
) : ICommand<Guid>;
}
