using BankAccount.BusinessLogic.Abstractions.Messaging;

namespace BankAccount.BusinessLogic.Accounts.Queries;

/// <summary>
/// Запрос на проверку на наличие счёта
/// </summary>
/// <param name="OwnerId">Идентификатор владельца</param>
public sealed record CheckAccountExistsQuery(Guid OwnerId) : IQuery<bool>;