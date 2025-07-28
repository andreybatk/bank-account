namespace BankAccount.Domain.Interfaces
{
    public interface IClientVerificationService
    {
        /// <summary>
        /// Проверяет существование клиента по его OwnerId
        /// </summary>
        Task<bool> ClientExistsAsync(Guid ownerId);
    }
}
