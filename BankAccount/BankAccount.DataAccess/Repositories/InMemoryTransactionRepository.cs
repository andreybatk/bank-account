using BankAccount.Domain.Entities;
using BankAccount.Domain.Interfaces;

namespace BankAccount.DataAccess.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly List<AccountTransaction> _transactions = [];

    public Task<AccountTransaction> RegisterTransactionAsync(AccountTransaction transaction)
    {
        _transactions.Add(transaction);
        return Task.FromResult(transaction);
    }
}