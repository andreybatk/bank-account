using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.DTOs;

namespace BankAccount.BusinessLogic.Accounts.Queries;

/// <summary>
/// Запрос на получение выписки по счёту
/// </summary>
/// <param name="OwnerId">Идентификатор владельца</param>
/// <param name="AccountId">Идентификатор счёта</param>
public sealed record GetAccountStatementQuery(Guid OwnerId, Guid AccountId)
    : IQuery<AccountStatementResponse>;