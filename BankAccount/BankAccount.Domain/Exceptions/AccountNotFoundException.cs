namespace BankAccount.Domain.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(Guid accountId)
            : base($"Account with Id {accountId} was not found.") { }
    }
}