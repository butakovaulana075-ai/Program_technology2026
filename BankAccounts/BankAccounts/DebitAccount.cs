namespace BankAccounts
{
    /// <summary>
    /// Дебетовый счёт с возможностью снятия в пределах дневного лимита.
    /// </summary>
    public class DebitAccount : Account, IWithdrawable
    {
        /// <summary>
        /// Дневной лимит снятия.
        /// </summary>
        public decimal DailyLimit { get; set; }

        /// <summary>
        /// Признак, что лимит ещё не исчерпан.
        /// </summary>
        public bool CanWithdrawMore => DailyLimit > 0;

        /// <summary>
        /// Инициализирует дебетовый счёт с номером, владельцем, балансом и дневным лимитом.
        /// </summary>
        /// <param name="number">Номер счёта.</param>
        /// <param name="owner">Владелец счёта.</param>
        /// <param name="balance">Начальный баланс.</param>
        /// <param name="dailyLimit">Дневной лимит снятия.</param>
        public DebitAccount(string number, string owner, decimal balance, decimal dailyLimit)
            : base(number, owner, balance)
        {
            DailyLimit = dailyLimit;
            Type = AccountType.Debit;
        }

        /// <summary>
        /// Инициализирует дебетовый счёт без номера и с нулевым балансом.
        /// </summary>
        /// <param name="owner">Владелец счёта.</param>
        /// <param name="dailyLimit">Дневной лимит снятия.</param>
        public DebitAccount(string owner, decimal dailyLimit)
            : this("N/A", owner, 0, dailyLimit)
        {
        }

        /// <summary>
        /// Снимает указанную сумму со счёта.
        /// </summary>
        /// <param name="amount">Сумма снятия.</param>
        public void Withdraw(decimal amount)
        {
            if (amount <= 0 || amount > DailyLimit || amount > Balance)
            {
                Console.WriteLine($"{Number} cannot withdraw.");
                return;
            }

            Balance -= amount;
            DailyLimit -= amount;
            Console.WriteLine($"{Number} withdrawal of {amount} processed.");
        }

        /// <summary>
        /// Возвращает строковое представление дебетового счёта.
        /// </summary>
        /// <returns>Строка с данными базового счёта и дневным лимитом.</returns>
        public override string ToString()
        {
            return base.ToString() + $", daily limit: {DailyLimit}";
        }
    }
}
