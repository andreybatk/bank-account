// ReSharper disable UnusedAutoPropertyAccessor.Global Используются для сериализации ответа API
using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.AccountTransactions.DTOs;

/// <summary>
/// Транзакция
/// </summary>
public class TransactionResponse
{
    /// <summary>
    /// Идентификатор транзакции
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// Идентификатор счёта
    /// </summary>
    public Guid AccountId { get; init; }
    /// <summary>
    /// Идентификатор контрагента (опционально)
    /// </summary>
    public Guid? CounterpartyAccountId { get; init; }
    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Amount { get; init; }
    /// <summary>
    /// Валюта (ISO 4217)
    /// </summary>
    public string Currency { get; init; } = string.Empty;
    /// <summary>
    /// Тип транзакции
    /// </summary>
    public ETransactionType Type { get; set; }
    /// <summary>
    /// Описание транзакции
    /// </summary>
    public string Description { get; init; } = string.Empty;
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; init; }
}