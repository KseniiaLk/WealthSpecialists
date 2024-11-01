using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    public class user
    {
    }

    public class Costumer : user
    {
        //list containing accounts user has, using abstract class account as <Type>
        List<Account> _accounts = new List<Account>();
        
        public Costumer (string userName, string passWord, Guid guid) : base (userName, passWord, guid)
        {

        }
    }

    public class Manager : user
    {
        public Manager (string userName, string passWord, Guid guid) : base (userName, passWord, guid)
        {


        }
    }

}
