﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Account> customer_accounts = new List<Account>();

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
                if (int.TryParse(Console.ReadLine(), out choise) && choise > 0 && choise < customer_accounts.Count)
                {
                    return customer_accounts[choise - 1];
                }
                else
                {
                    Console.WriteLine("Not a valid account number, try again");
                }
            }
        }

        public void Create_account(double balance, string currencyType, Bank_Application bank)
        {
            Account newAccount = new SavingsAccount(balance, currencyType, bank._totalAccounts);
            customer_accounts.Add(newAccount);
            bank._totalAccounts++;
        }

        public void Create_Currencyaccount(double balance, string currencyType, Bank_Application bank)
        {
            Account newAccount2 = new ForeingCurrency(balance, currencyType, bank._totalAccounts);
            customer_accounts.Add(newAccount2);
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
                        foreach (var account in customer.customer_accounts)
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
            Console.WriteLine("From which account would you like to transfer money from?");
            View_acc();
            int.TryParse(Console.ReadLine(), out int accountFrom);
            Account from = customer_accounts[accountFrom - 1];
            Console.WriteLine("how much money would you like to send?");
            int.TryParse(Console.ReadLine(), out int money);

            if (from._accountBalance < money)
            {
                Console.WriteLine("Your account does not have enough balance");
            }

            Console.WriteLine("What is the username of person you would like to transfer money to?");
            string transferTargetUser = Console.ReadLine();
            Console.WriteLine("what accountnumber would you like to send money to");
            //IEnumerable<User> readuser = bankapp._UserRegistry;
            //foreach (User item in readuser)
            //{
            //    if (transferTargetUser == item._userName)
            //            if(item is Customer customer)
            //            Console.WriteLine(customer.customer_accounts);

            //}
            int.TryParse(Console.ReadLine(), out int accounttarget);

            Task task = new Task(() =>
            {
                transferBetweenUsersLogic(bankapp, transferTargetUser, accounttarget, from, money);
            });
            bankapp.test.Enqueue(task);
        }

        public void View_acc()
        {
            int num = 1;
            foreach (Account item in customer_accounts)
            {
                if (item is SavingsAccount)
                    Console.WriteLine($"Account: {num} {item._accountNumber} Balance: {item._accountBalance:F} {item._currencyType}, Intrest {item._interestRate:F}%");
                else
                    Console.WriteLine($"Account: {num} {item._accountNumber} Balance: {item._accountBalance:F} {item._currencyType}");
                num++;
            }
        }

        public void View_detailed(Account account)

        {
            Console.WriteLine("═══════════════════════════════");
            Console.WriteLine($"Account Name: {account._accountNumber}\nCurrent balance: {account._accountBalance:F}\nCurrent Debt: {account._LoanAmount}\nCurrency Type: {account._currencyType} \nInterestrate: {account._interestRate}\nAccount ID {account._accountID}");
            Console.WriteLine("═══════════════════════════════");
        }

        public void Log_transaction(Account account, double transfer)
        {
            account._accounthistory.Add(new AccountHistory(account, transfer));
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
            if (customer_accounts[inputtwo - 1] is ForeingCurrency)
            {
                if (customer_accounts[inputtwo - 1]._currencyType == "USD")
                {
                    double output = inputthree / _bankApp._dollar;
                    customer_accounts[inputtwo - 1]._accountBalance += output;
                    customer_accounts[input - 1]._accountBalance -= inputthree;
                }
                else if (customer_accounts[inputtwo - 1]._currencyType == "EUR")
                {
                    double output = inputthree / _bankApp._euro;
                    customer_accounts[inputtwo - 1]._accountBalance += output;
                    customer_accounts[input - 1]._accountBalance -= inputthree;
                }
            }
            else if (inputthree <= customer_accounts[input - 1]._accountBalance)

            {
                customer_accounts[input - 1]._accountBalance -= inputthree;
                customer_accounts[inputtwo - 1]._accountBalance += inputthree;
            }
        }

        public void Transfer(Bank_Application _bankApp)
        {
            View_acc();
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
            if (inputthree > customer_accounts[input - 1]._accountBalance)
            {
                Console.WriteLine("Still not enough money");
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