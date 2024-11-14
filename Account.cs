using System;
using System.Collections.Generic;

namespace WealthSpecialists
{
    public abstract class Account
    {
        public Account(double accountBalance, string currencyType, int accountNumber)
        {
            _accountNumber = accountNumber;
            _accountBalance = accountBalance;
            _accountID = Guid.NewGuid();
            _currencyType = currencyType;
        }

        public double _accountBalance = 0;
        public double _LoanAmount = 0;
        public int _accountNumber = 1;
        public Guid _accountID { get; set; }
        public string _currencyType { get; set; }
        public double _interestRate { get; set; }
        public List<AccountHistory> _accounthistory { get; set; }
    }

    internal class SavingsAccount : Account
    {
        public SavingsAccount(double accountBalance, string currencyType, int accountNumber) : base(accountBalance, currencyType, accountNumber)
        {
            _interestRate = 2.5;
            _currencyType = "SEK";
        }
    }

    internal class ForeingCurrency : Account
    {
        public ForeingCurrency(double accountBalance, string currencyType, int accountNumber) : base(accountBalance, currencyType, accountNumber)
        {
        }
    }
}