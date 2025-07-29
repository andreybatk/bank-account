using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.AccountTransactions.DTOs;

namespace BankAccount.BusinessLogic.AccountTransactions.Commands;

public sealed record CreateTransferTransactionCommand(
    Guid AccountIdFrom,
    Guid AccountIdTo,
    decimal Amount,
    string Currency,
    string Description,
    DateTime CreatedAt
) : ICommand<TransferTransactionResponse>;
