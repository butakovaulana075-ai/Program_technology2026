using ConsoleApp1;
using System;

namespace bank
{
    /// <summary>
    /// Представляет банк, управляющий счетами и переводами между ними.
    /// </summary>
    public class Bank
    {
        /// <summary>
        /// Переводит указанную сумму со счёта <paramref name="from"/> на счёт <paramref name="to"/>.
        /// Обе операции помечаются общим <c>TransferId</c>, что позволяет связать их в истории и аудите.
        /// Если зачисление не удалось, списание откатывается компенсирующей операцией.
        /// </summary>
        /// <param name="from">Счёт-источник.</param>
        /// <param name="to">Счёт-получатель.</param>
        /// <param name="amount">Сумма перевода. Должна быть положительной.</param>
        /// <param name="date">Дата операции.</param>
        /// <param name="note">Комментарий к переводу.</param>
        /// <exception cref="ArgumentNullException">
        /// Бросается, если <paramref name="from"/> или <paramref name="to"/> равны <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Бросается, если <paramref name="from"/> и <paramref name="to"/> — один и тот же счёт.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Бросается, если <paramref name="amount"/> меньше или равен нулю.
        /// </exception>
        public void Transfer(BankAccount from, BankAccount to,
                             decimal amount, DateTime date, string note)
        {
            if (from is null) throw new ArgumentNullException(nameof(from));
            if (to is null) throw new ArgumentNullException(nameof(to));
            if (ReferenceEquals(from, to))
                throw new InvalidOperationException("Нельзя перевести средства на тот же счёт");

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

            Guid transferId = Guid.NewGuid();

            string fromNote = $"{note} (transfer to {to.Number})";
            string toNote = $"{note} (transfer from {from.Number})";

            from.MakeWithdrawal(amount, date, fromNote, transferId);

            try
            {
                to.MakeDeposite(amount, date, toNote, transferId);
            }
            catch
            {
                from.MakeDeposite(amount, date, "rollback: transfer failed", transferId);
                throw;
            }
        }
    }
}