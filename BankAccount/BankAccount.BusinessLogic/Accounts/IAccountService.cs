using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.BusinessLogic.Accounts.DTOs;
using BankAccount.Domain.Entities;

namespace BankAccount.BusinessLogic.Accounts
{
    public interface IAccountService
    {
        Task<Guid> CreateAccountAsync(CreateAccountCommand command);
        Task<Guid> UpdateAccountAsync(UpdateAccountCommand command);
        Task<Guid> DeleteAccountAsync(Guid accountId);
        Task<List<AccountResponse>> GetAccountsByOwnerIdAsync(Guid ownerId);
        Task<AccountStatementResponse> GetAccountStatementAsync(Guid ownerId, Guid accountId);
    }
}