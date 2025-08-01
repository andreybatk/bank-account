using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class UpdateAccountCommandHandler(
    IAccountRepository accountRepository,
    IClientVerificationService clientVerificationService,
    ICurrencyService currencyService)
    : ICommandHandler<UpdateAccountCommand, Guid>
{
    public async Task<Guid> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var clientExists = await clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            throw new EntityNotFoundException("Клиент с таким OwnerId не найден.");

        var currencySupported = await currencyService.IsCurrencySupportedAsync(request.Currency);
        if (!currencySupported)
            throw new ValidationException($"Валюта '{request.Currency}' не поддерживается.");

        var account = new Account
        {
            Id = request.AccountId,
            OwnerId = request.OwnerId,
            Type = request.Type,
            Currency = request.Currency,
            Balance = request.Balance,
            InterestRate = request.InterestRate,
            OpenDate = request.OpenDate,
            CloseDate = request.CloseDate
        };

        var resultId = await accountRepository.UpdateAsync(account);

        if(resultId is null)
            throw new EntityNotFoundException("Счёт с таким AccountId не найден.");

        return resultId.Value;
    }
}