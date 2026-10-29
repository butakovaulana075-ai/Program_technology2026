namespace BankAccounts.Core.Withdrawals;

// Единый контракт для всех политик снятия: возвращаем решение, а не выбрасываем исключения
// Это утраняет LSP между базовым и переопределенным методом
internal class WithdrawalsDecision(bool IsAllowed, decimal Fee, string? RejectionReason)
{
    public static WithdrawalsDecision Allow() => new(true, 0m, null);

    public static WithdrawalsDecision AllowWithFee(decimal fee) => new(true, fee, null);

    public static WithdrawalsDecision Reject(string reason) => new(false, 0m, reason);
}

