namespace BankAccounts
{
    /// <summary>
    /// Определяет возможность снятия средств со счёта.
    /// </summary>
    public interface IWithdrawable
    {
        /// <summary>
        /// Снимает указанную сумму со счёта.
        /// </summary>
        /// <param name="amount">Сумма снятия.</param>
        void Withdraw(decimal amount);
    }
}
