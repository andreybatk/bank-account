using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;
using MediatR;

namespace BankAccount.BusinessLogic.Accounts.Handlers
{
    public class DeleteAccountCommandHandler : ICommandHandler<DeleteAccountCommand, Guid>
    {
        private readonly IAccountService _accountService;

        public DeleteAccountCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Guid> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var deletedId = await _accountService.DeleteAccountAsync(request.AccountId);

            return deletedId;
        }
    }
}
