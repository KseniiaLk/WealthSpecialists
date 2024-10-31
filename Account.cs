using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
     public abstract class Account
    {
        public Account(double accountBalance, string currencyType)
        {
            _accountBalance = accountBalance;
            _accountNumber = Guid.NewGuid();
            _currencyType = currencyType;
        }

        public double _accountBalance = 0;
        public Guid _accountNumber { get; set; }
        public string _currencyType { get; set; }
    }
    internal class SavingsAccount : Account
    {
        public SavingsAccount(double accountBalance, string currencyType) : base(accountBalance, currencyType)
        {
            _interestRate = 2.5;
        }

        public double _interestRate { get; set; }

    }
    internal class ForeingCurrency : Account
    {
        public ForeingCurrency(double accountBalance, string currencyType, string currency) : base(accountBalance, currencyType)
        {
            _currencyType = currency;
        }

        public string _currencyType { get; set; }
    }

    
}
