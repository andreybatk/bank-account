// ReSharper disable UnusedAutoPropertyAccessor.Global Используется для десериализации JSON из запроса
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global Используется для десериализации JSON из запроса
using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.AccountTransactions.DTOs;

/// <summary>
/// Запрос на создание транзакции
/// </summary>
public class CreateTransactionRequest
{
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
    public TransactionType Type { get; init; }
    /// <summary>
    /// Описание транзакции
    /// </summary>
    public string Description { get; init; } = string.Empty;
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; init; }
}