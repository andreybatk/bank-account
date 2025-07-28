using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                OpenDate = DateTime.UtcNow
            };

            // Дополнительная бизнес-логика может быть здесь, например:
            // Проверка, что у клиента нет дубликатов и т.п.

            return await _accountRepository.CreateAsync(account);
        }
    }
}
