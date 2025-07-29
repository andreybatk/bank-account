using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.DTOs;
using BankAccount.BusinessLogic.Accounts.Mappers;
using BankAccount.BusinessLogic.Accounts.Queries;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers;

public class GetAccountsByOwnerIdQueryHandler : IQueryHandler<GetAccountsByOwnerIdQuery, List<AccountResponse>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IClientVerificationService _clientVerificationService;

    public GetAccountsByOwnerIdQueryHandler(IAccountRepository accountRepository, IClientVerificationService clientVerificationService)
    {
        _accountRepository = accountRepository;
        _clientVerificationService = clientVerificationService;
    }

    public async Task<List<AccountResponse>> Handle(GetAccountsByOwnerIdQuery request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        var clientExists = await _clientVerificationService.ClientExistsAsync(request.OwnerId);
        if (!clientExists)
            errors.Add(nameof(request.OwnerId), ["Клиент с таким OwnerId не найден."]);

        if (errors.Count != 0)
            throw new ValidationException(errors);

        return AccountMapper.ToResponseList(await _accountRepository.GetAllByOwnerIdAsync(request.OwnerId));
    }
}