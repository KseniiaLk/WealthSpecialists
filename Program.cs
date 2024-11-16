using System;

namespace WealthSpecialists
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Bank_Application applikation = new Bank_Application();

                applikation.menu();
            }
        }
    }
}