using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public class Bank_Applikation
    {

        Dictionary<string, User> _userRegistry = new Dictionary<string, User>();
        public ICollection<User> _UserRegistry = new List<User> { new Customer("Erik", "password") };


        public User _user { get; set; }
        public  ICollection<Account> _accounts { get; set; }
        public Account _account{ get; set; }

        public void Add_acc(Account account)
        {
            if (_user is Customer kund)
            {
                kund.Add_account(account);
            }
            else
            {
                Console.WriteLine("your User profile has no accounts");
            }
        }
    }
}
