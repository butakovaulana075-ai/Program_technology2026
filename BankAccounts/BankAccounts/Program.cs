using BankAccounts;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Account> accounts = new List<Account>
            {
                new DebitAccount("40817810001", "Ivanov", 250_000m, 50_000m),
                new CreditAccount("40817810002", "Petrov", -50_000m, 200_000m),
                new DepositAccount("40817810003", "Sidorov", 500_000m, 0.07m)
            };

            foreach (Account account in accounts)
            {
                account.DisplayInfo();

                if (account is IWithdrawable withdrawable)
                {
                    withdrawable.Withdraw(1_000m);
                }
                else
                {
                    Console.WriteLine($"{account.Number} cannot withdraw.");
                }

            }
        }
    }
}