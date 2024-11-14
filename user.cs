using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public abstract class User
    {
        public string _userName { get; set; }
        public string _passWord { get; set; }
        public bool _isLocked { get; set; }
        private Guid _id { get; set; }

        public User(string userName, string passWord)
        {
            _id = Guid.NewGuid();
            _userName = userName;
            _passWord = passWord;
        }
    }

    public class Customer : User
    {
        public List<Account> _accounts = new List<Account>();

        public Customer(string userName, string passWord) : base(userName, passWord)
        {
        }

        public void Remove_money(Account account, double sum)
        {
            account._accountBalance -= sum;
        }

        public void Add_money(Account account, double sum)
        {
            account._accountBalance += sum;
        }

        public Account Select_account(string prompt)
        {
            View_acc();
            Console.WriteLine(prompt);
            int choise;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choise) && choise > 0 && choise <= _accounts.Count)
                {
                    return _accounts[choise - 1];
                }
                else if (_accounts.Count == 0)
                {
                    Console.WriteLine("you have no accounts");
                    return null;
                }
                else
                {
                    Console.WriteLine("Not a valid account number, try again");
                }
            }
        }

        public void Log_transaction(Account account, double amountTrensfered) //behöver kontroleras mot metoderna den används i för att se om inputen är positiv eller negativ
        {
            AccountHistory history = new AccountHistory(account, amountTrensfered);
            account._accounthistory.Add(history);

        }
        public void veiw_accountHitory(Account account)
        { 
            foreach (AccountHistory item in account._accounthistory)
            {
                Console.WriteLine($"{item._date}: previous balance: {item._previousBalance} transfered {item._amountTransfered} new balance{item._postBalance}");
            }
        }

        public void Create_account(double balance, string currencyType, Bank_Application bank)
        {
            Account newAccount = new SavingsAccount(balance, currencyType, bank._totalAccounts);
            _accounts.Add(newAccount);
            bank._totalAccounts++;
        }

        public void Create_Currencyaccount(double balance, string currencyType, Bank_Application bank)
        {
            Account newAccount2 = new ForeingCurrency(balance, currencyType, bank._totalAccounts);
            _accounts.Add(newAccount2);
            bank._totalAccounts++;
        }

        public void transferBetweenUsersLogic(Bank_Application bankapp, string transferTargetUser, int accounttarget, Account from, int money)
        {
            Customer Origin = this;
            foreach (User user in bankapp._UserRegistry)
            {
                if (transferTargetUser == user._userName)
                {
                    if (user is Customer customer)
                        foreach (var account in customer._accounts)
                        {
                            if (accounttarget == account._accountNumber)
                            {
                                customer.Add_money(account, money);
                                Origin.Remove_money(from, money);
                            }
                        }
                }
            }
        }

        public void TransferBetweenUsers(Bank_Application bankapp)
        {   
            Console.WriteLine("From which account would you like to transfer money from? Press Enter to return to menu");
            View_acc();
            int.TryParse(Console.ReadLine(), out int accountFrom);

            if (accountFrom == 0)
                return;
            Account from;
            try
            {
                 from = customer_accounts[accountFrom - 1];
            }
            catch (ArgumentOutOfRangeException)
            {
                
                Console.WriteLine("Enter a number from 1 to " + customer_accounts.Count());// if user inputs wrong number restart method = gets to input number again
                TransferBetweenUsers(bankapp);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message); // catches all other errors and exits method and returns to meny
                return;
            }
            
            Console.WriteLine("how much money would you like to send?");
            int.TryParse(Console.ReadLine(), out int money);

            while (from._accountBalance < money) //reruns loop untill user put in amount below total of balance
            {
                Console.WriteLine("Your account does not have enough balance"); 
                Console.WriteLine("how much money would you like to send? Press Enter or 0 to return to meny");
                int.TryParse(Console.ReadLine(), out money);
                if (money == 0) 
                    return;

            }

            Console.WriteLine("What is the username of person you would like to transfer money to?");
            string transferTargetUser = Console.ReadLine();
            Console.WriteLine("what accountnumber would you like to send money to");
            int.TryParse(Console.ReadLine(), out int accounttarget);

            bool success = false;
            IEnumerable<User> readuser = bankapp._UserRegistry; // check if username and accountnumber exists set bool to true
            foreach (User item in readuser)
            {
                if (transferTargetUser == item._userName)
                {
                    Customer cust = (Customer)item;
                    foreach (var account in cust.customer_accounts)
                        if (accounttarget == account._accountNumber)
                        {
                            Console.WriteLine("Transfer in progress");
                            success = true;
                        }

                
                }

            }
           

            if(success == true) // if username/accountnumber found add to task
            { 
                Task task = new Task(() =>
                {
                transferBetweenUsersLogic(bankapp, transferTargetUser, accounttarget, from, money);
                });
                bankapp.test.Enqueue(task);
             }
            else
            { 
                Console.WriteLine("Transfer failed could not find username/accountnumber, press Enter to return to menu");
                Console.ReadLine();
            }
    }

        public void View_acc()
        {
            int num = 1;
            if (_accounts.Count ==0)
            {
                Console.WriteLine("You have no accounts");
            }
            foreach (Account item in _accounts)
            {
                if (item is SavingsAccount)

                    Console.WriteLine($"Account: {num} Accountnumber: {item._accountNumber} Balance: {item._accountBalance:F} {item._currencyType}, Intrest {item._interestRate:F}%");

                else
                    Console.WriteLine($"Account: {num} Accountnumber: {item._accountNumber} Balance: {item._accountBalance:F} {item._currencyType}");
                num++;
            }
        }

        public void View_detailed(Account account)

        {
            Console.WriteLine("═══════════════════════════════");
 
            Console.WriteLine($"Account Number: {account._accountNumber}\nCurrent balance: {account._accountBalance:F}\nCurrent Debt: {account._LoanAmount}\nCurrency Type: {account._currencyType} \nInterestrate: {account._interestRate}\nAccount ID {account._accountID}");

            Console.WriteLine("═══════════════════════════════");
        }


        public void View_acc_history(Account account)
        {
            int num = 1;
            foreach (AccountHistory item in account._accounthistory)
            {
                Console.WriteLine($" {num}. {item._date} {item._previousBalance} {item._amountTransfered:F} Balance after transaction: {item._postBalance:F}");
            }
        }

        public void TransferLogic(int input, int inputtwo, int inputthree, Bank_Application _bankApp)
        {
            Customer customer = this;
            if (_accounts[inputtwo - 1] is ForeingCurrency)
            {
                if (_accounts[inputtwo - 1]._currencyType == "USD")
                {
                    double output = inputthree / _bankApp._dollar;
                    _accounts[inputtwo - 1]._accountBalance += output;
                    _accounts[input - 1]._accountBalance -= inputthree;
                }
                else if (_accounts[inputtwo - 1]._currencyType == "EUR")
                {
                    double output = inputthree / _bankApp._euro;
                    _accounts[inputtwo - 1]._accountBalance += output;
                    _accounts[input - 1]._accountBalance -= inputthree;
                }
            }
            else if (inputthree <= _accounts[input - 1]._accountBalance)

            {
                _accounts[input - 1]._accountBalance -= inputthree;
                _accounts[inputtwo - 1]._accountBalance += inputthree;
            }
        }

        public void Transfer(Bank_Application _bankApp)
        {
            if (customer_accounts.Count() == 0)
            {
                Console.WriteLine("You dont have any accounts. Press Enter to return to main menu");
                Console.ReadLine();
                return;
            }
            View_acc();
            Console.WriteLine("From which account would you like to transfer? Press 0 or Enter to return to main menu");
            int.TryParse(Console.ReadLine(), out int input);
            if (input < 0 || input > customer_accounts.Count)
            {
                Console.WriteLine("Please enter a valid number between 1 and " + customer_accounts.Count());
                Transfer(_bankApp); // restarts method
                return;
            }
            else if (input == 0)//return to main menu
                return;
            Console.WriteLine("Choose the account you want to transfer to? Press 0 or Enter to return to main menu");
            int.TryParse(Console.ReadLine(), out int inputtwo);
            if (inputtwo < 0 || inputtwo > customer_accounts.Count)
            {
                Console.WriteLine("Please enter a valid number between 1 and " + customer_accounts.Count());
                Transfer(_bankApp);
                return;
            }

            else if (inputtwo == 0)
                return;
            else if (input == inputtwo)

            {
                Console.WriteLine("you have selected the same account twice");
                Transfer(_bankApp);                                                         
                return;

            }
            int inputthree;
            while (true)
            { 
                Console.WriteLine("How much money would you like to transfer? Press Enter or 0 to return to main menu");
                int.TryParse(Console.ReadLine(), out inputthree);
                if (inputthree > customer_accounts[input - 1]._accountBalance)
                {
                    Console.WriteLine("You do not have enough money in your account");
                }
                else if (inputthree == 0) //back to main menu
                    return;
                else
                    break;
            }
        Task task = new Task(() =>
            {
                TransferLogic(input, inputtwo, inputthree, _bankApp);
            });
            _bankApp.test.Enqueue(task);
        }
    }

    public class Manager : User
    {
        public Manager(string userName, string passWord) : base(userName, passWord)
        {
        }

        public Account Create_account(double balance, string currencyType, int accountNumber)
        {
            Account newAccount = new SavingsAccount(balance, currencyType, accountNumber);
            return newAccount;
        }

        public void Add_user(Bank_Application bank)
        {
            Console.WriteLine("Vänligen skriv in ditt användarnamn");
            string input = Console.ReadLine();
            Console.WriteLine("Vänligen skriv in ditt lösenord");
            string pwinput = Console.ReadLine();
            Customer customer = new Customer(input, pwinput);
            bank._UserRegistry.Add(customer);
            Console.WriteLine($"Användaren: {input} har blivit skapad.");
        }

        public void UpdateCurrency(Bank_Application bank)
        {
            Console.WriteLine("Which currency would you like to update");
            Console.WriteLine($"1: Dollar which is at the moment worth : {bank._dollar} in sek");
            Console.WriteLine($"2: Euro which is at the moment worth : {bank._euro} in sek");

            int.TryParse(Console.ReadLine(), out int input);
            if (input == 1)
            {
                Console.WriteLine("Enter the new value for dollar in sek");
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
