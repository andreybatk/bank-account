using BankAccount.DataAccess;
using BankAccount.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BankAccount.BusinessLogic.Services;

public class AccrueInterestService(IServiceScopeFactory scopeFactory, ILogger<AccrueInterestService> logger)
{
    public async Task ProcessAllDepositsAsync(CancellationToken token = default)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var depositAccountIds = await context.Accounts
            .Where(a => a.Type == EAccountType.Deposit)
            .Select(a => a.Id)
            .ToListAsync(token);

        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 10, CancellationToken = token };

        await Parallel.ForEachAsync(depositAccountIds, parallelOptions, async (accountId, ct) =>
        {
            await AccrueInterestAsync(accountId, ct);
        });
    }

    private async Task AccrueInterestAsync(Guid accountId, CancellationToken token)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await using var tx = await context.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.Serializable, token);

        try
        {
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"CALL accrue_interest({accountId})", token);

            await tx.CommitAsync(token);
        }
        catch (PostgresException pgEx)
        {
            logger.LogError(pgEx, "Ошибка Postgres при начислении процентов на аккаунт {AccountId}", accountId);
            await tx.RollbackAsync(token);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при начислении процентов на аккаунт {AccountId}", accountId);
            await tx.RollbackAsync(token);
            throw;
        }
    }
}

