using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Services;

public class CurrencyService : ICurrencyService
{
    public Task<bool> IsCurrencySupportedAsync(string currencyCode)
    {
        return Task.FromResult(true);
    }
}