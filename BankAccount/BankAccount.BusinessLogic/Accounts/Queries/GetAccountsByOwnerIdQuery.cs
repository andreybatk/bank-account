using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.DTOs;

namespace BankAccount.BusinessLogic.Accounts.Queries;

public sealed record GetAccountsByOwnerIdQuery(Guid OwnerId) : IQuery<List<AccountResponse>>;