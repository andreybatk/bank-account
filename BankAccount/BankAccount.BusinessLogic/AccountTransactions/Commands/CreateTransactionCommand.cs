using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.AccountTransactions.Commands;

/// <summary>
/// Команда на создание транзакции
/// </summary>
/// <param name="AccountId">Идентификатор счёта</param>
/// <param name="CounterpartyAccountId">Идентификатор контрагента (опционально)</param>
/// <param name="Amount">Сумма</param>
/// <param name="Currency">Валюта (ISO 4217)</param>
/// <param name="Type">Тип транзакции</param>
/// <param name="Description">Описание транзакции</param>
/// <param name="CreatedAt">Дата создания</param>
public sealed record CreateTransactionCommand(
    Guid AccountId,
    Guid? CounterpartyAccountId,
    decimal Amount,
    string Currency,
    ETransactionType Type,
    string Description,
    DateTime CreatedAt
) : ICommand<Guid>;