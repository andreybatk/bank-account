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
public class AccountsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создать счет
    /// </summary>
    /// <param name="command">Тело запроса</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(MbResult<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command, CancellationToken token)
    {
        var accountId = await mediator.Send(command, token);

        return Ok(MbResult<Guid>.Success(accountId));
    }

    /// <summary>
    /// Обновить счет
    /// </summary>
    /// <param name="accountId">Id счёта</param>
    /// <param name="request">Тело запроса</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
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

        var updatedAccountId = await mediator.Send(command, token);

        return Ok(MbResult<Guid>.Success(updatedAccountId));
    }

    /// <summary>
    /// Удалить счёт
    /// </summary>
    /// <param name="accountId">Id счёта</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
    [HttpDelete("{accountId:guid}")]
    [ProducesResponseType(typeof(MbResult<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAccount(Guid accountId, CancellationToken token)
    {
        var command = new DeleteAccountCommand(accountId);

        var deletedId = await mediator.Send(command, token);

        return Ok(MbResult<Guid>.Success(deletedId));
    }

    /// <summary>
    /// Получить все счета клиента
    /// </summary>
    /// <param name="ownerId">Id клиента</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(MbResult<List<AccountResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountsByOwnerId([FromQuery][Required] Guid ownerId, CancellationToken token)
    {
        var query = new GetAccountsByOwnerIdQuery(ownerId);

        var accounts = await mediator.Send(query, token);

        if (accounts.Count == 0)
            return NotFound($"У владельца {ownerId} нет счетов.");

        return Ok(MbResult<List<AccountResponse>>.Success(accounts));
    }

    /// <summary>
    /// Получить выписку по счёту
    /// </summary>
    /// <param name="accountId">Id счёта</param>
    /// <param name="ownerId">Id клиента</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
    [HttpGet("{accountId:guid}")]
    [ProducesResponseType(typeof(MbResult<AccountStatementResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountStatement(Guid accountId, [FromQuery][Required] Guid ownerId, CancellationToken token)
    {
        var query = new GetAccountStatementQuery(ownerId, accountId);

        var statement = await mediator.Send(query, token);

        return Ok(MbResult<AccountStatementResponse>.Success(statement));
    }

    /// <summary>
    /// Получить баланс по счёту
    /// </summary>
    /// <param name="accountId">Id счёта</param>
    /// <param name="token">Cancellation Token</param>
    /// <returns></returns>
    [HttpGet("{accountId:guid}/balance")]
    [ProducesResponseType(typeof(MbResult<decimal>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountBalance(Guid accountId, CancellationToken token)
    {
        var query = new GetAccountBalanceQuery(accountId);

        var balance = await mediator.Send(query, token);

        return Ok(MbResult<decimal>.Success(balance));
    }

    /// <summary>
    /// Проверить существование счёта
    /// </summary>
    /// <param name="ownerId">Id клиента</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    [HttpGet("exists")]
    [ProducesResponseType(typeof(MbResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MbResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckAccountExists([FromQuery][Required] Guid ownerId, CancellationToken cancellationToken)
    {
        var query = new CheckAccountExistsQuery(ownerId);

        var exists = await mediator.Send(query, cancellationToken);

        return Ok(MbResult<bool>.Success(exists));
    }
}