using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class UpdateAccountCommandHandler : ICommandHandler<UpdateAccountCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IClientVerificationService _clientVerificationService;
    private readonly ICurrencyService _currencyService;

    public UpdateAccountCommandHandler(
        IAccountRepository accountRepository,
        IClientVerificationService clientVerificationService,
        ICurrencyService currencyService)
    {
        _accountRepository = accountRepository;
        _clientVerificationService = clientVerificationService;
        _currencyService = currencyService;
    }

    public async Task<Guid> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
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

        var account = new Account
        {
            Id = request.AccountId,
            OwnerId = request.OwnerId,
            Type = request.Type,
            Currency = request.Currency,
            Balance = request.InitialBalance,
            InterestRate = request.InterestRate,
            OpenDate = request.OpenDate,
            CloseDate = request.CloseDate
        };

        var resultGuid = await _accountRepository.UpdateAsync(account);

        if(resultGuid is null)
            throw new AccountNotFoundException(account.Id);

        return resultGuid.Value;
    }
}