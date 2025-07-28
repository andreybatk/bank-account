using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts
{
    public class CurrencyService : ICurrencyService
    {
        public Task<bool> IsCurrencySupportedAsync(string currencyCode)
        {
            return Task.FromResult(true);
        }
    }
}