using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccounts
{
    /// <summary>
    /// Кредитный счёт с допустимым отрицательным балансом.
    /// </summary>
    public class CreditAccount : Account
    {
        /// <summary>
        /// Кредитный лимит (максимально допустимая сумма задолженности).
        /// </summary>
        public decimal CreditLimit { get; set; }

        /// <summary>
        /// Доступный остаток кредита.
        /// </summary>
        public decimal AvailableCredit => CreditLimit + Balance;

        /// <summary>
        /// Инициализирует кредитный счёт с номером, владельцем, балансом и кредитным лимитом.
        /// </summary>
        /// <param name="number">Номер счёта.</param>
        /// <param name="owner">Владелец счёта.</param>
        /// <param name="balance">Начальный баланс (может быть отрицательным).</param>
        /// <param name="creditLimit">Кредитный лимит.</param>
        public CreditAccount(string number, string owner, decimal balance, decimal creditLimit)
            : base(number, owner, balance)
        {
            CreditLimit = creditLimit;
            Type = AccountType.Credit;
        }

        /// <summary>
        /// Инициализирует кредитный счёт без номера и с нулевым балансом.
        /// </summary>
        /// <param name="owner">Владелец счёта.</param>
        /// <param name="creditLimit">Кредитный лимит.</param>
        public CreditAccount(string owner, decimal creditLimit)
            : this("N/A", owner, 0, creditLimit)
        {
        }

        /// <summary>
        /// Возвращает строковое представление кредитного счёта.
        /// </summary>
        /// <returns>Строка с данными базового счёта и кредитным лимитом.</returns>
        public override string ToString()
        {
            return base.ToString() + $", limit: {CreditLimit}";
        }
    }
}
