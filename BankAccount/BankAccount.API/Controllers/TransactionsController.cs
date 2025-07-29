using BankAccount.BusinessLogic.AccountTransactions.Commands;
using BankAccount.BusinessLogic.AccountTransactions.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
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

        var transactionId = await _mediator.Send(command, token);

        return Ok(transactionId);
    }

    [HttpPost("transfers")]
    [ProducesResponseType(typeof(TransferTransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransferTransaction([FromBody] CreateTransferTransactionRequest request, CancellationToken token)
    {
        var command = new CreateTransferTransactionCommand(
            request.AccountIdFrom,
            request.AccountIdTo,
            request.Amount,
            request.Currency,
            request.Description,
            request.CreatedAt);

        var transactionIds = await _mediator.Send(command, token);

        return Ok(transactionIds);
    }
}