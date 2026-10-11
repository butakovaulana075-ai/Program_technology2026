using bank;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BankAccount account1 = new BankAccount("Danil", 10000000);
            BankAccount account2 = new BankAccount("Ulyana", 1000);

            Console.WriteLine($"{account1.Owner} {account1.Balance} {account1.Number}");
            Console.WriteLine($"{account2.Owner} {account2.Balance} {account2.Number}");

            account1.MakeDeposite(12000, DateTime.UtcNow, ";)");
            Console.WriteLine($"Balance: {account1.Balance}");

            account1.MakeWithdrawal(123, DateTime.UtcNow, ";)");
            Console.WriteLine($"Balance: {account1.Balance}");
            Console.WriteLine(account1.GetAccountHistory());

            try
            {
                account2.MakeWithdrawal(10000, DateTime.UtcNow, "asdas");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            InterestEarningAccount interest = new InterestEarningAccount("Ulyana", 1000);
            interest.PerformMonthAndTransactions();

            Console.WriteLine(interest.GetAccountHistory());

            LineOfCreditAccount lineOfCredit = new LineOfCreditAccount("Ulyana", 10, 1000m);
            lineOfCredit.MakeWithdrawal(500m, DateTime.UtcNow, "credit");

            GiftCardAccount giftcart = new GiftCardAccount("Ulyana", 1000m, 5000m);

            List<BankAccount> accounts = new List<BankAccount>();
            accounts.Add(account1);
            accounts.Add(interest);
            accounts.Add(lineOfCredit);
            accounts.Add(giftcart);

            foreach (var account in accounts)
            {
                Console.WriteLine(account);
                account.PerformMonthAndTransactions();
                Console.WriteLine(account.GetAccountHistory());
            }

            lineOfCredit.MakeWithdrawal(600m, DateTime.UtcNow, "credit");
            Console.WriteLine(lineOfCredit.GetAccountHistory());

            Console.WriteLine("=== REGULAR ===");
            var regular = new BankAccount("Danil", 25_000m);
            Console.WriteLine(regular);
            Console.WriteLine($"Баланс до: {regular.Balance}");

            regular.PerformMonthAndTransactions();

            Console.WriteLine($"Баланс после: {regular.Balance}");
            Console.WriteLine(regular.GetAccountHistory());
            Console.WriteLine();

            Console.WriteLine("=== ПЕРЕВОД ===");
            var bank = new Bank();

            var accA = new BankAccount("Danil", 10_000m);
            var accB = new BankAccount("Ulyana", 1_000m);

            Console.WriteLine($"A до: {accA.Balance}  B до: {accB.Balance}");
            bank.Transfer(accA, accB, 500m, DateTime.UtcNow, "перевод другу");
            Console.WriteLine($"A после: {accA.Balance}  B после: {accB.Balance}");

            Console.WriteLine("=== ЖУРНАЛ АУДИТА ===");
            Console.WriteLine(BankAccount.Audit);
        }
    }
}