using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public class AccountHistory
    {
        public int _amount { get; set; }
        public string _currency { get; set; }
        public DateTime _date = DateTime.Now;
        public Guid _accountNumberFrom;
        public Guid _accountNumberTo;

        public AccountHistory(int amount, string currency, Guid accountNumberFrom, Guid accountNumberTo)
        {
            _amount = amount;
            _currency = currency;
            _accountNumberFrom = accountNumberFrom;
            _accountNumberTo = accountNumberTo;

        }

    }
}
