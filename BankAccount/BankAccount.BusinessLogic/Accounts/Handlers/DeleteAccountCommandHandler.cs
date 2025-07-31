using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Exceptions;
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
        var resultId = await _accountRepository.DeleteAsync(request.AccountId);

        if (resultId is null)
            throw new EntityNotFoundException("Счёт с таким AccountId не найден.");

        return resultId.Value;
    }
}