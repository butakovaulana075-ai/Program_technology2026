using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    // BankAccount - потомок класса object

    /// <summary>
    /// Представляет банковский счёт с возможностью пополнения, снятия средств,
    /// ведения истории транзакций и аудита операций.
    /// </summary>
    public class BankAccount
    {
        private readonly decimal _minimalBalance;
        private List<Transaction> _allTransactions = new List<Transaction>();

        /// <summary>
        /// Статический журнал аудита, общий для всех счетов.
        /// </summary>
        private static readonly AuditLog s_auditLog = new();

        /// <summary>
        /// Владелец счёта.
        /// </summary>
        public string Owner { get; private set; }

        /// <summary>
        /// Номер счёта.
        /// </summary>
        public string Number { get; }

        /// <summary>
        /// Статус клиента банка, определяющий условия обслуживания.
        /// </summary>
        public AccountStatus Status { get; private set; }

        /// <summary>
        /// Текущий баланс счёта, вычисляемый как сумма всех транзакций.
        /// </summary>
        public decimal Balance
        {
            get
            {
                decimal balance = 0;
                foreach (var transaction in _allTransactions)
                {
                    balance += transaction.Amount;
                }
                return balance;
            }
        }

        /// <summary>
        /// Публичный доступ к общему журналу аудита.
        /// </summary>
        public static AuditLog Audit => s_auditLog;

        private static int s_accountNumberSeed = 1000000000;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BankAccount"/>
        /// с указанным владельцем и начальным балансом. Минимальный баланс равен нулю,
        /// статус — <see cref="AccountStatus.Regular"/>.
        /// </summary>
        /// <param name="name">Имя владельца счёта.</param>
        /// <param name="initialBalance">Начальный баланс счёта.</param>
        public BankAccount(string name, decimal initialBalance)
            : this(name, initialBalance, 0, AccountStatus.Regular)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BankAccount"/>
        /// с указанным владельцем, начальным балансом и минимально допустимым балансом.
        /// Статус — <see cref="AccountStatus.Regular"/>.
        /// </summary>
        /// <param name="name">Имя владельца счёта.</param>
        /// <param name="initialBalance">Начальный баланс счёта.</param>
        /// <param name="minimalBalance">Минимально допустимый баланс счёта.</param>
        public BankAccount(string name, decimal initialBalance, decimal minimalBalance)
            : this(name, initialBalance, minimalBalance, AccountStatus.Regular)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BankAccount"/>
        /// с указанным владельцем, начальным балансом, минимально допустимым балансом и статусом.
        /// </summary>
        /// <param name="name">Имя владельца счёта.</param>
        /// <param name="initialBalance">Начальный баланс счёта.</param>
        /// <param name="minimalBalance">Минимально допустимый баланс счёта.</param>
        /// <param name="status">Статус клиента банка.</param>
        public BankAccount(string name, decimal initialBalance, decimal minimalBalance, AccountStatus status)
        {
            Owner = name;
            Number = s_accountNumberSeed.ToString();
            s_accountNumberSeed++;
            _minimalBalance = minimalBalance;
            Status = status;

            if (initialBalance > 0)
            {
                MakeDeposite(initialBalance, DateTime.UtcNow, "initial balance");
            }
        }

        /// <summary>
        /// Пополняет счёт на указанную сумму.
        /// </summary>
        /// <param name="amout">Сумма пополнения. Должна быть положительной.</param>
        /// <param name="date">Дата операции.</param>
        /// <param name="note">Комментарий к операции.</param>
        /// <param name="transferId">
        /// Идентификатор перевода, если операция — часть перевода между счетами.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Бросается, если <paramref name="amout"/> меньше или равен нулю.
        /// </exception>
        public void MakeDeposite(decimal amout, DateTime date, string note, Guid? transferId = null)
        {
            if (amout <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amout), "Amount of deposite must be positive");
            }

            AddTransaction(amout, date, note, OperationType.Deposit, transferId);
        }

        /// <summary>
        /// Снимает указанную сумму со счёта.
        /// Для VIP-клиентов начисляет кэшбэк 5% от суммы снятия.
        /// </summary>
        /// <param name="amout">Сумма снятия. Должна быть положительной.</param>
        /// <param name="date">Дата операции.</param>
        /// <param name="note">Комментарий к операции.</param>
        /// <param name="transferId">
        /// Идентификатор перевода, если операция — часть перевода между счетами.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Бросается, если <paramref name="amout"/> меньше или равен нулю.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Бросается, если после снятия баланс становится меньше минимально допустимого
        /// (в базовой реализации <see cref="CkeckWithdrawalLimit"/>).
        /// </exception>
        public void MakeWithdrawal(decimal amout, DateTime date, string note, Guid? transferId = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amout);

            Transaction? overdraftTransaction = CkeckWithdrawalLimit(Balance - amout < _minimalBalance);

            AddTransaction(-amout, date, note, OperationType.Withdrawal, transferId);

            if (overdraftTransaction is not null)
            {
                _allTransactions.Add(overdraftTransaction);
                s_auditLog.Record(Number, OperationType.Withdrawal,
                                  overdraftTransaction.Amount, overdraftTransaction.Date);
            }

            if (Status == AccountStatus.Vip)
            {
                decimal cashback = amout * 0.05m;
                AddTransaction(cashback, date, "VIP cashback", OperationType.Deposit);
            }
        }

        // protected - модификатор доступа, который означает,
        // что этот метод можно вызвать только из текущего и дочернего класса
        // Клиент (внешний код) данный метод вызвать не может

        /// <summary>
        /// Проверяет, допустимо ли снятие средств при превышении лимита.
        /// Переопределяется в дочерних классах для изменения правил снятия.
        /// </summary>
        /// <param name="isOverdrawn">Признак превышения минимально допустимого баланса.</param>
        /// <returns>Дополнительную транзакцию (например, комиссию) или <see langword="null"/>.</returns>
        /// <exception cref="InvalidOperationException">Если снятие недопустимо.</exception>
        protected virtual Transaction? CkeckWithdrawalLimit(bool isOverdrawn)
        {
            if (isOverdrawn)
            {
                throw new InvalidOperationException("Not sufficient rubls for this withdrawal");
            }
            else
            {
                // default - содержит значение по умолчанию, так как тип возвращаемого значения - ссылочный, то
                // default = null
                return default;
            }
        }

        /// <summary>
        /// Возвращает историю операций по счёту в виде форматированной строки.
        /// </summary>
        /// <returns>
        /// Строка, содержащая дату, сумму, баланс после операции и комментарий для каждой транзакции.
        /// </returns>
        public string GetAccountHistory()
        {
            var report = new StringBuilder();
            decimal balance = 0;
            report.AppendLine("Data\t\tAmount\tBalance\tNote");
            foreach (var item in _allTransactions)
            {
                balance += item.Amount;
                report.AppendLine($"" +
                    $"{item.Date.ToShortDateString()}\t" +
                    $"{item.Amount}\t{balance}\t{item.Note}");
            }
            return report.ToString();
        }

        // Ключевое слово virtual позволяет в дочернем классе предоставить другую реализацию
        // Метода PerformMonthAndTransactions

        /// <summary>
        /// Выполняет ежемесячные операции по счёту: начисление процентов на остаток
        /// свыше 10 000 и списание комиссии за обслуживание в соответствии со статусом клиента.
        /// Может быть переопределён в дочерних классах.
        /// </summary>
        public virtual void PerformMonthAndTransactions()
        {
            const decimal threshold = 10_000m;

            // 1. Начисление процентов на остаток свыше 10 000
            if (Balance > threshold)
            {
                decimal over = Balance - threshold;
                decimal interest = Status switch
                {
                    AccountStatus.Premium => over * 0.01m,
                    AccountStatus.Vip => over * 0.03m,
                    _ => 0m
                };

                if (interest > 0)
                {
                    AddTransaction(interest, DateTime.UtcNow, "Monthly interest", OperationType.Interest);
                }
            }

            // 2. Списание комиссии за обслуживание
            decimal fee = Status switch
            {
                AccountStatus.Regular => 50m,
                _ => 0m
            };

            if (fee > 0)
            {
                MakeWithdrawal(fee, DateTime.UtcNow, "Monthly fee");
            }
        }

        // переопределяем метод базового класса - класса object
        // toString возвращает строку с информацией об объекте

        /// <summary>
        /// Возвращает строковое представление счёта.
        /// </summary>
        /// <returns>
        /// Строка, содержащая имя владельца, номер счёта, тип счёта и статус клиента.
        /// </returns>
        public override string ToString()
        {
            return $"Owner: {Owner}\taccount number: {Number} (тип счета {GetType()}, статус {Status})";
        }

        // ============ Приватные вспомогательные методы ============

        /// <summary>
        /// Добавляет транзакцию в историю счёта и записывает её в журнал аудита.
        /// Единственная точка создания транзакций — гарантирует, что ни одна операция
        /// не будет пропущена в аудите.
        /// </summary>
        /// <param name="amount">Сумма операции.</param>
        /// <param name="date">Дата и время операции.</param>
        /// <param name="note">Комментарий к операции.</param>
        /// <param name="type">Тип операции для журнала аудита.</param>
        /// <param name="transferId">Идентификатор перевода, если операция — часть перевода.</param>
        private void AddTransaction(decimal amount, DateTime date, string note,
                                    OperationType type, Guid? transferId = null)
        {
            _allTransactions.Add(new Transaction(amount, date, note, transferId));
            s_auditLog.Record(Number, type, amount, date, transferId);
        }
    }
}