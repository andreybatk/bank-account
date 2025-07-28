using BankAccount.Domain.Entities;
using BankAccount.Domain.Enums;

namespace BankAccount.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Зарегистрировать транзакцию по счёту
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<AccountTransaction> RegisterTransactionAsync(AccountTransaction transaction);
    }
}
