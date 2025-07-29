namespace BankAccount.Domain.Interfaces;

public interface ICurrencyService
{
    /// <summary>
    /// Проверяет, поддерживается ли указанная валюта (ISO 4217)
    /// </summary>
    Task<bool> IsCurrencySupportedAsync(string currencyCode);
}