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
        public IDictionary<int,IList<int>> _transactionHistory = new Dictionary<int,IList<int>>();
        public List<AccountHistory> _accountHistory = new List<AccountHistory>();


        public Customer(string userName, string passWord) : base(userName, passWord)
        {

        }
        // Both methods below can be added togeter to create Logic for transfering money
        // Ither seperatly or combined in another method

        // Removes money from an account, sum is the amount
        public void Remove_money(Account account, double sum)
        {
            account._accountBalance -= sum;
        }
        // Adds money to an account, sum is the amount
        public void Add_money(Account account, double sum)
        {
            account._accountBalance += sum;
        }
        // selects an account based on account name, the string promt makes the method reusebal for different meny choises
        public Account Select_account(string prompt)
        {
            foreach (Account item in _accounts)
            {
                Console.WriteLine($"item{item._accountname}");
            }
            //asking the user what acccount they want to select
            Console.WriteLine(prompt);
            string choise = Console.ReadLine();
            foreach(Account item in _accounts)
            {
                if (choise ==item._accountname)
                {
                    return item;
                }                
            }
            return null;
        }
        // adds a account to the list of accounts, can be used when creating accounts or when moving accounts

        public Account Create_account(double balance, string currencyType)
        {
            Account newAccount = new SavingsAccount(balance, currencyType);
            return newAccount;
        }

        public void TransferBetweenUsers(Bank_Applikation bankapp)
        {
            Console.WriteLine("From which account would you like to transfer money from?");
            Veiw_accounts_information();
            int.TryParse(Console.ReadLine(), out int accountFrom);
            Account from = _accounts[accountFrom - 1]; // here we can use accountselector method when finished
            Console.WriteLine("how much money would you like to send?");
            int.TryParse(Console.ReadLine(), out int money);

            if (from._accountBalance < money )
            {
                Console.WriteLine("Your account does not have enough balance"); //need to make a loop so user can input new sum
                
            }


            Console.WriteLine("What is the username of person you would like to transfer money to?");
            string transferTargetUser = Console.ReadLine();
            Console.WriteLine("what accountnumber would you like to send money to");
            int.TryParse(Console.ReadLine(), out int accounttarget);
            foreach(User user in bankapp._UserRegistry)
            {
                if(transferTargetUser == user._userName) 
                {
                    if(user is Customer customer)
                        foreach (var account in customer._accounts)
                        {
                            if(accounttarget == account._accountNumber)
                            {
                                customer.Add_money(account, money);
                                this.Remove_money(from, money);      //  "this" will remove money from the user accessing method
                                                                    //transferBetweenUsers tested this method it works, very nice
                                                                    //just  need to add method to update transactionHistory
                            }
                            
                        }
                }
                else
                    Console.WriteLine("Username/account not found");
            }
         
        }

        public void Add_account(Account account)
        {
            _accounts.Add(account);
            

        }
        public void Veiw_accounts_information()
        {
            int num = 1;
            foreach (Account item in _accounts)
            {
                Console.WriteLine($"{num} +\nAccount: {item._accountNumber}\nBalance: {item._accountBalance} {item._currencyType}");
                num++;
            }
        }

        public void Weiv_detailed_account_information(Account account)
        {
            Console.WriteLine($"Account Name: {account._accountname}Current balance: {account._accountBalance}\nCurrent Debt: {account._LoanAmount}\nCurrency Type{account._currencyType} \nInterestrate: {account._interestRate}\nAccount ID {account._accountID}");
        }
        public void Logg_history(int amount, string currency, Guid accountFrom, Guid accountTo)
        {
            AccountHistory accountlogg = new AccountHistory(amount, currency, accountFrom, accountTo);
            _accountHistory.Add(accountlogg);
            Console.WriteLine("Transaktion has been saved to your transaktion history.");
        }
        //Michaels bästa logik
        public void Transfer(Bank_Applikation _bankApp)
        {
            Veiw_accounts_information();
            Console.WriteLine("From which account would you like to transfer?");
            int.TryParse(Console.ReadLine(), out int input);
            Console.WriteLine("Choose the account you want to transfer to?");
            int.TryParse(Console.ReadLine(), out int inputtwo);

            if (input == inputtwo)
            {
                Console.WriteLine("Sorry we cant transfer");
            }
            Console.WriteLine("How much money would you like to transfer?");
            int.TryParse(Console.ReadLine(), out int inputthree);
            if (inputthree > _accounts[input-1]._accountBalance)
            {
                Console.WriteLine("Still not enough money");
            }
            else if (_accounts[inputtwo - 1] is ForeingCurrency)
            {
                if (_accounts[input - 1]._currencyType == "$") 
                {
                    double output = inputthree / _bankApp._dollar;
                    _accounts[inputtwo - 1]._accountBalance += output;
                    _accounts[input - 1]._accountBalance -= inputthree;
                }
                else if(_accounts[input - 1]._currencyType == "£")
                 {
                    double output = inputthree / _bankApp._euro;
                    _accounts[inputtwo - 1]._accountBalance += output;
                    _accounts[input - 1]._accountBalance -= inputthree;
                }
            }

            else if(inputthree <= _accounts[input - 1]._accountBalance)

            {
                Console.WriteLine("You can transfer");
                _accounts[input - 1]._accountBalance -= inputthree;
                _accounts[inputtwo - 1]._accountBalance += inputthree;
                Console.WriteLine("Transfer is done");

            }
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
        // Creates a account, and returns it for use by the system. can for example be used by
        // "Add_account" method under Coustomer
        public Account Create_account(double balance, string currencyType)
        {
            Account newAccount = new SavingsAccount(balance, currencyType);
            return newAccount;
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
