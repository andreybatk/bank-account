using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.BusinessLogic.Accounts.DTOs;
using BankAccount.BusinessLogic.Accounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankAccount.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
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

            return Ok(accountId);
        }

        [HttpPut("{accountId:guid}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAccount(Guid accountId, [FromBody] UpdateAccountRequest request, CancellationToken token)
        {
            var command = new UpdateAccountCommand(
                accountId,
                request.OwnerId,
                request.Type,
                request.Currency,
                request.InitialBalance,
                request.InterestRate,
                request.OpenDate,
                request.CloseDate);

            var updatedAccountId = await _mediator.Send(command, token);

            return Ok(updatedAccountId);
        }

        [HttpDelete("{accountId:guid}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAccount(Guid accountId, CancellationToken token)
        {
            var command = new DeleteAccountCommand(accountId);

            var deletedId = await _mediator.Send(command, token);

            return Ok(deletedId);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<AccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountsByOwnerId([FromQuery][Required] Guid ownerId, CancellationToken token)
        {
            var query = new GetAccountsByOwnerIdQuery(ownerId);

            var accounts = await _mediator.Send(query, token);

            if (accounts.Count == 0)
                return NotFound($"У владельца {ownerId} нет счетов.");

            return Ok(accounts);
        }

        [HttpGet("{accountId:guid}")]
        [ProducesResponseType(typeof(AccountStatementResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountStatement(Guid accountId, [FromQuery][Required] Guid ownerId, CancellationToken token)
        {
            var query = new GetAccountStatementQuery(ownerId, accountId);

            var statement = await _mediator.Send(query, token);

            if (statement == null)
                return NotFound($"Счет {accountId} для владельца {ownerId} не найден.");

            return Ok(statement);
        }
    }
}
