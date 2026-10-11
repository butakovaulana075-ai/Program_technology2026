using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Text;

namespace bank;
/// <summary>
/// Представляет кредитный счёт (кредитную линию) с возможностью ухода в минус
/// в пределах установленного кредитного лимита и начислением процентов на отрицательный баланс.
/// Наследуется от <see cref="BankAccount"/>.
/// </summary>
public class LineOfCreditAccount: BankAccount
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="LineOfCreditAccount"/>.
    /// </summary>
    /// <param name="name">Имя владельца счёта.</param>
    /// <param name="initialBalance">Начальный баланс счёта.</param>
    /// <param name="creditLimit">Кредитный лимит, задающий минимально допустимый (отрицательный) баланс.</param>
    public LineOfCreditAccount(string name, decimal initialBalance, decimal creditLimit) 
        : base(name, initialBalance, -creditLimit)
    {

    }
    /// <summary>
    /// Выполняет ежемесячные операции по счёту: если баланс отрицательный,
    /// начисляет проценты в размере 7% от суммы задолженности.
    /// </summary>
    public override void PerformMonthAndTransactions()
    {
        if (Balance < 0)
        {
            decimal interest = -Balance * 0.07m;
            MakeWithdrawal(interest, DateTime.UtcNow, "Charge monthly interest");

        }

    }
    /// <summary>
    /// Проверяет, допустимо ли снятие средств при превышении кредитного лимита.
    /// При превышении лимита возвращает транзакцию с комиссией за овердрафт.
    /// </summary>
    /// <param name="isOverdrawn">Признак превышения кредитного лимита.</param>
    /// <returns>
    /// Транзакцию с комиссией за овердрафт (-20), если <paramref name="isOverdrawn"/>
    /// равен <see langword="true"/>; иначе <see langword="null"/>.
    /// </returns>
    protected override Transaction? CkeckWithdrawalLimit(bool isOverdrawn)
        => isOverdrawn ? new Transaction(-20, DateTime.UtcNow, "apply overdraft") : default;
    
}
