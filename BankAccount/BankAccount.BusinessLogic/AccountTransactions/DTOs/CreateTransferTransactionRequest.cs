namespace BankAccount.BusinessLogic.AccountTransactions.DTOs;

/// <summary>
/// Запрос на перевод средств
/// </summary>
public class CreateTransferTransactionRequest
{
    /// <summary>
    /// Идентификатор счёта отправителя
    /// </summary>
    public Guid AccountIdFrom { get; init; }
    /// <summary>
    /// Идентификатор счёта получателя
    /// </summary>
    public Guid AccountIdTo { get; init; }
    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Amount { get; init; }
    /// <summary>
    /// Валюта (ISO 4217)
    /// </summary>
    public string Currency { get; init; } = string.Empty;
    /// <summary>
    /// Описание транзакции
    /// </summary>
    public string Description { get; init; } = string.Empty;
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
