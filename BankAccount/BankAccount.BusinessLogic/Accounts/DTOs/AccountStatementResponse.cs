// ReSharper disable UnusedAutoPropertyAccessor.Global Используются для сериализации ответа API
using BankAccount.BusinessLogic.AccountTransactions.DTOs;
using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.Accounts.DTOs;

/// <summary>
/// Выписка счёта
/// </summary>
public class AccountStatementResponse
{
    /// <summary>
    /// Идентификатор счёта
    /// </summary>
    public Guid Id { get; init; }
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
    /// <summary>
    /// Транзакции аккаунта
    /// </summary>
    public List<TransactionResponse> Transactions { get; init; } = [];
}