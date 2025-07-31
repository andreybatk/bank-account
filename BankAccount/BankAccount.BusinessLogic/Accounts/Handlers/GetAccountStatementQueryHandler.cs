using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.DTOs;
using BankAccount.BusinessLogic.Accounts.Mappers;
using BankAccount.BusinessLogic.Accounts.Queries;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class GetAccountStatementQueryHandler
    : IQueryHandler<GetAccountStatementQuery, AccountStatementResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IClientVerificationService _clientVerificationService;

    public GetAccountStatementQueryHandler(IAccountRepository accountRepository, IClientVerificationService clientVerificationService)
    {
        _accountRepository = accountRepository;
        _clientVerificationService = clientVerificationService;
    }

    public async Task<AccountStatementResponse> Handle(GetAccountStatementQuery request, CancellationToken cancellationToken)
    {
        var clientExists = await _clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            throw new EntityNotFoundException("Клиент с таким OwnerId не найден.");

        var account = await _accountRepository.GetByOwnerIdAsync(request.OwnerId, request.AccountId);

        if(account is null)
            throw new EntityNotFoundException("Счёт с таким AccountId не найден.");

        return AccountMapper.ToStatementResponse(account);
    }
}