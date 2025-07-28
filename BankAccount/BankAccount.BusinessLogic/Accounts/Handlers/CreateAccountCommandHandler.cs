using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;

namespace BankAccount.BusinessLogic.Accounts.Handlers
{
    public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Guid>
    {
        private readonly IAccountService _accountService;

        public CreateAccountCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var accountId = await _accountService.CreateAccountAsync(request);

            return accountId;
        }
    }
}
