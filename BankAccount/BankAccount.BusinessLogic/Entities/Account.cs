using BankAccount.BusinessLogic.Enums;

namespace BankAccount.BusinessLogic.Entities
{
    public class Account
    {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public Guid OwnerId { get; set; }
        /// <summary>
        /// Тип аккаунта
        /// </summary>
        public AccountType Type { get; set; }
        /// <summary>
        /// Валюта (ISO4217)
        /// </summary>
        public string Currency { get; set; } = "RUB";
        /// <summary>
        /// Баланс
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// Процентная ставка (для Deposit/Credit)
        /// </summary>
        public decimal? InterestRate { get; set; }
        /// <summary>
        /// Дата открытия 
        /// </summary>
        public DateTime OpenDate { get; set; }
        /// <summary>
        /// Дата закрытия (опционально)
        /// </summary>
        public DateTime? CloseDate { get; set; }
        /// <summary>
        /// Транзакции аккаунта
        /// </summary>
        public List<AccountTransaction> Transactions { get; set; } = [];
    }
}
