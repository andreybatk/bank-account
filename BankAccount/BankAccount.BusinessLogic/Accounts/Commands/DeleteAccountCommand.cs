using BankAccount.BusinessLogic.Abstractions.Messaging;

namespace BankAccount.BusinessLogic.Accounts.Commands;

/// <summary>
/// Команда на удаление счёта
/// </summary>
/// <param name="AccountId">Идентификатор счёта</param>
public sealed record DeleteAccountCommand(Guid AccountId) : ICommand<Guid>;