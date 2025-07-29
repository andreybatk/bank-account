using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class DeleteAccountCommandHandler : ICommandHandler<DeleteAccountCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;

    public DeleteAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Guid> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        return await _accountRepository.DeleteAsync(request.AccountId);
    }
}