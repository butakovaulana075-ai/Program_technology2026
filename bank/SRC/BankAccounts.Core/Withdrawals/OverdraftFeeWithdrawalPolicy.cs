namespace BankAccounts.Core.Withdrawals;
public sealed class OverdraftFeeWithdrawalPolicy: IWithdrawalPolicy
{
    private readonly decimal _fee;
    public OverdraftFeeWithdrawalPolicy(decimal fee) => _fee = fee;

    WithdrawalsDecision IWithdrawalPolicy.Evaluate(decimal currentBalance, decimal amount, decimal minimumBalance)
     => currentBalance - amount < minimumBalance
        ? WithdrawalsDecision.AllowWithFee(_fee)
        : WithdrawalsDecision.Allow();
  


}


