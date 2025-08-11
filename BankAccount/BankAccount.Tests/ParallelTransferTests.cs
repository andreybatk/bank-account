using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain;
using BankAccount.Domain.Enums;
using System.Net.Http.Json;

namespace BankAccount.Tests;

public class ParallelTransferTests(TestApplicationFactory factory)
    : IClassFixture<TestApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();

    private Guid _accountFromId;
    private Guid _accountToId;

    public async Task InitializeAsync()
    {
        await InitializeAccountsAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ParallelTransfers_ShouldPreserveTotalBalance()
    {
        // arrange
        var accountFromId = await CreateTestAccountAsync(1000m);
        var accountToId = await CreateTestAccountAsync(1000m);

        var initialTotal = await GetBalanceAsync(accountFromId) + await GetBalanceAsync(accountToId);

        // act
        var tasks = Enumerable.Range(0, 50).Select(_ =>
            _client.PostAsJsonAsync("/api/transactions/transfers", new
            {
                FromAccountId = accountFromId,
                ToAccountId = accountToId,
                Amount = 10m
            })
        );

        await Task.WhenAll(tasks);

        // assert
        var finalTotal = await GetBalanceAsync(accountFromId) + await GetBalanceAsync(accountToId);
        Assert.Equal(initialTotal, finalTotal);
    }

    private async Task InitializeAccountsAsync()
    {
        _accountFromId = await CreateTestAccountAsync(initialBalance: 1000m);
        _accountToId = await CreateTestAccountAsync(initialBalance: 1000m);
    }

    private async Task<Guid> CreateTestAccountAsync(decimal initialBalance)
    {
        var command = new CreateAccountCommand(
            OwnerId: Guid.NewGuid(),
            Type: EAccountType.Checking,
            Currency: "RUB",
            InitialBalance: initialBalance,
            InterestRate: null,
            OpenDate: DateTime.UtcNow,
            CloseDate: null
        );

        var response = await _client.PostAsJsonAsync("/api/accounts", command);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<MbResult<Guid>>();
        if (result is null)
            throw new InvalidOperationException("Ошибка при создании клиента в CreateTestAccountAsync.");

        return result.Value;
    }

    private async Task<decimal> GetBalanceAsync(Guid accountId)
    {
        var response = await _client.GetAsync($"/api/accounts/{accountId}/balance");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<MbResult<decimal>>();
        if (result is null)
            throw new InvalidOperationException("Ошибка при получение баланса в GetBalanceAsync.");

        return result.Value;
    }
}
