using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public class UserService
    {
        public List<User> users = new List<User>();

        public UserService(Bank_Applikation bank_Applikation)
        {
            users = bank_Applikation._UserRegistry;
        }

        public User Loggin()
        {
            int attempt = 1;
            while (attempt >= 3)
            {
                Console.WriteLine("User Name: ");
                string username = Console.ReadLine();
                Console.WriteLine("Password: ");
                string password = Console.ReadLine();
                foreach (User user in users)
                {
                    if (user._userName == username && user._passWord == password)
                    {
                        return user;
                    }
                }
                Console.WriteLine("Fel lösenord eller användar namn vänligen försök igen. kontot kommer bli låst efter 3 fel försök.");
                attempt++;
            }
              return null;
        }
    }
}