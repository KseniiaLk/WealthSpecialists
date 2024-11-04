using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public abstract class User
    {
        public string _userName { get; set; }
        public string _passWord { get; set; }
        Guid _id { get; set; }

        public User(string userName, string passWord)
        {
            _id = Guid.NewGuid();
            _userName = userName;
            _passWord = passWord;
        }
    }

    public class Customer : User
    {
        //list containing accounts user has, using abstract class account as <Type>
        public List<Account> _accounts = new List<Account>();
        
        public Customer (string userName, string passWord) : base (userName, passWord)
        {

        }
        public void Add_account(Account account)
        {
            _accounts.Add(account);
        }
    }

    public class Manager : User
    {
        public Manager (string userName, string passWord) : base (userName, passWord)
        {


        }
    }

}
