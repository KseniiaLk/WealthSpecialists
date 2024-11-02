using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public abstract class User
    {
        string _userName { get; set; }
        string _passWord { get; set; }
        Guid _id { get; set; }

        protected User(string userName, string passWord, Guid id)
        {
            _id = id;
            _userName = userName;
            _passWord = passWord;
        }
    }

    public class Customer : User
    {
        //list containing accounts user has, using abstract class account as <Type>
        public List<Account> _accounts = new List<Account>();
        
        public Customer (string userName, string passWord, Guid guid) : base (userName, passWord, guid)
        {

        }
        public void Add_account(Account account)
        {
            _accounts.Add(account);
        }
    }

    public class Manager : User
    {
        public Manager (string userName, string passWord, Guid guid) : base (userName, passWord, guid)
        {


        }
    }

}
