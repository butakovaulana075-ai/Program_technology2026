using ConsoleApp1;
using System;

namespace bank;

/// <summary>
/// Представляет счёт с начислением процентов на остаток.
/// Наследуется от <see cref="BankAccount"/>.
/// </summary>
public class InterestEarningAccount : BankAccount
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="InterestEarningAccount"/>.
    /// </summary>
    /// <param name="name">Имя владельца счёта.</param>
    /// <param name="initialBalance">Начальный баланс счёта.</param>
    public InterestEarningAccount(string name, decimal initialBalance)
        : base(name, initialBalance)
    {
    }

    /// <summary>
    /// Выполняет ежемесячные операции по счёту: если баланс превышает 500,
    /// начисляет проценты в размере 2% от текущего баланса.
    /// </summary>
    public override void PerformMonthAndTransactions()
    {
        if (Balance > 500m)
        {
            decimal interest = Balance * 0.02m;
            MakeDeposite(interest, DateTime.UtcNow, "Apply month interest");
        }
    }
}