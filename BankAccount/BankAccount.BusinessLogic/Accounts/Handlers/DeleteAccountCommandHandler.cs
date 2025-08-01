using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class DeleteAccountCommandHandler(IAccountRepository accountRepository)
    : ICommandHandler<DeleteAccountCommand, Guid>
{
    public async Task<Guid> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var resultId = await accountRepository.DeleteAsync(request.AccountId);

        if (resultId is null)
            throw new EntityNotFoundException("Счёт с таким AccountId не найден.");

        return resultId.Value;
    }
}