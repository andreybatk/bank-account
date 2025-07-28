using BankAccount.Domain.Enums;

namespace BankAccount.Domain.Entities
{
    public class AccountTransaction
    {
        /// <summary>
        /// Идентификатор транзакции
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        public Guid AccountId { get; set; }
        /// <summary>
        /// Идентификатор контрагента (опционально)
        /// </summary>
        public Guid? CounterpartyAccountId { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Валюта (ISO 4217)
        /// </summary>
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// Тип транзакции
        /// </summary>
        public TransactionType Type { get; set; }
        /// <summary>
        /// Описание транзакции
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
