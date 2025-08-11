using BankAccount.Domain.Entities;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.DataAccess.Repositories;

public class AccountRepository(ApplicationDbContext context) : IAccountRepository
{
    public async Task<Guid> CreateAsync(Account account)
    {
        account.Id = Guid.NewGuid();
        context.Accounts.Add(account);
        await context.SaveChangesAsync();
        return account.Id;
    }

    public async Task<Guid?> UpdateAsync(Account account)
    {
        var existing = await context.Accounts.FindAsync(account.Id);
        if (existing == null)
            return null;

        context.Entry(existing).CurrentValues.SetValues(account);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("Данные были изменены другим пользователем. Пожалуйста, обновите данные и повторите операцию.");
        }

        return account.Id;
    }

    public async Task<Guid?> DeleteAsync(Guid accountId)
    {
        var entity = await context.Accounts.FindAsync(accountId);
        if (entity == null)
            return null;

        context.Accounts.Remove(entity);
        await context.SaveChangesAsync();
        return accountId;
    }

    public async Task<Account?> GetByIdAsync(Guid accountId)
    {
        return await context.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == accountId);
    }

    public async Task<List<Account>> GetAllByOwnerIdAsync(Guid ownerId)
    {
        return await context.Accounts
            .Where(a => a.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<Account?> GetByOwnerIdAsync(Guid ownerId, Guid accountId)
    {
        return await context.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.OwnerId == ownerId && a.Id == accountId);
    }

    public async Task<bool> ExistsByOwnerIdAsync(Guid ownerId)
    {
        return await context.Accounts.AnyAsync(a => a.OwnerId == ownerId);
    }

    public async Task<bool> ExistsByIdAsync(Guid accountId)
    {
        return await context.Accounts.AnyAsync(a => a.Id == accountId);
    }
}
