using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.BusinessLogic.Accounts.DTOs;
using BankAccount.BusinessLogic.Accounts.Queries;
using BankAccount.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace BankAccount.API.Controllers;

/// <summary>
/// Счета
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Создать счёт
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MbResult<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request, CancellationToken token)
    {
        var command = new CreateAccountCommand(
            request.OwnerId,
            request.Type,
            request.Currency,
            request.InitialBalance,
            request.InterestRate,
            request.OpenDate,
            request.CloseDate);

        var accountId = await _mediator.Send(command, token);

        return Ok(MbResult<Guid>.Success(accountId));
    }

    /// <summary>
    /// Обновить счёт
    /// </summary>
    [HttpPut("{accountId:guid}")]
    [ProducesResponseType(typeof(MbResult<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAccount(Guid accountId, [FromBody] UpdateAccountRequest request, CancellationToken token)
    {
        var command = new UpdateAccountCommand(
            accountId,
            request.OwnerId,
            request.Type,
            request.Currency,
            request.Balance,
            request.InterestRate,
            request.OpenDate,
            request.CloseDate);

        var updatedAccountId = await _mediator.Send(command, token);

        return Ok(MbResult<Guid>.Success(updatedAccountId));
    }

    /// <summary>
    /// Удалить счёт
    /// </summary>
    [HttpDelete("{accountId:guid}")]
    [ProducesResponseType(typeof(MbResult<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAccount(Guid accountId, CancellationToken token)
    {
        var command = new DeleteAccountCommand(accountId);

        var deletedId = await _mediator.Send(command, token);

        return Ok(MbResult<Guid>.Success(deletedId));
    }

    /// <summary>
    /// Получить все счета клиента
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(MbResult<List<AccountResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountsByOwnerId([FromQuery][Required] Guid ownerId, CancellationToken token)
    {
        var query = new GetAccountsByOwnerIdQuery(ownerId);

        var accounts = await _mediator.Send(query, token);

        if (accounts.Count == 0)
            return NotFound($"У владельца {ownerId} нет счетов.");

        return Ok(MbResult<List<AccountResponse>>.Success(accounts));
    }

    /// <summary>
    /// Получить выписку по счёту
    /// </summary>
    [HttpGet("{accountId:guid}")]
    [ProducesResponseType(typeof(MbResult<AccountStatementResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountStatement(Guid accountId, [FromQuery][Required] Guid ownerId, CancellationToken token)
    {
        var query = new GetAccountStatementQuery(ownerId, accountId);

        var statement = await _mediator.Send(query, token);

        return Ok(MbResult<AccountStatementResponse>.Success(statement));
    }

    /// <summary>
    /// Проверить существование счёта
    /// </summary>
    [HttpGet("exists")]
    [ProducesResponseType(typeof(MbResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckAccountExists([FromQuery][Required] Guid ownerId, CancellationToken cancellationToken)
    {
        var query = new CheckAccountExistsQuery(ownerId);

        var exists = await _mediator.Send(query, cancellationToken);

        return Ok(MbResult<bool>.Success(exists));
    }
}