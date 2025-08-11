// ReSharper disable CollectionNeverQueried.Local Класс-заглушка, коллекция оставлена для дальнейшего использования
using BankAccount.Domain.Entities;
using BankAccount.Domain.Interfaces;

namespace BankAccount.DataAccess.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly List<AccountTransaction> _transactions = [];

    public Task<Guid> RegisterTransactionAsync(AccountTransaction transaction)
    {
        _transactions.Add(transaction);
        return Task.FromResult(transaction.Id);
    }
}