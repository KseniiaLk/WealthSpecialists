﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthSpecialists
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Account konto = new Account(12000, "SEK");
            Console.WriteLine(konto._accountNumber);
        }
    }
}
