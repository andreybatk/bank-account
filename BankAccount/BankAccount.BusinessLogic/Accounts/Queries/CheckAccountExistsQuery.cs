using BankAccount.BusinessLogic.Abstractions.Messaging;

namespace BankAccount.BusinessLogic.Accounts.Queries;

public sealed record CheckAccountExistsQuery(Guid OwnerId) : IQuery<bool>;