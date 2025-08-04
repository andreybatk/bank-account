using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.Domain.Enums;

namespace BankAccount.BusinessLogic.AccountTransactions.Commands;

public sealed record CreateTransactionCommand(
    Guid AccountId,
    Guid? CounterpartyAccountId,
    decimal Amount,
    string Currency,
    ETransactionType Type,
    string Description,
    DateTime CreatedAt
) : ICommand<Guid>;