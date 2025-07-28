using BankAccount.BusinessLogic.Accounts.DTOs;
using BankAccount.BusinessLogic.AccountTransactions.DTOs;
using BankAccount.BusinessLogic.AccountTransactions.Mappers;
using BankAccount.Domain.Entities;

namespace BankAccount.BusinessLogic.Accounts.Mappers
{
    public static class AccountMapper
    {
        public static AccountResponse ToResponse(Account account) =>
            new()
            {
                Id = account.Id,
                OwnerId = account.OwnerId,
                Type = account.Type,
                Currency = account.Currency,
                Balance = account.Balance,
                InterestRate = account.InterestRate,
                OpenDate = account.OpenDate,
                CloseDate = account.CloseDate
            };

        public static List<AccountResponse> ToResponseList(List<Account> accounts) =>
            accounts.Select(ToResponse).ToList();

        public static AccountStatementResponse ToStatementResponse(Account account) =>
             new()
             {
                 Id = account.Id,
                 OwnerId = account.OwnerId,
                 Type = account.Type,
                 Currency = account.Currency,
                 Balance = account.Balance,
                 InterestRate = account.InterestRate,
                 OpenDate = account.OpenDate,
                 CloseDate = account.CloseDate,
                 Transactions = account.Transactions != null
                     ? TransactionMapper.ToResponseList(account.Transactions)
                     : new List<TransactionResponse>()
             };
    }
}
