namespace BankAccount.Domain.Interfaces;

public interface IClientVerificationService
{
    /// <summary>
    /// Проверяет существование клиента по его OwnerId
    /// </summary>
    // ReSharper disable once UnusedParameter.Global Параметр оставлен для дальнейшего использования
    Task<bool> ClientExistsAsync(Guid ownerId);
}