using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public class UserService
    {
        List<User> users = new List<User>();
        public UserService(Bank_Applikation bank_Applikation) 
        {
            users = bank_Applikation._UserRegistry;
        }
    }
}
