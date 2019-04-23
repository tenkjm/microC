using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroC
{
    public struct Parser
    {
        public TokenType? getNextSymbols(string text, int currentPos, List<string> symbols)
        {
            int start_symbol = currentPos++;
            char currentSymbol = text[currentPos];

            while (!char.IsSymbol(currentSymbol++))
            {

            }


            if (char.IsDigit(currentSymbol))
            {
                while (char.IsDigit(currentSymbol++))

                    return TokenType.Number;
            }

            if (currentSymbol == '+')
            {
                return TokenType.Plus;
            }

            if (currentSymbol == '-')
            {
                return TokenType.Minus;
            }

            if (currentSymbol == '*')
            {
                return TokenType.Multiply;
            }

            if (currentSymbol == '/')
            {
                return TokenType.Divide;
            }

            if (currentSymbol == 'i'
            && text[currentPos + 1] == 'f')
            {
                return TokenType.If;
            }

            if (currentSymbol == '{')
            {
                return TokenType.BlockEnd;
            }

            if (currentSymbol == '}')
            {
                return TokenType.BlockEnd;
            }

            if (currentSymbol == '=')
            {
                return TokenType.BlockEnd;
            }

            if (text.IndexOf("print", currentPos) == currentPos)
            {
                return TokenType.Print;
            }

            if (currentSymbol == '>')
            {
                return TokenType.Grater;
            }

            if (currentSymbol == '<')
            {
                return TokenType.Divide;
            }

            if (currentSymbol == '='
            && text[currentPos + 1] == 'f')
            {
                return TokenType.If;
            }

            if(symbols.Any( s=>text.IndexOf(s, currentPos) == currentPos))
                {
                    return TokenType.TableSymbol;
                }

            return null;

        }


        public string getSymbol(string text, ref int  index, ref int  value)
        {
            int start_symbol = index++;
            int endSymbol = -1;
            char currentSymbol = text[index];
            while(char.IsLetter(currentSymbol))
            {
                index++;
            }
           return (text.Substring(start_symbol + index - start_symbol));

        }


        public void ReadAllSymbols()
        {


        }


    }

    public enum TokenType
    {
        Number, //Число
        Plus,
        Minus,
        Multiply,
        Divide,
        If, //Оператор 
        BlockBegin, //Блок кода
        BlockEnd, //Блок кода
        TableSymbol, // Переменная
        Equal, //Знак равенства
        Print, //Печать в консооль
        Grater,
        Less,
        EqualtityCompare
    }

}
