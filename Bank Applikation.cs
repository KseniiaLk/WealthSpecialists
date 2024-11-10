using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WealthSpecialists
{
    public class Bank_Applikation
    {

        Dictionary<string, User> _userRegistry = new Dictionary<string, User>();
        public List<User> _UserRegistry = new List<User> { new Manager("Raidar", "Bääst"), new Customer("Erik", "password")};
        public double _sek = 1;
        public double _dollar = 11;
        public double _euro = 12;


        public User _user { get; set; }
        public ICollection<Account> _accounts { get; set; }
        public Account _account { get; set; }

        public void menu()
        {
            while (true)
            {
              Console.WriteLine("[1]logg in to account");
               _user = Login();
                User user = Login();
                int? v = access(_user);
                switch (v)
                { 
                    case 1:
                        Console.WriteLine("[1]KontoÖversikt");
                        Console.WriteLine("[2]Överföring av pengar");
                        Console.WriteLine("[3]Skapa nytt Konto");
                        Console.WriteLine("[4]Ansök om lån");
                        Console.WriteLine("[5]Logga ut");
                        int.TryParse(Console.ReadLine(), out int input);
                        switch (input)
                        {
                            case 1:
                                user.Veiw_accounts_information();




                        }
                        break;
 


                    case 2:
                        Console.WriteLine("[1]Skapa ny kund");
                        Console.WriteLine("[2]Uppdatera valutaomvandling");
                        Console.WriteLine("[3]Logga ut");
                        int.TryParse(Console.ReadLine(), out int userInput2);
                        break;
                    case null:
                        Console.WriteLine("fel användarnamn/lösenord");
                        //+ a counter to count number of logins for later
                        break;

                }
            }
        }
        public  User Login()
        {
            Console.WriteLine("User Name: ");
            string username = Console.ReadLine();
            Console.WriteLine("Password: ");
            string password = Console.ReadLine();
            foreach (var user in _UserRegistry)
            {
                if (user._userName == username && user._passWord == password)
                {
                    return user;
                }
            }
            return null;
        }
        static int? access(User user)
        {
            if (user is Customer)
            {
                return 1;
            }
            else if (user is Manager)
            {
                return 2;
            }
            else
                return null;
        }
        public void Add_acc(Account account)
        {
            if (_user is Customer kund && account is SavingsAccount acc)
            {
                kund.Add_account(account);
                Console.WriteLine($"A new account has been added with an interest rate of {acc._interestRate} per Annum");
            }
            else
            {
                Console.WriteLine("your User prifile does not allowe accounts to be added, try switching accounts to accses this funtion");
            }
        }
        public void Veiw_interest(Account account)
        {
            Console.WriteLine($"Your account {account._accountID} currently has a interest rate of {account._interestRate} per annum");
        }
        // Method for requesting loan, it also handels 
        public double Request_loan(Account account)
        {
            Console.WriteLine($"You can Loan a maximum amount of {account._accountBalance * 5} at an interest rate of {account._interestRate}% \n how much yould you like to Loan?: ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount <= account._accountBalance * 5)
            {
                Console.WriteLine($"You have Requested a loan of {amount} at {account._interestRate}, the total amount to be repayed will be {amount * (1 + (account._interestRate/100))}");
                return amount;
            }
            else
            {
                Console.WriteLine("your loan has been denied");
                return 0;
            }
        }
        public void Aprove_loan(Account account, double loan)
        {
            account._LoanAmount += loan;
        }
        public void Veiw_accounts_information(User user)
        {
            if (user is Customer customer)
            {
                foreach (Account item in customer._accounts)
                {
                    Console.WriteLine($"Account: {item._accountname}\nBalance: {item._accountBalance} {item._currencyType}");
                }
            }
            else
            {
                Console.WriteLine("User is not a customer, cannot veiw account information");
            }
        }


        public double CurrencyConverter(Account account)
        {
            if(account._currencyType == "dollar")
            {
                double output = _dollar / account._accountBalance;
                return output;
            }
            else if(account._currencyType == "Euro")
            {
                double output = _euro / account._accountBalance;
                return output;
            }
            Console.WriteLine("Conversion failed");
            return 0;

        }

    }
}
