using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IClientVerificationService _clientVerificationService;
    private readonly ICurrencyService _currencyService;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, IClientVerificationService clientVerificationService, ICurrencyService currencyService)
    {
        _accountRepository = accountRepository;
        _clientVerificationService = clientVerificationService;
        _currencyService = currencyService;
    }

    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var clientExists = await _clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            throw new EntityNotFoundException("Клиент с таким OwnerId не найден.");

        var currencySupported = await _currencyService.IsCurrencySupportedAsync(request.Currency);
        if (!currencySupported)
            throw new ValidationException($"Валюта '{request.Currency}' не поддерживается.");

        var account = new Account
        {
            Id = Guid.NewGuid(),
            OwnerId = request.OwnerId,
            Type = request.Type,
            Currency = request.Currency,
            Balance = request.InitialBalance,
            InterestRate = request.InterestRate,
            OpenDate = request.OpenDate,
            CloseDate = request.CloseDate
        };

        return await _accountRepository.CreateAsync(account);
    }
}