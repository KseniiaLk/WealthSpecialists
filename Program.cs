using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                Bank_Applikation applikation = new Bank_Applikation();
                applikation.menu();

                /* switch (access(applikation._user))
                 {
                     case 1:
                         Console.WriteLine("[1]KontoÖversikt");
                         Console.WriteLine("[2]Överföring av pengar");
                         Console.WriteLine("[3]Skapa nytt Konto");
                         Console.WriteLine("[4]Ansök om lån");
                         Console.WriteLine("[5]Logga ut");
                         int.TryParse(Console.ReadLine(), out int userInput);
                         //first meny we use userinput to access relevant
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
                 }*/
            }
        }

        private static User Loggin(Bank_Applikation app)
        {
            Console.WriteLine("User Name: ");
            string username = Console.ReadLine();
            Console.WriteLine("Password: ");
            string password = Console.ReadLine();
            foreach (User user in app._UserRegistry)
            {
                if (user._userName == username && user._passWord == password)
                {
                    return user;
                }
            }
            return null;
        }

        //        private static int? access(User user)
        //        {
        //            if (user is Customer)
        //            {
        //                return 1;
        //            }
        //            else if (user is Manager)
        //            {
        //                return 2;
        //            }
        //            else
        //                return null;
        //        }
    }
}