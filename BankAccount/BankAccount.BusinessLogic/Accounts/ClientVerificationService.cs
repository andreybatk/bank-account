using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts
{
    public class ClientVerificationService : IClientVerificationService
    {
        public Task<bool> ClientExistsAsync(Guid ownerId)
        {
            return Task.FromResult(true);
        }
    }
}