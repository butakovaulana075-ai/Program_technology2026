namespace BankAccounts.Core.Withdrawals;

// Политика по умолчанию: если баланс ниже минимума - отклоняем
internal class StrictWithdrawalPolicy : IWithdrawalPolicy
{
    public WithdrawalsDecision Evaluate(decimal currentBalance, decimal amount, decimal minimumBalance)
       => currentBalance - amount < minimumBalance
       ? WithdrawalsDecision.Reject("Not sufficient rubls for this withdrawal")
       : WithdrawalsDecision.Allow();

}