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
            _accountID = Guid.NewGuid();
            _currencyType = currencyType;
        }

        public int _accountNumber = 1;

        public double _accountBalance = 0;
        public double _LoanAmount = 0;
        public string _accountname { get; set; }
        public Guid _accountID { get; set; }
        public string _currencyType { get; set; }
        public double _interestRate { get; set; }
        public List<AccountHistory> _accounthistory { get; set; }
    }

    internal class SavingsAccount : Account
    {
        public SavingsAccount(double accountBalance, string currencyType) : base(accountBalance, currencyType)
        {
            _interestRate = 2.5;
        }
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