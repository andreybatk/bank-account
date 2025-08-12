using BankAccount.Domain.Entities;
using BankAccount.Domain.Interfaces;

namespace BankAccount.DataAccess.Repositories;

public class TransactionRepository(ApplicationDbContext context) : ITransactionRepository
{
    public async Task<Guid> RegisterTransactionAsync(AccountTransaction transaction)
    {
        transaction.Id = Guid.NewGuid();
        transaction.CreatedAt = DateTime.UtcNow;
        context.Transactions.Add(transaction);

        await context.SaveChangesAsync();
        return transaction.Id;
    }
}
