namespace BankAccount.Domain;

public class MbResult<T>
{
    public T? Value { get; set; }
    public List<string>? MbError { get; set; }

    public static MbResult<T> Success(T value) =>
        new MbResult<T> { Value = value, MbError = null };

    public static MbResult<T> Fail(List<string> errors) =>
        new MbResult<T> { Value = default, MbError = errors };
}