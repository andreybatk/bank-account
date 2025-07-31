using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Queries;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class CheckAccountExistsQueryHandler : IQueryHandler<CheckAccountExistsQuery, bool>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IClientVerificationService _clientVerificationService;

    public CheckAccountExistsQueryHandler(IAccountRepository accountRepository, IClientVerificationService clientVerificationService)
    {
        _accountRepository = accountRepository;
        _clientVerificationService = clientVerificationService;
    }

    public async Task<bool> Handle(CheckAccountExistsQuery request, CancellationToken cancellationToken)
    {
        var clientExists = await _clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            throw new EntityNotFoundException("Клиент с таким OwnerId не найден.");

        return await _accountRepository.ExistsByOwnerIdAsync(request.OwnerId);
    }
}