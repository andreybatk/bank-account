namespace BankAccount.Domain.Enums;

/// <summary>
/// Тип счёта
/// </summary>
public enum AccountType
{
    /// <summary>
    /// Расчетный 
    /// </summary>
    Checking,
    /// <summary>
    /// Депозитный
    /// </summary>
    Deposit,
    /// <summary>
    /// Кредитный
    /// </summary>
    Credit
}