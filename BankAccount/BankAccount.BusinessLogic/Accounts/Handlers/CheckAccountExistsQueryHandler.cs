using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Queries;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class CheckAccountExistsQueryHandler : IQueryHandler<CheckAccountExistsQuery, bool>
{
    private readonly IAccountService _accountService;
    private readonly IClientVerificationService _clientVerificationService;

    public CheckAccountExistsQueryHandler(IAccountService accountService, IClientVerificationService clientVerificationService)
    {
        _accountService = accountService;
        _clientVerificationService = clientVerificationService;
    }

    public async Task<bool> Handle(CheckAccountExistsQuery request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        var clientExists = await _clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            errors.Add(nameof(request.OwnerId), ["Клиент с таким OwnerId не найден."]);

        if (errors.Count != 0)
            throw new ValidationException(errors);

        return await _accountService.AccountExistsAsync(request.OwnerId);
    }
}