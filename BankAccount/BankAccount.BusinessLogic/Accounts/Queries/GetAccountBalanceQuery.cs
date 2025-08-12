using BankAccount.BusinessLogic.Abstractions.Messaging;

namespace BankAccount.BusinessLogic.Accounts.Queries;

/// <summary>
/// Запрос на получение баланса счёта
/// </summary>
/// <param name="AccountId">Идентификатор счёта</param>
public sealed record GetAccountBalanceQuery(Guid AccountId)
    : IQuery<decimal>;