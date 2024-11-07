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


        public Customer(string userName, string passWord) : base(userName, passWord)
        {

        }
        public void Add_account(Account account)
        {
            _accounts.Add(account);
        }
        public void Veiw_accounts_information()
        {
            foreach (Account item in _accounts)
            {
                Console.WriteLine($"\nAccount: {item._accountname}\nBalance: {item._accountBalance} {item._currencyType}");
            }
        }
        public void Weiv_detailed_account_information(Account account)
        {
            Console.WriteLine($"Account Name: {account._accountname}Current balance: {account._accountBalance}\nCurrent Debt: {account._LoanAmount}\nCurrency Type{account._currencyType} \nInterestrate: {account._interestRate}\nAccount ID {account._accountID}");

        public void Logg_history(int amount, string currency, Guid accountFrom, Guid accountTo)
        {
            AccountHistory accountlogg = new AccountHistory(amount, currency, accountFrom, accountTo);
            _accountHistory.Add(accountlogg);
            Console.WriteLine("Transaktion has been saved to your transaktion history.");
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

        public void UpdateCurrency(Bank_Applikation bank)
        {
            Console.WriteLine("Which currency would you like to update");
            Console.WriteLine($"1: Dollar which is at the moment worth : {bank._dollar} in sek");
            Console.WriteLine($"2: Euro which is at the moment worth : {bank._euro} in sek");

            int.TryParse(Console.ReadLine(),out int input);
            if (input == 1)
            {
                Console.WriteLine("Énter the new value for dollar in sek");
                int.TryParse(Console.ReadLine(), out int inputDollar);
                bank._dollar = inputDollar;
            }
            if (input == 2)
            {
                Console.WriteLine("Enter the new value for euro in sek");
                int.TryParse(Console.ReadLine(), out int inputEuro);
                bank._euro = inputEuro;
            }

        }
    }

}
