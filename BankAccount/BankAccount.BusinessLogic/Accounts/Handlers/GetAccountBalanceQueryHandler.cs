using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Queries;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class GetAccountBalanceQueryHandler(
    IAccountRepository accountRepository)
    : IQueryHandler<GetAccountBalanceQuery, decimal>
{
    public async Task<decimal> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(request.AccountId);

        if(account is null)
            throw new EntityNotFoundException("Счёт не найден.");

        return account.Balance;
    }
}