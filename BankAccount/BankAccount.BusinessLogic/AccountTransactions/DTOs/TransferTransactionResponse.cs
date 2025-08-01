// ReSharper disable UnusedAutoPropertyAccessor.Global Используются для сериализации ответа API
namespace BankAccount.BusinessLogic.AccountTransactions.DTOs;

/// <summary>
/// Результат перевода средств
/// </summary>
public class TransferTransactionResponse
{
    /// <summary>
    /// Идентификатор транзакции по пополнению
    /// </summary>
    public Guid DebitTransactionId { get; init; }
    /// <summary>
    /// Идентификатор транзакции по списанию
    /// </summary>
    public Guid CreditTransactionId { get; init; }
}