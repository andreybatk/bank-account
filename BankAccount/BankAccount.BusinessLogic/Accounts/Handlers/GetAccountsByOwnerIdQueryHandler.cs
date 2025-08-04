using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.DTOs;
using BankAccount.BusinessLogic.Accounts.Mappers;
using BankAccount.BusinessLogic.Accounts.Queries;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class GetAccountsByOwnerIdQueryHandler(
    IAccountRepository accountRepository,
    IClientVerificationService clientVerificationService)
    : IQueryHandler<GetAccountsByOwnerIdQuery, List<AccountResponse>>
{
    public async Task<List<AccountResponse>> Handle(GetAccountsByOwnerIdQuery request, CancellationToken cancellationToken)
    {
        var clientExists = await clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            throw new EntityNotFoundException("Клиент не найден.");

        return AccountMapper.ToResponseList(await accountRepository.GetAllByOwnerIdAsync(request.OwnerId));
    }
}