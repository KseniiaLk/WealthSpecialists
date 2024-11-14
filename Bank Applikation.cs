using System;
using System.Collections.Generic;
using System.Data;
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
            _timer = new Timer(TimerCallback, null, TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(15)); // first time we start the meny we start timer starts goes to 1 minute then 5
            int limit = 0;
            while (limit < 4)
            {
                Console.WriteLine("═══════════════════════════════");
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
                        Console.Clear();
                        Title();
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
                Center_text("═══════════════════════════════");
                Center_text("Du är inloggad på : " + customer._userName);
                Center_text("═══════════════════════════════");
                Center_text("[1]➤ Account overview");
                Center_text("═══════════════════════════════");
                Center_text("[2]➤View Account History");
                Center_text("═══════════════════════════════");
                Center_text("[3]➤ Money transfer");
                Center_text("═══════════════════════════════");
                Center_text("[4]➤ Create a new account");
                Center_text("═══════════════════════════════");
                Center_text("[5]➤ Apply for loan");
                Center_text("═══════════════════════════════");
                Center_text("[6]➤ Clear screen");
                Center_text("═══════════════════════════════");
                Center_text("[7]➤ Log out");
                Center_text("═══════════════════════════════");
                int.TryParse(Console.ReadLine(), out int input);

                switch (input)
                {
                    case 1:
                        Console.Clear();
                        Title();
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("Would you like to see a detailed overview");
                        Console.WriteLine("Press the number corresponding to the account.");
                        Console.WriteLine("═══════════════════════════════");
                        customer.View_acc();
                        Console.WriteLine("Press enter to return");
                        int.TryParse(Console.ReadLine(), out int inputcust);
                        Console.WriteLine("═══════════════════════════════");
                        if (inputcust == 0)
                            break;
                        try
                        {
                            Console.Clear();
                            Title();
                            customer.View_detailed(customer.customer_accounts[inputcust - 1]);
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }
                        Console.WriteLine("Press enter to return to menu");
                        Console.ReadLine();
                        Console.Clear();
                        Title();
                        break;
                    case 2:
                        Console.Clear();
                        Title();
                        customer.View_acc();
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("Would you like to see account history");
                        Console.WriteLine("Press the number corresponding to the account.");
                        Console.WriteLine("═══════════════════════════════");
                        Console.WriteLine("Press enter to return");
                        int.TryParse(Console.ReadLine(), out int inputhis);
                        Console.WriteLine("═══════════════════════════════");
                        if (inputhis == 0)
                            break;
                        try
                        {
                            Console.Clear();
                            Title();
                            customer.veiw_accountHitory(customer.customer_accounts[inputhis - 1]);
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }
                        Console.WriteLine("Press enter to return to menu");
                        Console.ReadLine();
                        Console.Clear();
                        Title();
                        break;

                    case 3:
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

                    case 4:
                        Console.Clear();
                        Title();
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
                                Console.Clear();
                                Title();
                                Console.WriteLine("═══════════════════════════════");
                                customer.Create_account(1000, "SEK", this);
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("A new Savings account has been created.");
                                customer.View_acc();
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("Press enter to return to menu");
                                Console.WriteLine("═══════════════════════════════");
                                Console.ReadLine();
                                Console.Clear();
                                Title();
                                break;

                            case 2:
                                Console.Clear();
                                Title();
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("Choose what currency you want to create the account in");
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("[1]➤ $ USD");
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("[2]➤ € EUR");
                                Console.WriteLine("═══════════════════════════════");
                                Console.WriteLine("[3]➤ Return to previous menu");
                                Console.WriteLine("═══════════════════════════════");
                                int.TryParse(Console.ReadLine(), out int currency);

                                switch (currency)
                                {
                                    case 1:
                                        Console.Clear();
                                        Title();
                                        customer.Create_Currencyaccount(1000, "USD", this);
                                        Console.WriteLine("A new account with the currency [USD] has been created.");
                                        customer.View_acc();
                                        Console.WriteLine("═══════════════════════════════");
                                        Console.WriteLine("Press enter to return to menu");
                                        Console.WriteLine("═══════════════════════════════");
                                        Console.ReadLine();
                                        Console.Clear();
                                        Title();
                                        break;

                                    case 2:
                                        Console.Clear();
                                        Title();
                                        customer.Create_Currencyaccount(1000, "EUR", this);
                                        Console.WriteLine("A new account with the currency [EUR] has been created.");
                                        customer.View_acc();
                                        Console.WriteLine("═══════════════════════════════");
                                        Console.WriteLine("Press enter to return to menu");
                                        Console.WriteLine("═══════════════════════════════");
                                        Console.ReadLine();
                                        Console.Clear();
                                        Title();
                                        break;

                                    case 3:
                                        Console.Clear();
                                        Title();
                                        break;
                                }
                                break;

                            case 3:
                                break;
                        }
                        break;

                    case 5:
                        Console.Clear();
                        Title();
                        this.Request_loan(customer.Select_account("Choose account"));
                        Console.WriteLine("Press enter to return");
                        int.TryParse(Console.ReadLine(), out int inputreturn);
                        if (inputreturn == 0)
                            break;
                        try
                        {
                            customer.View_detailed(customer.customer_accounts[inputreturn - 1]);
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }
                        break;
                        

                    case 6:
                        Console.Clear();
                        Title();
                        break;

                    case 7:
                        return;

                    default:
                        Console.WriteLine("Invalid selection please try again.");
                        break;
                }
            }
        }

        public void Request_loan(Account account)
        {
            Console.WriteLine("═══════════════════════════════");
            Console.WriteLine($"You can loan a maximum amount of {account._accountBalance * 5} at an interest rate of {account._interestRate} % \nhow much would you like to loan?: ");
            Console.WriteLine("═══════════════════════════════");
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

        public double CurrencyConverter(Account account, double amount, string currencyIn)

        {
            if (currencyIn == "SEK")
            { 
                if (account._currencyType == "USD")
                {
                    double output = _dollar / amount;
                    return output;
                }
                else if (account._currencyType == "EUR")
                {
                    double output = _euro / amount;
                    return output;
                }
            Console.WriteLine("Conversion failed");
            return 0;
            }
            else if (currencyIn == "USD")
            {
                if (account._currencyType == "SEK")
                {
                    double output = _dollar * amount;
                    return output;
                }
                else if (account._currencyType == "EUR")
                {
                    double output = amount / _euro;
                    return output;
                }
                Console.WriteLine("Conversion failed");
                return 0;
            }
            else if (currencyIn == "EUR")
            {
                if (account._currencyType == "SEK")
                {
                    double output = _euro * amount;
                    return output;
                }
                else if (account._currencyType == "USD")
                {
                    double output = amount / _dollar;
                    return output;
                }
                Console.WriteLine("Conversion failed");
                return 0;
            }
            Console.WriteLine("Conversion failed");
            return 0;
        }
        static void Center_text(string text)
        {
            int consoleWidth = Console.WindowWidth;
            int leftPadding = (consoleWidth - text.Length) / 2;
            Console.WriteLine(text.PadLeft(leftPadding + text.Length));
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