using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public class Bank_Applikation
    {
        //these list and dictionarys will hold the user database, at first we will use the list for simplicitys sake then
        //when we know the setup for the applikation we will use will start using the dictionary later
        Dictionary<string,User> _userRegistry = new Dictionary<string,User>();
        ICollection<User> _UserRegistry = new List<User>();


        // the bank applikation will use the other classes to perform the actions/methods that are neaded,
        //the exakt methos will have to be discusses in a  group metting
        public User _user { get; set; }
        public  ICollection<Account> _accounts { get; set; }

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
