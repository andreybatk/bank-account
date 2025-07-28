using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.DTOs;

namespace BankAccount.BusinessLogic.Accounts.Queries
{
    public sealed record GetAccountStatementQuery(Guid OwnerId, Guid AccountId)
        : IQuery<AccountStatementResponse>;
}