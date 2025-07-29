using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Services;

public class ClientVerificationService : IClientVerificationService
{
    public Task<bool> ClientExistsAsync(Guid ownerId)
    {
        return Task.FromResult(true);
    }
}