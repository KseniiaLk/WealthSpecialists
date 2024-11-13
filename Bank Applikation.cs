using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public class Bank_Application
    {
        public List<User> _UserRegistry = new List<User> { new Manager("Raidar", "Bääst"), new Customer("Erik", "password") , new Customer("o","o")};
        public Timer _timer;
        public bool _timerRunning = false;
        public double _sek = 1;
        public double _dollar = 11;
        public double _euro = 12;
        public int _totalAccounts = 100;

        public Queue<Task> test = new Queue<Task>();

        public async void taskstarter(Queue<Task> queue)
        {
            if (_timerRunning) return;

            _timerRunning = true;
            while (queue.Count > 0)
            {
                Task task = queue.Dequeue();
                task.Start();
                await task;
            }
            Console.WriteLine("All tasks completed.");
            _timerRunning = false;
        }

        public void TimerCallback(object state
            )
        {
            taskstarter(test);
        }

        public void menu()
        {
            Title();
            _timer = new Timer(TimerCallback, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(0.5)); // first time we start the meny we start timer starts goes to 1 minute then 5
            int limit = 0;
            while (limit < 4)
            {
                Console.WriteLine("[1]log in to account");
                User user = Login();
                if (user == null)
                {
                    limit++;
                    Console.WriteLine($"Invalid username or password, you have: {3 - limit} attempts left .");
                    if (limit == 3)
                    {
                        Console.WriteLine("To many failed attempts the program will now close.");
                        limit = -1;

                        if (limit == -1)
                        {
                            Environment.Exit(0);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                switch (user)
                {
                    case Customer:
                        Console.Clear();
                        Title();
                        Customer_Menu((Customer)user);
                        break;

                    case Manager:
                        Console.Clear();
                        Title();
                        Manager_Menu((Manager)user);
                        break;

                    default:

                        break;
                }
            }
        }

        public void Manager_Menu(Manager manager)
        {
            while (true)
            {
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("[1]➤ Create new Customer");
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("[2]➤ Update currency");
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("[3]➤ Clear screen");
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("[4]➤ Log out");
                Console.WriteLine("═══════════════════════════════");
                int.TryParse(Console.ReadLine(), out int input);

                switch (input)
                {
                    case 1:
                        manager.Add_user(this);
                        break;

                    case 2:
                        manager.UpdateCurrency(this);
                        break;

                    case 3:
                        Console.Clear();
                        Title();
                        break;

                    case 4:
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
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("Du är inloggad på : " + customer._userName);
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("[1]➤ Account overview");
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("[2]➤ Money transfer");
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("[3]➤ Create a new account");
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("[4]➤ Apply for loan");
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("[5]➤ Clear screen");
                Console.WriteLine("═══════════════════════════════");
                Console.WriteLine("[6]➤ Log out");
                Console.WriteLine("═══════════════════════════════");
                int.TryParse(Console.ReadLine(), out int input);

                switch (input)
                {
                    case 1:
                        customer.View_acc();
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("Would you like to see a detailed overview");
                        Console.WriteLine("Press the number corresponding to the account.");
                        Console.WriteLine("Press enter to return");
                        int.TryParse(Console.ReadLine(), out int inputcust);
                        Console.WriteLine("═══════════════════════════════");
                        if (inputcust == 0)
                            break;
                        try
                        {
                            customer.View_detailed(customer.customer_accounts[inputcust - 1]);
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }
                        break;

                    case 2:
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("Would you like to:");
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("[1]➤ Transfers money between your own accounts");
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("[2]➤ Transfer to another user");
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("[3]➤ Clear screen");
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("[4]➤ Return to previous menu");
                        Console.WriteLine("═══════════════════════════════");
                        int.TryParse(Console.ReadLine(), out int userchoice);
                        switch (userchoice)
                        {
                            case 1:
                                customer.Transfer(this);
                                break;

                            case 2:
                                customer.TransferBetweenUsers(this);
                                break;

                            case 3:
                                Console.Clear();
                                Title();
                                break;

                            default:
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("Enter 1 or 2");
                                Console.WriteLine("═══════════════════════════════");
                                break;
                        }

                        break;

                    case 3:
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("Select account type to create");
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("[1]➤ Savings Account");
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("[2]➤ ForeingCurrency Account");
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("[3]➤ Return to Previous menu");
                        Console.WriteLine("═══════════════════════════════");
                        int.TryParse(Console.ReadLine(), out int accountType);

                        switch (accountType)
                        {
                            case 1:
                                Console.WriteLine();
                                customer.Create_account(1000, "SEK", this);
                                Console.WriteLine("A new account has been created.");
                                customer.View_acc();
                                Console.WriteLine();
                                break;

                            case 2:
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("Choose what currency you want to create the account in");
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("[1]➤ $");
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("[2]➤ €");
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("[3]➤ Return to previous menu");
                                Console.WriteLine("═══════════════════════════════");
                                int.TryParse(Console.ReadLine(), out int currency);

                                switch (currency)
                                {
                                    case 1:
                                        customer.Create_Currencyaccount(1000, "USD", this);
                                        Console.WriteLine("A new account with the currency [USD] has been created.");
                                        customer.View_acc();
                                        break;

                                    case 2:
                                        customer.Create_Currencyaccount(1000, "EUR", this);

                                        Console.WriteLine("A new account with the currency [EUR] has been created.");
                                        customer.View_acc();
                                        break;

                                    case 3:
                                        break;
                                }
                                break;

                            case 3:
                                break;
                        }
                        break;

                    case 4:
                        Console.WriteLine("═══════════════════════════════");
                        this.Request_loan(customer.Select_account("Choose account"));
                        Console.WriteLine("═══════════════════════════════");
                        break;

                    case 5:
                        Console.Clear();
                        Title();
                        break;

                    case 6:
                        return;

                    default:
                        Console.WriteLine("Invalid selection please try again.");
                        break;
                }
            }
        }

        public void Request_loan(Account account)
        {
            Console.WriteLine($"You can loan a maximum amount of {account._accountBalance * 5} at an interest rate of {account._interestRate}% \n how much would you like to loan?: ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount <= account._accountBalance * 5)
            {
                Console.WriteLine($"You have Requested a loan of {amount} at {account._interestRate}, the total amount to be repayed will be {amount * (1 + (account._interestRate / 100))}");
                account._accountBalance += amount;
                account._LoanAmount += amount;
            }
            else
            {
                Console.WriteLine("your loan has been denied");
            }
        }

        private User Login()
        {
            Console.WriteLine("═══════════════════════════════");
            Console.Write("➤ User Name: ");
            string username = Console.ReadLine();
            Console.Write("➤ Password: ");
            string password = Console.ReadLine();
            Console.WriteLine("═══════════════════════════════");
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
            if (account._currencyType == "USD")
            {
                double output = _dollar / account._accountBalance;
                return output;
            }
            else if (account._currencyType == "EUR")
            {
                double output = _euro / account._accountBalance;
                return output;
            }
            Console.WriteLine("Conversion failed");
            return 0;
        }

        public void Title()
        {
            string Title = (@"
██╗    ██╗███████╗ █████╗ ██╗  ████████╗██╗  ██╗    ███████╗██████╗ ███████╗ ██████╗██╗ █████╗ ██╗     ██╗███████╗████████╗
██║    ██║██╔════╝██╔══██╗██║  ╚══██╔══╝██║  ██║    ██╔════╝██╔══██╗██╔════╝██╔════╝██║██╔══██╗██║     ██║██╔════╝╚══██╔══╝
██║ █╗ ██║█████╗  ███████║██║     ██║   ███████║    ███████╗██████╔╝█████╗  ██║     ██║███████║██║     ██║███████╗   ██║
██║███╗██║██╔══╝  ██╔══██║██║     ██║   ██╔══██║    ╚════██║██╔═══╝ ██╔══╝  ██║     ██║██╔══██║██║     ██║╚════██║   ██║
╚███╔███╔╝███████╗██║  ██║███████╗██║   ██║  ██║    ███████║██║     ███████╗╚██████╗██║██║  ██║███████╗██║███████║   ██║
 ╚══╝╚══╝ ╚══════╝╚═╝  ╚═╝╚══════╝╚═╝   ╚═╝  ╚═╝    ╚══════╝╚═╝     ╚══════╝ ╚═════╝╚═╝╚═╝  ╚═╝╚══════╝╚═╝╚══════╝   ╚═╝

        ");
            Console.WriteLine(Title);
        }
    }
}