namespace ConsoleApp1;

/// <summary>
/// Статус клиента банка, определяющий условия обслуживания.
/// </summary>
public enum AccountStatus
{
    /// <summary>Обычный клиент: комиссия 50 руб/мес.</summary>
    Regular,

    /// <summary>Премиум: без комиссии, 1% на остаток свыше 10 000.</summary>
    Premium,

    /// <summary>VIP: без комиссии, 3% на остаток свыше 10 000, кэшбэк 5% на списания.</summary>
    Vip
}            