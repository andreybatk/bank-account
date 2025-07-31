namespace BankAccount.Domain;

/// <summary>
/// Результат запроса
/// </summary>
public class MbResult<T>
{
    /// <summary>
    /// Значение
    /// </summary>
    public T? Value { get; set; }
    /// <summary>
    /// Ошибки
    /// </summary>
    public List<string>? MbError { get; set; }

    public static MbResult<T> Success(T value) =>
        new MbResult<T> { Value = value, MbError = null };

    public static MbResult<T> Fail(List<string> errors) =>
        new MbResult<T> { Value = default, MbError = errors };
}