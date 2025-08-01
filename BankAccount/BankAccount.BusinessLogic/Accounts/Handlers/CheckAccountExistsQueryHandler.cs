using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Queries;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class CheckAccountExistsQueryHandler(
    IAccountRepository accountRepository,
    IClientVerificationService clientVerificationService)
    : IQueryHandler<CheckAccountExistsQuery, bool>
{
    public async Task<bool> Handle(CheckAccountExistsQuery request, CancellationToken cancellationToken)
    {
        var clientExists = await clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            throw new EntityNotFoundException("Клиент с таким OwnerId не найден.");

        return await accountRepository.ExistsByOwnerIdAsync(request.OwnerId);
    }
}