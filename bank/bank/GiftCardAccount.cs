using ConsoleApp1;

namespace bank;

public class GiftCardAccount: BankAccount
{
    private readonly decimal _monthlyDeposit = 0m;

    // monthlyDeposit - параметр по умолчанию, по умолчанию принимает 0,
    // при создании new GiftCardAccount("Ulyana", 1000); - monthlyDeposit = 0
    // new GiftCardAccount("Ulyana", 1000, 5000) => monthlyDeposit = 5000
    public GiftCardAccount(string name, decimal initialBalance, decimal monthlyDeposit = 0)
        :base(name, initialBalance)
        => _monthlyDeposit = monthlyDeposit;

    public override void PerformMonthAndTransactions()
    {
        if (_monthlyDeposit != 0)
        {
            MakeDeposite(_monthlyDeposit, DateTime.UtcNow, "");
        }
    }
    public override string ToString()
    {
        return base.ToString()+$"monthly deposit: {_monthlyDeposit}";
    }
}
