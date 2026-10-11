using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    // BankAccount - потомок класса object 
    public class BankAccount
    {
        private readonly decimal _minimalBalance;
        private List<Transaction> _allTransactions = new List<Transaction>();
        public string Owner { get; private set; }
        public string Number { get; }
        public decimal Balance
        {
            get
            {
                decimal balance = 0;
                foreach (var transaction in _allTransactions)
                {
                    balance += transaction.Amount;
                }
                return balance;
            }
        }



        private static int s_accountNumberSeed = 1000000000;

        public BankAccount(string name, decimal initialBalance): this(name, initialBalance, 0)
        {


        }

        public BankAccount(string name, decimal initialBalance, decimal minimalBalance)
        {
            // this.balance = initialBalance;
            
            Owner = name;
            Number = s_accountNumberSeed.ToString();
            s_accountNumberSeed++;
            _minimalBalance = minimalBalance;
            if (initialBalance > 0)
            {
                MakeDeposite(initialBalance, DateTime.UtcNow, "initial balance");
            }
        }
        public void MakeDeposite(decimal amout, DateTime date, string note)
        {
            if (amout <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amout), "Amount of deposite must be positive");
            }


            var deposite = new Transaction(amout, date, note);
            _allTransactions.Add(deposite);

        }

        public void MakeWithdrawal(decimal amout, DateTime date, string note)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amout);
            Transaction? overdraftTransaction = CkeckWithdrawalLimit(Balance - amout < _minimalBalance);
            Transaction? withdrawal = new(-amout, date, note);

            _allTransactions.Add(withdrawal);

            if (overdraftTransaction is not null)
                _allTransactions.Add(overdraftTransaction);
        }

        // protected - модификатор доступа, который означает,
        // что этот метод можно вызвать только из текущего и дочернего класса 
        // Клиент (внешний код) данный метод вызвать не может
      
        protected virtual Transaction? CkeckWithdrawalLimit(bool isOverdrawn)
        {
            if (isOverdrawn)
            {
                throw new InvalidOperationException("Not sufficient rubls for this withdrawal");
            }
            else
            {
                // default - содержит значение по умолчанию, так как тип возвращаемого значения - ссылочный, то
                // default = null
                return default;
            }
        }


        public string GetAccountHistory()
        {
            var report = new StringBuilder();
            decimal balance = 0;
            report.AppendLine("Data\t\tAmount\tBalance\tNote");
            foreach (var item in _allTransactions)
            {
                balance += item.Amount;
                report.AppendLine($"" +
                    $"{item.Date.ToShortDateString()}\t" +
                    $"{item.Amount}\t{balance}\t{item.Note}");
            }
            return report.ToString();
        }
        // Ключевое слово virtual позволяет в дочернем классе предоставить другую реализацию 
        // Метода PerformMonthAndTransactions
        public virtual void PerformMonthAndTransactions()
        {

        }

        // переопределяем метод базового класса - класса object 
        // toString возвращает строку с информацией об объекте 
        public override string ToString()
        {
            return $"Owner: {Owner}\taccount number: {Number} (тип счета {GetType()})";
        }
    }
}