using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public class Bank_Application
    {
        public List<User> _UserRegistry = new List<User> { new Manager("Raidar", "Bääst"), new Customer("Erik", "password") };

        //new test stuff timers
        public Timer _timer;
        public bool _timerRunning = false;
        //

        public double _sek = 1;
        public double _dollar = 11;
        public double _euro = 12;
        public int _totalAccounts = 100;

        Queue<Task> test = new Queue<Task>(); //tasks go in here
        
        public async void  taskstarter(Queue<Task> queue)
        {
           
            // if we for some reason end up here in this method again before taskstarter completes its tasks it will abort and restart timer so we dont have multiple taskstarters running
            // async await should fix this since it waits for task to be completed before continueing and relooping (not sure)
            if (_timerRunning) return;

            _timerRunning = true;
            while (queue.Count > 0)
            {
                
                Task task = queue.Dequeue();
                task.Start();
                await task; //tasks are not gauranteed to run in order, by using await we make sure to start a task , wait untill its finished then rerun the whileloop and start the next task
            }
            Console.WriteLine("All tasks completed.");
            _timerRunning = false;
        }

        public void TimerCallback(object state)
        {
            taskstarter(test);  // Call the taskstarter method when the timer elapses
        }
        public void menu()
        {
            _timer = new Timer(TimerCallback, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5)); // first time we start the meny we start timer starts goes to 1 minute then 5
            int limit = 0;
            while (limit < 4)
            {
                Console.WriteLine("[1]log in to account");
                User user = Login();
                if (user == null)
                {
                    limit++;
                    Console.WriteLine($"Invalid username or password, you have: {3-limit} attempts left .");
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
                        Customer_Menu((Customer)user);
                        break;

                    case Manager:
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
                Console.WriteLine("[1] Create new Customer");
                Console.WriteLine("[2] Update currency");
                Console.WriteLine("[3] Log out");
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
                Console.Clear();
                Console.WriteLine("Du är inloggad på : " + customer._userName);
                Console.WriteLine("[1] Account overview");
                Console.WriteLine("[2] Money transfer");
                Console.WriteLine("[3] Create a new account");
                Console.WriteLine("[4] Apply for loan");
                Console.WriteLine("[5] Log out");
                int.TryParse(Console.ReadLine(), out int input);

                switch (input)
                {
                    case 1:
                        customer.View_acc();
                        Console.ReadLine();
                        break;

                    case 2:               
                        customer.Transfer(this);
                        break;

                    case 3:
                        Console.WriteLine("Select account type to create");
                        Console.WriteLine("[1] Savings Account");
                        Console.WriteLine("[2] ForeingCurrency Account");
                        Console.WriteLine("[3] Return to Previous menu");
                        int.TryParse(Console.ReadLine(), out int accountType);

                        switch(accountType)
                        {
                            case 1:
                                Task task = new Task(() =>
                                {  
                                    customer.Create_account(1000, "SEK", this);
                                 });

                                test.Enqueue(task); //testing enqueue using create account
                                Console.WriteLine("A new account has been created.");
                               // customer.View_acc();
                                break;
                            case 2:
                                Console.WriteLine("Choose what currency you want to create the account in");
                                Console.WriteLine("[1] $");
                                Console.WriteLine("[2] €");
                                Console.WriteLine("[3] Return to previous menu");
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
                        this.Request_loan(customer.Select_account("Choose account"));
                        break;

                    case 5:
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
            }
            else
            {
                Console.WriteLine("your loan has been denied");
            }
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
            if (account._currencyType == "Dollar")
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