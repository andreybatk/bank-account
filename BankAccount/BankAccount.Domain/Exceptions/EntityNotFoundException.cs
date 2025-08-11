namespace BankAccount.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public List<string> Errors { get; }

    public EntityNotFoundException(List<string> errors)
        : base("One or more not found errors occurred.")
    {
        Errors = errors;
    }
    public EntityNotFoundException(string error)
        : base("Not found error occurred.")
    {
        Errors = [error];
    }
}