using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// Тип операции, фиксируемой в журнале аудита.
    /// </summary>
    public enum OperationType
    {
        /// <summary>Пополнение счёта (в том числе начальный баланс, кэшбэк).</summary>
        Deposit,

        /// <summary>Списание средств (снятие, комиссия за обслуживание, комиссия за овердрафт).</summary>
        Withdrawal,

        /// <summary>Начисление процентов на остаток.</summary>
        Interest
    }

    /// <summary>
    /// Журнал аудита операций по банковским счетам.
    /// Хранит записи обо всех пополнениях, списаниях и начислениях процентов.
    /// </summary>
    public class AuditLog
    {
        /// <summary>
        /// Одна запись журнала аудита.
        /// </summary>
        /// <param name="AccountNumber">Номер счёта, по которому выполнена операция.</param>
        /// <param name="Type">Тип операции.</param>
        /// <param name="Amount">Сумма операции.</param>
        /// <param name="Date">Дата и время операции.</param>
        /// <param name="TransferId">
        /// Идентификатор перевода, если операция — часть перевода между счетами.
        /// Для обычных операций — <see langword="null"/>.
        /// </param>
        public record Entry(string AccountNumber, OperationType Type, decimal Amount,
                            DateTime Date, Guid? TransferId = null);

        private readonly List<Entry> _entries = new();

        /// <summary>
        /// Добавляет запись в журнал аудита.
        /// </summary>
        /// <param name="accountNumber">Номер счёта, по которому выполнена операция.</param>
        /// <param name="type">Тип операции.</param>
        /// <param name="amount">Сумма операции.</param>
        /// <param name="date">Дата и время операции.</param>
        /// <param name="transferId">Идентификатор перевода, если операция — часть перевода.</param>
        public void Record(string accountNumber, OperationType type, decimal amount,
                           DateTime date, Guid? transferId = null)
        {
            _entries.Add(new Entry(accountNumber, type, amount, date, transferId));
        }

        /// <summary>
        /// Возвращает все записи журнала аудита.
        /// </summary>
        /// <returns>Список записей журнала в порядке добавления.</returns>
        public IReadOnlyList<Entry> GetEntries() => _entries;

        /// <summary>
        /// Возвращает журнал аудита в виде форматированной строки.
        /// </summary>
        /// <returns>
        /// Строка, содержащая по одной строке на каждую операцию:
        /// номер счёта, тип операции, сумму, дату и идентификатор перевода.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Account\t\tType\t\tAmount\tDate\t\tTransferId");
            foreach (var e in _entries)
            {
                sb.AppendLine($"{e.AccountNumber}\t{e.Type}\t{e.Amount}\t" +
                              $"{e.Date:yyyy-MM-dd HH:mm:ss}\t{e.TransferId}");
            }
            return sb.ToString();
        }
    }
}