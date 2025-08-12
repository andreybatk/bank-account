using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.BusinessLogic.Accounts.Handlers;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Enums;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;
using Moq;

namespace BankAccount.Tests;
public class AccountCommandHandlersTests
{
    [Fact]
    public async Task CreateAccountCommandHandler_ShouldReturnNewGuid_WhenValidRequest()
    {
        var accountRepoMock = new Mock<IAccountRepository>();
        var clientVerificationMock = new Mock<IClientVerificationService>();
        var currencyServiceMock = new Mock<ICurrencyService>();

        clientVerificationMock.Setup(s => s.ClientExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        currencyServiceMock.Setup(s => s.IsCurrencySupportedAsync(It.IsAny<string>())).ReturnsAsync(true);
        accountRepoMock.Setup(r => r.CreateAsync(It.IsAny<Account>())).ReturnsAsync(Guid.NewGuid());

        var handler = new CreateAccountCommandHandler(accountRepoMock.Object, clientVerificationMock.Object, currencyServiceMock.Object);

        var command = new CreateAccountCommand(Guid.NewGuid(), EAccountType.Checking, "RUB", 100, null, DateTime.UtcNow,
            null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result);
        clientVerificationMock.Verify(v => v.ClientExistsAsync(command.OwnerId), Times.Once);
        currencyServiceMock.Verify(v => v.IsCurrencySupportedAsync(command.Currency), Times.Once);
        accountRepoMock.Verify(r => r.CreateAsync(It.IsAny<Account>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAccountCommandHandler_ShouldThrow_WhenAccountNotFound()
    {
        var accountRepoMock = new Mock<IAccountRepository>();
        accountRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync((Guid?)null);

        var handler = new DeleteAccountCommandHandler(accountRepoMock.Object);
        var command = new DeleteAccountCommand(Guid.NewGuid());

        await Assert.ThrowsAsync<EntityNotFoundException>(() => handler.Handle(command, CancellationToken.None));
        accountRepoMock.Verify(r => r.DeleteAsync(command.AccountId), Times.Once);
    }

    [Fact]
    public async Task UpdateAccountCommandHandler_ShouldThrowValidationException_WhenCurrencyNotSupported()
    {
        var accountRepoMock = new Mock<IAccountRepository>();
        var clientVerificationMock = new Mock<IClientVerificationService>();
        var currencyServiceMock = new Mock<ICurrencyService>();

        clientVerificationMock.Setup(v => v.ClientExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        currencyServiceMock.Setup(v => v.IsCurrencySupportedAsync(It.IsAny<string>())).ReturnsAsync(false);

        var handler = new UpdateAccountCommandHandler(accountRepoMock.Object, clientVerificationMock.Object, currencyServiceMock.Object);

        var command = new UpdateAccountCommand(Guid.NewGuid(), Guid.NewGuid(), EAccountType.Deposit, "XYZ5", 100, 5,
            DateTime.UtcNow, null);

        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        clientVerificationMock.Verify(v => v.ClientExistsAsync(command.OwnerId), Times.Once);
        currencyServiceMock.Verify(v => v.IsCurrencySupportedAsync(command.Currency), Times.Once);
        accountRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Account>()), Times.Never);
    }
}
