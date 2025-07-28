using BankAccount.Domain.Entities;

namespace BankAccount.Domain.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Создать счёт
        /// </summary>
        /// <param name="account">Аккаунт</param>
        /// <returns></returns>
        Task<Guid> CreateAsync(Account account);
        /// <summary>
        /// Изменить счёт
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<Guid> UpdateAsync(Account account);
        /// <summary>
        /// Удалить счёт
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid accountId);
        /// <summary>
        /// Получить список счетов
        /// </summary>
        /// <returns></returns>
        Task<List<Account>> GetAllAsync();
        /// <summary>
        /// Получить счет
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<Account?> GetByIdAsync(Guid accountId);
        /// <summary>
        /// Выдать выписку клиенту
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<List<Account>> GetByOwnerIdAsync(Guid ownerId);
        /// <summary>
        /// Проверить наличие счёта у клиента
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid ownerId);
    }
}
