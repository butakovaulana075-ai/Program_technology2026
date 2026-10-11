using ConsoleApp1;

namespace bank;

/// <summary>
/// Представляет подарочный карточный счёт с возможностью ежемесячного автоматического пополнения.
/// Наследуется от <see cref="BankAccount"/>.
/// </summary>
public class GiftCardAccount: BankAccount
{
    private readonly decimal _monthlyDeposit = 0m;

    // monthlyDeposit - параметр по умолчанию, по умолчанию принимает 0,
    // при создании new GiftCardAccount("Ulyana", 1000); - monthlyDeposit = 0
    // new GiftCardAccount("Ulyana", 1000, 5000) => monthlyDeposit = 5000

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GiftCardAccount"/>.
    /// </summary>
    /// <param name="name">Имя владельца счёта.</param>
    /// <param name="initialBalance">Начальный баланс счёта.</param>
    /// <param name="monthlyDeposit">Сумма ежемесячного автоматического пополнения. По умолчанию равна 0.</param>
    public GiftCardAccount(string name, decimal initialBalance, decimal monthlyDeposit = 0)
        :base(name, initialBalance)
        => _monthlyDeposit = monthlyDeposit;

    /// <summary>
    /// Выполняет ежемесячные операции по счёту: при ненулевой сумме
    /// <c>_monthlyDeposit</c> пополняет счёт на эту сумму.
    /// </summary>
    public override void PerformMonthAndTransactions()
    {
        if (_monthlyDeposit != 0)
        {
            MakeDeposite(_monthlyDeposit, DateTime.UtcNow, "");
        }
    }

    /// <summary>
    /// Возвращает строковое представление счёта, включая информацию
    /// о сумме ежемесячного пополнения.
    /// </summary>
    /// <returns>
    /// Строка с данными базового класса <see cref="BankAccount"/> и суммой ежемесячного пополнения.
    /// </returns>
    public override string ToString()
    {
        return base.ToString()+$"monthly deposit: {_monthlyDeposit}";
    }
}
