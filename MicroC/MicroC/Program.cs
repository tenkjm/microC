using System;
using System.IO;

namespace MicroC
{
    class Program
    {
        static void Main(string[] args)
        {


            var programText = File.ReadAllText("prog.txt");


            int fromIndex = 0;

            var parser = new Parser();

            do
            {
                var token = parser.getNextToken(programText, ref fromIndex);
                Console.WriteLine(token);
            }
            while (fromIndex < programText.Length);


            Console.WriteLine("Hello World!");
        }
    }
}
