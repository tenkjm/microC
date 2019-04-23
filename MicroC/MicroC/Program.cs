using System;
using System.IO;

namespace MicroC
{
    class Program
    {
        static void Main(string[] args)
        {


            var programText = File.ReadAllText("main.c");






            Console.WriteLine("Hello World!");
        }
    }
}
