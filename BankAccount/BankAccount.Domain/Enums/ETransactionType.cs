namespace BankAccount.Domain.Enums;

/// <summary>
/// Тип транзакции
/// </summary>
public enum ETransactionType
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