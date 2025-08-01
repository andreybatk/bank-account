namespace BankAccount.Domain.Exceptions;

public class BadRequestException : Exception
{
    public List<string> Errors { get; }

    // ReSharper disable once UnusedMember.Global Оставлен для дальнейшего использования
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
