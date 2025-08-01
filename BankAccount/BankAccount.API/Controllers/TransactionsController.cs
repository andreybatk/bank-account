using BankAccount.BusinessLogic.AccountTransactions.Commands;
using BankAccount.BusinessLogic.AccountTransactions.DTOs;
using BankAccount.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.API.Controllers;

/// <summary>
/// Транзакции
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransactionsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создать транзакцию
    /// </summary>
    /// <param name="request">Тело запроса</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(MbResult<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request, CancellationToken token)
    {
        var command = new CreateTransactionCommand(
            request.AccountId,
            request.CounterpartyAccountId,
            request.Amount,
            request.Currency,
            request.Type,
            request.Description,
            request.CreatedAt);

        var transactionId = await mediator.Send(command, token);

        return Ok(MbResult<Guid>.Success(transactionId));
    }

    /// <summary>
    /// Создать транзакции по переводу средств
    /// </summary>
    /// <param name="request">Тело запроса</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
    [HttpPost("transfers")]
    [ProducesResponseType(typeof(MbResult<TransferTransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransferTransaction([FromBody] CreateTransferTransactionRequest request, CancellationToken token)
    {
        var command = new CreateTransferTransactionCommand(
            request.AccountIdFrom,
            request.AccountIdTo,
            request.Amount,
            request.Currency,
            request.Description,
            request.CreatedAt);

        var transactionIds = await mediator.Send(command, token);

        return Ok(MbResult<TransferTransactionResponse>.Success(transactionIds));
    }
}