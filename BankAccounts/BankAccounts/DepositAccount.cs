using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccounts
{
    /// <summary>
    /// Депозитный счёт с начислением процентов.
    /// </summary>
    public class DepositAccount : Account
    {
        /// <summary>
        /// Годовая процентная ставка (например, 0.05 = 5%).
        /// </summary>
        public decimal InterestRate { get; set; }

        /// <summary>
        /// Прогнозируемый годовой доход по текущему балансу.
        /// </summary>
        public decimal ProjectedAnnualYield => Balance * InterestRate;

        /// <summary>
        /// Инициализирует депозитный счёт с номером, владельцем, балансом и ставкой.
        /// </summary>
        /// <param name="number">Номер счёта.</param>
        /// <param name="owner">Владелец счёта.</param>
        /// <param name="balance">Начальный баланс.</param>
        /// <param name="interestRate">Годовая процентная ставка.</param>
        public DepositAccount(string number, string owner, decimal balance, decimal interestRate)
            : base(number, owner, balance)
        {
            InterestRate = interestRate;
            Type = AccountType.Deposit;
        }

        /// <summary>
        /// Инициализирует депозитный счёт без номера и с нулевым балансом.
        /// </summary>
        /// <param name="owner">Владелец счёта.</param>
        /// <param name="interestRate">Годовая процентная ставка.</param>
        public DepositAccount(string owner, decimal interestRate)
            : this("N/A", owner, 0, interestRate)
        {
        }

        /// <summary>
        /// Возвращает строковое представление депозитного счёта.
        /// </summary>
        /// <returns>Строка с данными базового счёта и процентной ставкой.</returns>
        public override string ToString()
        {
            return base.ToString() + $", rate: {InterestRate}";
        }
    }
}
