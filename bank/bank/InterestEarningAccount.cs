using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace bank
{
    public class InterestEarningAccount:BankAccount
    {
        public InterestEarningAccount(string name, decimal initialBalance)
             : base(name, initialBalance)
            {

        }
        // override позволяет в дочернем классе определить новую реализацию
        // метода PerformMonthAndTransactions
        public override void PerformMonthAndTransactions()
        {
                if(Balance > 500m)
            {
                decimal interest = Balance * 0.02m;
                MakeDeposite(interest, DateTime.UtcNow, "Apply month interest");
            }
                
        }
   

    }
}
