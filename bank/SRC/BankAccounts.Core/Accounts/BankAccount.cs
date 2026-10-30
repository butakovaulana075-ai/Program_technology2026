using BankAccounts.Core.AccountNumbers;
using BankAccounts.Core.MonthEnd;
using BankAccounts.Core.Time;
using BankAccounts.Core.Transactions;
using BankAccounts.Core.Withdrawals;

namespace BankAccounts.Core.Accounts;

// Тонкий класс-ядро: хранит транзакции, применяет политики и правила
public class BankAccount : IAccountOperations, IAccountStatement
{
    private readonly List<Transaction> _transactions = new();
    private readonly List<IMonthEndRule> _monthEndRules;
    private readonly IClock _clock;
    private readonly IWithdrawalPolicy _withdrawalPolicy;
    public string Number { get; }
    public string Owner { get; }
    public decimal MinimumBalance { get; }
    public IReadOnlyList<Transaction> Transactions => _transactions;
    public decimal Balance => _transactions.Sum(t => t.Amount);
    public BankAccount(
        IClock clock,
        string owner, 
        decimal InitialBalance,
        decimal minimumBalance,
        IAccountNumberGenerator numberGenerator,
        IWithdrawalPolicy withdrawalPolicy,
        IEnumerable<IMonthEndRule>? monthEndRules = null)
    {
        _monthEndRules = monthEndRules?.ToList() ?? null;
        //if (monthEndRules is null)
        //    monthEndRules = null;
        //else _monthEndRules = monthEndRules!.ToList()
        Number = numberGenerator.Next();
        _clock = clock;
        _withdrawalPolicy = withdrawalPolicy;;
        Owner = owner;
        MinimumBalance = minimumBalance;

        if (InitialBalance > 0)
            Deposit(InitialBalance, _clock.Now, "InitialBalance");
    }

    public void Deposit(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposite must be positive");
        }


        var deposite = new Transaction(amount, date, note);
        _transactions.Add(deposite);
    }

    public void Withdraw(decimal amount, DateTime date, string note)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var decision = _withdrawalPolicy.Evaluate(Balance, amount, MinimumBalance);

        if (!decision.IsAllowed)
            throw new InvalidOperationException(decision.RejectionReason);

        _transactions.Add(new Transaction(-amount, date, note));
        if (decision.Fee > 0)
            _transactions.Add(new Transaction(-decision.Fee, date, "Apply overdraft fee"));
        
    }

    public virtual void PerformMonthEndTransactions()
    {
        var asOf = _clock.Now;
        foreach (var rule in _monthEndRules)
            rule.Apply(this, asOf);
    }
}
