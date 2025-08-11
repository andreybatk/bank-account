// ReSharper disable PropertyCanBeMadeInitOnly.Global Сущность, должна иметь возможность полностью измениться
using BankAccount.Domain.Enums;

namespace BankAccount.Domain.Entities;

public class AccountTransaction
{
    /// <summary>
    /// Идентификатор транзакции
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Идентификатор счёта
    /// </summary>
    public Guid AccountId { get; set; }
    /// <summary>
    /// Идентификатор контрагента (опционально)
    /// </summary>
    public Guid? CounterpartyAccountId { get; set; }
    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Amount { get; set; }
    /// <summary>
    /// Валюта (ISO 4217)
    /// </summary>
    public string Currency { get; set; } = string.Empty;
    /// <summary>
    /// Тип транзакции
    /// </summary>
    public ETransactionType Type { get; set; }
    /// <summary>
    /// Описание транзакции
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }
}