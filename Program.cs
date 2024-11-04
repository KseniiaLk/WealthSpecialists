using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WealthSpecialists
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Bank_Applikation applikation = new Bank_Applikation();
                applikation._user=Loggin(applikation);
               
               



            }

        }
        static User Loggin(Bank_Applikation app)
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
            





        
    }
}
