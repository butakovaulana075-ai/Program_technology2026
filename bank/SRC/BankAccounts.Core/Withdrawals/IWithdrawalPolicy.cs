namespace BankAccounts.Core.Withdrawals;

public interface IWithdrawalPolicy
{
    WithdrawalsDecision Evaluate(decimal currentBalance, decimal amount, decimal minimumBalance);
}

