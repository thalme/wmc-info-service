using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsMediaCenterInfoClient;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            LCDSmartie lcdSmartie = new LCDSmartie();

            Console.WriteLine(lcdSmartie.function20("calc.exe", ""));
            Console.WriteLine(lcdSmartie.function1("", ""));
            Console.WriteLine(lcdSmartie.function2("", ""));


            Console.ReadLine();
        }
    }
}
