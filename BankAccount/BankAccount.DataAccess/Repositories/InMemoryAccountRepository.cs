using BankAccount.Domain.Entities;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.DataAccess.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly List<Account> _accounts = [];

    public Task<Guid> CreateAsync(Account account)
    {
        _accounts.Add(account);
        return Task.FromResult(account.Id);
    }

    public Task<Guid> UpdateAsync(Account account)
    {
        var existing = _accounts.FirstOrDefault(a => a.Id == account.Id)
                       ?? throw new AccountNotFoundException(account.Id);

        existing.Type = account.Type;
        existing.Currency = account.Currency;
        existing.Balance = account.Balance;
        existing.InterestRate = account.InterestRate;
        existing.CloseDate = account.CloseDate;

        return Task.FromResult(existing.Id);
    }

    public Task<Guid> DeleteAsync(Guid accountId)
    {
        var account = _accounts.FirstOrDefault(a => a.Id == accountId)
                      ?? throw new AccountNotFoundException(accountId);

        _accounts.Remove(account);
        return Task.FromResult(accountId);
    }

    public Task<Account> GetByIdAsync(Guid accountId)
    {
        var account = _accounts.FirstOrDefault(a => a.Id == accountId)
                      ?? throw new AccountNotFoundException(accountId);

        return Task.FromResult(account);
    }

    public Task<List<Account>> GetAllByOwnerIdAsync(Guid ownerId)
    {
        return Task.FromResult(_accounts.Where(a => a.OwnerId == ownerId).ToList());
    }

    public Task<Account> GetByOwnerIdAsync(Guid ownerId, Guid accountId)
    {
        var account = _accounts.FirstOrDefault(a => a.OwnerId == ownerId && a.Id == accountId)
                      ?? throw new AccountNotFoundException(accountId);

        return Task.FromResult(account);
    }

    public Task<bool> ExistsAsync(Guid ownerId)
    {
        return Task.FromResult(_accounts.Any(a => a.OwnerId == ownerId));
    }
}