namespace BankAccount.Domain.Exceptions;

public class BadRequestException : Exception
{
    public List<string> Errors { get; }

    public BadRequestException(List<string> errors)
        : base("One or more errors occurred.")
    {
        Errors = errors;
    }

    public BadRequestException(string error)
        : base("Error occurred.")
    {
        Errors = [error];
    }
}
