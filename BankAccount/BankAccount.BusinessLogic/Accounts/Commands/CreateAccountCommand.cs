using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.Accounts.Commands;

/// <summary>
/// Команда на создание счёта
/// </summary>
/// <param name="OwnerId">Идентификатор владельца</param>
/// <param name="Type">Тип аккаунта</param>
/// <param name="Currency">Валюта (ISO4217)</param>
/// <param name="InitialBalance">Начальный баланс</param>
/// <param name="InterestRate">Процентная ставка (для Deposit/Credit)</param>
/// <param name="OpenDate">Дата открытия</param>
/// <param name="CloseDate">Дата закрытия (опционально)</param>
public sealed record CreateAccountCommand(
    Guid OwnerId,
    EAccountType Type,
    string Currency,
    decimal InitialBalance,
    decimal? InterestRate,
    DateTime OpenDate,
    DateTime? CloseDate
) : ICommand<Guid>;