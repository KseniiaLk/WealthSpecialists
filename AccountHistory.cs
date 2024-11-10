using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public class AccountHistory
    {
        public AccountHistory(Account account, double amountTransfered)
        {
            _previusBalance = account._accountBalance;
            _amountTransfered = amountTransfered;
            _postBalance = account._accountBalance + amountTransfered;
        }
        public DateTime _date = DateTime.Now;
        public double _previusBalance { get; set; }
        public double _amountTransfered { get; set; }
        public double _postBalance { get; set; }
    }
}
