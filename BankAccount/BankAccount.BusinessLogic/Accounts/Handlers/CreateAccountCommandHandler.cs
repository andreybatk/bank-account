using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Guid>
{
    private readonly IAccountService _accountService;
    private readonly IClientVerificationService _clientVerificationService;
    private readonly ICurrencyService _currencyService;

    public CreateAccountCommandHandler(IAccountService accountService, IClientVerificationService clientVerificationService, ICurrencyService currencyService)
    {
        _accountService = accountService;
        _clientVerificationService = clientVerificationService;
        _currencyService = currencyService;
    }

    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        var clientExists = await _clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            errors.Add(nameof(request.OwnerId), ["Клиент с таким OwnerId не найден."]);

        var currencySupported = await _currencyService.IsCurrencySupportedAsync(request.Currency);
        if (!currencySupported)
            errors.Add(nameof(request.Currency), [$"Валюта '{request.Currency}' не поддерживается."]);

        if (errors.Count != 0)
            throw new ValidationException(errors);

        var accountId = await _accountService.CreateAccountAsync(request);

        return accountId;
    }
}