using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.DTOs;
using BankAccount.BusinessLogic.Accounts.Mappers;
using BankAccount.BusinessLogic.Accounts.Queries;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class GetAccountStatementQueryHandler(
    IAccountRepository accountRepository,
    IClientVerificationService clientVerificationService)
    : IQueryHandler<GetAccountStatementQuery, AccountStatementResponse>
{
    public async Task<AccountStatementResponse> Handle(GetAccountStatementQuery request, CancellationToken cancellationToken)
    {
        var clientExists = await clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            throw new EntityNotFoundException("Клиент с таким OwnerId не найден.");

        var account = await accountRepository.GetByOwnerIdAsync(request.OwnerId, request.AccountId);

        if(account is null)
            throw new EntityNotFoundException("Счёт с таким AccountId не найден.");

        return AccountMapper.ToStatementResponse(account);
    }
}