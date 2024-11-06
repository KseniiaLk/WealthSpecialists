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

        public bool _isLocked { get; set; }
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
        public List<AccountHistory> _accountHistory = new List<AccountHistory>();

        
        public Customer (string userName, string passWord) : base (userName, passWord)
        {

        }
        public void Add_account(Account account)
        {
            _accounts.Add(account);
        }

        /*public void Add_History()
        {
            AccountHistory accountHistory = new AccountHistory();
        }
        future add accounthistory to list
        */

    }

    public class Manager : User
    {
        public Manager (string userName, string passWord) : base (userName, passWord)
        {

        }
        public void Add_user(UserService userService)
        {
            Console.WriteLine("Vänligen skriv in ditt användarnamn");
            string input = Console.ReadLine();
            Console.WriteLine("Vänligen skriv in ditt lösenord");
            string pwinput = Console.ReadLine();
            Customer customer = new Customer (input, pwinput);
            userService.users.Add(customer);
            Console.WriteLine($"Användaren: {input} har blivit skapad.");

        }
    }

}
