namespace BankAccount.Domain.Exceptions;

public class ConflictException(string error) : Exception("Error occurred.")
{
    public List<string> Errors { get; } = [error];
}
