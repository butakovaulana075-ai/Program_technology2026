using System;

namespace BankAccounts
{
    /// <summary>
    /// Базовый класс банковского счёта.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Приватное поле — номер счёта
        /// </summary>
        private readonly string? _number;

        /// <summary>
        /// Владелец счёта.
        /// </summary>
        public string? Owner { get; set; }

        /// <summary>
        /// Текущий баланс счёта.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Тип счёта. По умолчанию — <see cref="AccountType.Debit"/>.
        /// </summary>
        public AccountType Type { get; set; } = AccountType.Debit;

        /// <summary>
        /// Признак VIP-клиента: баланс больше 1 000 000.
        /// </summary>
        public bool IsVip => Balance > 1_000_000;

        /// <summary>
        /// Номер счёта (только для чтения).
        /// </summary>
        public string? Number => _number;

        /// <summary>
        /// Инициализирует счёт с указанным номером, владельцем и балансом.
        /// </summary>
        /// <param name="number">Номер счёта.</param>
        /// <param name="owner">Владелец счёта.</param>
        /// <param name="balance">Начальный баланс.</param>
        public Account(string number, string owner, decimal balance)
        {
            _number = number;
            Owner = owner;
            Balance = balance;
        }

        /// <summary>
        /// Инициализирует счёт без номера и с нулевым балансом.
        /// </summary>
        /// <param name="owner">Владелец счёта.</param>
        public Account(string owner)
            : this("N/A", owner, 0) { }

        // Ключевое слово virtual позволяет в дочернем классе предоставить
        // другую реализацию метода DisplayInfo
        /// <summary>
        /// Выводит информацию о счёте.
        /// </summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine(ToString());
            Console.WriteLine($"   vip: {IsVip}");
            Console.WriteLine($"{Number} ({Type}) displayed.");
        }

        // переопределяем метод базового класса - класса object
        // toString возвращает строку с информацией об объекте
        /// <summary>
        /// Возвращает строковое представление счёта.
        /// </summary>
        /// <returns>Строка с номером, владельцем, балансом и типом счёта.</returns>
        public override string ToString()
        {
            return $"[{GetType().Name}] {_number}, {Owner}, {Balance}, type: {Type}";
        }
    }
}
