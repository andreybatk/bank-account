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
    /// <param name="command">Тело запроса</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(MbResult<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command, CancellationToken token)
    {
        var transactionId = await mediator.Send(command, token);

        return Ok(MbResult<Guid>.Success(transactionId));
    }

    /// <summary>
    /// Создать транзакции по переводу средств
    /// </summary>
    /// <param name="command">Тело запроса</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
    [HttpPost("transfers")]
    [ProducesResponseType(typeof(MbResult<TransferTransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransferTransaction([FromBody] CreateTransferTransactionCommand command, CancellationToken token)
    {
        var transactionIds = await mediator.Send(command, token);

        return Ok(MbResult<TransferTransactionResponse>.Success(transactionIds));
    }
}