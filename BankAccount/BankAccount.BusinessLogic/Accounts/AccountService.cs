using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.BusinessLogic.Accounts.DTOs;
using BankAccount.BusinessLogic.Accounts.Mappers;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Guid> CreateAccountAsync(CreateAccountCommand command)
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                OwnerId = command.OwnerId,
                Type = command.Type,
                Currency = command.Currency,
                Balance = command.InitialBalance,
                InterestRate = command.InterestRate,
                OpenDate = command.OpenDate,
                CloseDate = command.CloseDate
            };

            return await _accountRepository.CreateAsync(account);
        }

        public async Task<Guid> UpdateAccountAsync(UpdateAccountCommand command)
        {
            var account = new Account
            {
                Id = command.AccountId,
                OwnerId = command.OwnerId,
                Type = command.Type,
                Currency = command.Currency,
                Balance = command.InitialBalance,
                InterestRate = command.InterestRate,
                OpenDate = command.OpenDate,
                CloseDate = command.CloseDate
            };

            return await _accountRepository.UpdateAsync(account);
        }

        public async Task<Guid> DeleteAccountAsync(Guid accountId)
        {
            return await _accountRepository.DeleteAsync(accountId);
        }

        public async Task<List<AccountResponse>> GetAccountsByOwnerIdAsync(Guid ownerId)
        {
            return AccountMapper.ToResponseList(await _accountRepository.GetAllByOwnerIdAsync(ownerId));
        }

        public async Task<AccountStatementResponse> GetAccountStatementAsync(Guid ownerId, Guid accountId)
        {
            return AccountMapper.ToStatementResponse(await _accountRepository.GetByOwnerIdAsync(ownerId, accountId));
        }
    }
}
