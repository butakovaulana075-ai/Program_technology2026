using System;

namespace ConsoleApp1;

/// <summary>
/// Представляет банковскую транзакцию: сумму, дату и комментарий.
/// </summary>
/// <param name="Amount">Сумма транзакции. Положительная — пополнение, отрицательная — снятие.</param>
/// <param name="Date">Дата и время совершения транзакции.</param>
/// <param name="Note">Комментарий к транзакции.</param>
/// <param name="TransferId">
/// Идентификатор перевода, если транзакция является частью перевода между счетами.
/// Для обычных операций — <see langword="null"/>.
/// </param>
public record Transaction(decimal Amount, DateTime Date, string Note, Guid? TransferId = null);