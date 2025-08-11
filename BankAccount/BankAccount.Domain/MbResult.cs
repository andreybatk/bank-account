namespace BankAccount.Domain;

/// <summary>
/// Результат запроса
/// </summary>
public class MbResult<T>
{
    /// <summary>
    /// Значение
    /// </summary>
    public T? Value { get; init; }
    /// <summary>
    /// Ошибки
    /// </summary>
    public List<string>? MbError { get; init; }

    /// <summary>
    /// Успешный результат
    /// </summary>
    public static MbResult<T> Success(T value) =>
        new() { Value = value, MbError = null };

    /// <summary>
    /// Неуспешный результат
    /// </summary>
    public static MbResult<T> Fail(List<string> errors) =>
        new() { Value = default, MbError = errors };
}