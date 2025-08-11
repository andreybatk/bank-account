using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.Accounts.Commands;

public sealed record CreateAccountCommand(
    Guid OwnerId,
    EAccountType Type,
    string Currency,
    decimal InitialBalance,
    decimal? InterestRate,
    DateTime OpenDate,
    DateTime? CloseDate
) : ICommand<Guid>;