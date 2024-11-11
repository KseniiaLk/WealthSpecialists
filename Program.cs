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
                Bank_Application applikation = new Bank_Application();
                applikation.menu();
            }
        }
    }
}