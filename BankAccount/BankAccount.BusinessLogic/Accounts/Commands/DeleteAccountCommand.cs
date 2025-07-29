using BankAccount.BusinessLogic.Abstractions.Messaging;

namespace BankAccount.BusinessLogic.Accounts.Commands;

public sealed record DeleteAccountCommand(Guid AccountId) : ICommand<Guid>;