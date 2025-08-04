using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class CreateAccountCommandHandler(
    IAccountRepository accountRepository,
    IClientVerificationService clientVerificationService,
    ICurrencyService currencyService)
    : ICommandHandler<CreateAccountCommand, Guid>
{
    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var clientExists = await clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            throw new EntityNotFoundException("Клиент не найден.");

        var currencySupported = await currencyService.IsCurrencySupportedAsync(request.Currency);
        if (!currencySupported)
            throw new ValidationException("Валюта не поддерживается.");

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

        return await accountRepository.CreateAsync(account);
    }
}