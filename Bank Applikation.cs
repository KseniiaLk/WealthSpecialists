﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public class Bank_Application
    {
        public List<User> _UserRegistry = new List<User> { new Manager("Raidar", "Bääst"), new Customer("Erik", "password") };

        public double _sek = 1;
        public double _dollar = 11;
        public double _euro = 12;

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
                switch (user)
                {
                    case Customer:
                        Customer_Menu((Customer)user);
                        break;

                    case Manager:
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
                        break;

                    case 2:
                        customer.Transfer(this);
                        break;

                    case 3:
                        Account newAccount = new SavingsAccount(1000, "SEK");
                        customer._accounts.Add(newAccount);
                        Console.WriteLine("A new account has been created.");
                        customer.View_acc();

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