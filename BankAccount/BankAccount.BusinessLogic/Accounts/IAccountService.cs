using BankAccount.BusinessLogic.Accounts.Commands;

namespace BankAccount.BusinessLogic.Accounts
{
    public interface IAccountService
    {
        Task<Guid> CreateAccountAsync(CreateAccountCommand command);
    }
}