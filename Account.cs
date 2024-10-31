using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    internal class Account
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
}
