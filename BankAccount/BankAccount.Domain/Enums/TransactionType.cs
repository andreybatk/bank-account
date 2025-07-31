namespace BankAccount.Domain.Enums;

/// <summary>
/// Тип транзакции
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Списание
    /// </summary>
    Credit,
    /// <summary>
    /// Пополнение
    /// </summary>
    Debit
}