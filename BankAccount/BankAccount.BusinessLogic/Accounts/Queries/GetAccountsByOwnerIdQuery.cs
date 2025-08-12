using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.DTOs;

namespace BankAccount.BusinessLogic.Accounts.Queries;

/// <summary>
/// Запрос на получение всех аккаунтов у владельца
/// </summary>
/// <param name="OwnerId">Идентификатор владельца</param>
public sealed record GetAccountsByOwnerIdQuery(Guid OwnerId) : IQuery<List<AccountResponse>>;