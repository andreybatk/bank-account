using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.Accounts.DTOs;

/// <summary>
/// Запрос на обновление данных счёта
/// </summary>
public class UpdateAccountRequest
{
    /// <summary>
    /// Идентификатор владельца
    /// </summary>
    public Guid OwnerId { get; init; }
    /// <summary>
    /// Тип аккаунта
    /// </summary>
    public AccountType Type { get; init; }
    /// <summary>
    /// Валюта (ISO4217)
    /// </summary>
    public string Currency { get; init; } = string.Empty;
    /// <summary>
    /// Баланс
    /// </summary>
    public decimal Balance { get; init; }
    /// <summary>
    /// Процентная ставка (для Deposit/Credit)
    /// </summary>
    public decimal? InterestRate { get; init; }
    /// <summary>
    /// Дата открытия 
    /// </summary>
    public DateTime OpenDate { get; init; }
    /// <summary>
    /// Дата закрытия (опционально)
    /// </summary>
    public DateTime? CloseDate { get; init; }
}