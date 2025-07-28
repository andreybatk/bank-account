using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
