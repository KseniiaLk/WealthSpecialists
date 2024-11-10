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
        public List<User> _UserRegistry = new List<User>
        { new Manager("Raidar", "Bääst"),
        new Customer("Erik", "password")
        };

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
                Console.WriteLine("[1]log in to account");
                User user = Login();
                if (user == null)
                {
                    Console.WriteLine("Invalid username or password, please try again.");
                    continue;
                }
                int? userType = access(user);
                switch (userType)
                {
                    case 1:
                        Customer_Menu((Customer)user);
                        break;
                    case 2:
                        Manager_Menu((Manager)user);
                        break;
                    default:
                        Console.WriteLine("Usertype not found.");
                        break;
                }
            }
        }
        public void Manager_Menu(Manager manager)
        {
            while (true)
            {
                Console.WriteLine("[1] Create new Customer");
                Console.WriteLine("[2] Update currency");
                Console.WriteLine("[3] Log out");
                int.TryParse(Console.ReadLine(), out int input);

                switch (input)
                {
                    case 1:
                        manager.Add_user(new UserService(this));
                        break;
                    case 2:
                        //uppdatring av currency
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Invalid selection please try again.");
                        break;

                }
            }
        }
        public void Customer_Menu(Customer customer)
        {
            while (true)
            {
                Console.WriteLine("[1] Account overview");
                Console.WriteLine("[2] Money transfer");
                Console.WriteLine("[3] Create a new account");
                Console.WriteLine("[4] Apply for loan");
                Console.WriteLine("[5] Log out");
                int.TryParse(Console.ReadLine(), out int input);

                switch (input)
                {
                    case 1:
                        customer.Veiw_accounts_information();
                        break;
                    case 2:
                        //metod för överföring av pengar
                        break;
                    case 3:
                        Account newAccount = new SavingsAccount(1000, "SEK");
                        Add_acc(newAccount);
                        break;
                    case 4:
                        //customer.Request_loan();
                        Console.WriteLine("Test");
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Invalid selection please try again.");
                        break;

                }
            }

        }

        private static void Veiw_accounts_information(User user)
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

        private static void Aprove_loan(Account account, double loan)
        {
            account._LoanAmount += loan;
        }

        public double Request_loan(Account account)
        {
            Console.WriteLine($"You can Loan a maximum amount of {account._accountBalance * 5} at an interest rate of {account._interestRate}% \n how much yould you like to Loan?: ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount <= account._accountBalance * 5)
            {
                Console.WriteLine($"You have Requested a loan of {amount} at {account._interestRate}, the total amount to be repayed will be {amount * (1 + (account._interestRate / 100))}");
                return amount;
            }
            else
            {
                Console.WriteLine("your loan has been denied");
                return 0;
            }
        }

        private static void Veiw_interest(Account account)
        {
            Console.WriteLine($"Your account {account._accountID} currently has a interest rate of {account._interestRate} per annum");
        }

        private void Add_acc(Account account)
        {
            if (_user is Customer customer && account is SavingsAccount acc)
            {
                customer.Add_account(account);
                Console.WriteLine($"A new account has been added with an interest rate of {acc._interestRate} per Annum");
            }
            else
            {
                Console.WriteLine("your User prifile does not allowe accounts to be added, try switching accounts to accses this funtion");
            }
        }

        private static int? access(User user)
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

        private User Login()
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

        private double CurrencyConverter(Account account)
        {
            if (account._currencyType == "dollar")
            {
                double output = _dollar / account._accountBalance;
                return output;
            }
            else if (account._currencyType == "Euro")
            {
                double output = _euro / account._accountBalance;
                return output;
            }
            Console.WriteLine("Conversion failed");
            return 0;

        }
    }
}
