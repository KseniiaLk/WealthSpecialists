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
            if (_user is Customer kund && account is SavingsAccount acc)
            {
                kund.Add_account(account);
                Console.WriteLine($"A new account has been added with an interest rate of {acc._interestRate} per Annum");
            }
            else 
            {
                Console.WriteLine("yuor User prifile does not allowe accounts to be added, try switching accounts to accses this funtion");
            }
        }
        public void veiw_interest(Account account)
        {
            Console.WriteLine($"Your account {account._accountNumber} currently has a interest rate of {account._interestRate} per annum");
        }

    }
}
