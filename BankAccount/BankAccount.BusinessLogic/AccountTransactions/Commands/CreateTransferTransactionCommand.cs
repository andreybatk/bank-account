using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.AccountTransactions.DTOs;

namespace BankAccount.BusinessLogic.AccountTransactions.Commands;

/// <summary>
/// Команда на создание транзакции по переводу средств
/// </summary>
/// <param name="AccountIdFrom">Идентификатор счёта отправителя</param>
/// <param name="AccountIdTo">Идентификатор счёта получателя</param>
/// <param name="Amount">Сумма</param>
/// <param name="Currency">Валюта (ISO 4217)</param>
/// <param name="Description">Описание транзакции</param>
/// <param name="CreatedAt">Дата создания</param>
public sealed record CreateTransferTransactionCommand(
    Guid AccountIdFrom,
    Guid AccountIdTo,
    decimal Amount,
    string Currency,
    string Description,
    DateTime CreatedAt
) : ICommand<TransferTransactionResponse>;
