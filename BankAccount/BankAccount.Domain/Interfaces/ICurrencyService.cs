namespace BankAccount.Domain.Interfaces;

public interface ICurrencyService
{
    /// <summary>
    /// Проверяет, поддерживается ли указанная валюта (ISO 4217)
    /// </summary>
    // ReSharper disable once UnusedParameter.Global Параметр оставлен для дальнейшего использования
    Task<bool> IsCurrencySupportedAsync(string currencyCode);
}