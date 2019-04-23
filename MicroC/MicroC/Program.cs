using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MicroC
{
    class Program
    {
        static void Main(string[] args)
        {


            var programText = File.ReadAllText("prog.txt");

            List<string> tokens = new List<string>();


            int fromIndex = 0;

            var parser = new Parser();

            do
            {
                var token = parser.getNextToken(programText, ref fromIndex);
                tokens.Add(token);

               

                Console.WriteLine(token + "   " + parser.getSymbolType(token, new List<string>()).ToString());
            }
            while (fromIndex < programText.Length);
            Interpreter interpreter = new Interpreter(programText);

            interpreter.Interpret();

            Console.WriteLine("Hello World!");
        }
    }
}
