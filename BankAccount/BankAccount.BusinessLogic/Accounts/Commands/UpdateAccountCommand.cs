using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.Accounts.Commands;
/// <summary>
/// Команда на обновление счёта
/// </summary>
/// <param name="AccountId">Идентификатор счёта</param>
/// <param name="OwnerId">Идентификатор владельца</param>
/// <param name="Type">Тип аккаунта</param>
/// <param name="Currency">Валюта (ISO4217)</param>
/// <param name="Balance">Баланс</param>
/// <param name="InterestRate">Процентная ставка (для Deposit/Credit)</param>
/// <param name="OpenDate">Дата открытия</param>
/// <param name="CloseDate">Дата закрытия (опционально)</param>
public sealed record UpdateAccountCommand(
    Guid AccountId,
    Guid OwnerId,
    EAccountType Type,
    string Currency,
    decimal Balance,
    decimal? InterestRate,
    DateTime OpenDate,
    DateTime? CloseDate
) : ICommand<Guid>;