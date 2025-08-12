using BankAccount.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.DataAccess;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    // dotnet ef migrations add InitialCreate -p BankAccount.DataAccess\BankAccount.DataAccess.csproj -s BankAccount.API\BankAccount.API.csproj
    // dotnet ef migrations add AddAccrueInterestProcedure -p BankAccount.DataAccess\BankAccount.DataAccess.csproj -s BankAccount.API\BankAccount.API.csproj

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountTransaction> Transactions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property<uint>("xmin")
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(3);

            entity.HasMany(e => e.Transactions)
                .WithOne()
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.OwnerId)
                .HasDatabaseName("idx_accounts_owner_hash")
                .HasMethod("hash");
        });

        modelBuilder.Entity<AccountTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(3);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.HasIndex(e => new { e.AccountId, e.CreatedAt })
                .HasDatabaseName("idx_transactions_account_date");
        });
    }
}