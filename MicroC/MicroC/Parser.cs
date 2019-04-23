using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroC
{
    public class Parser
    {

        public List<string> delimiters;


        public Parser()
        {
            delimiters = new List<string>()
            {
                "+",
                "-" ,
                "*" ,
                "/" ,
                "{",
                "}",
                "=",
                ">",
                "<",
                ";",
                

            };
        }

        /// <summary>
        /// Gets the next token.
        /// </summary>
        /// <returns>The next token.</returns>
        /// <param name="text">Text.</param>
        /// <param name="fromIndex">From index.</param>
        public string getNextToken(string text,  ref int fromIndex)
        {

            var currentSymbol = text[fromIndex];
            while (char.IsWhiteSpace(currentSymbol))
            {
                fromIndex++;
                currentSymbol = text[fromIndex];
            }
            if(delimiters.Contains(currentSymbol.ToString()))
            {
                fromIndex++;
                return currentSymbol.ToString();
            }

            int startIndex = fromIndex;

            while ((!char.IsWhiteSpace(currentSymbol)))
            {
                fromIndex++;

                currentSymbol = text[fromIndex];
                if( delimiters.Contains(currentSymbol.ToString()))
                {
                    fromIndex -= 1;
                    return text.Substring(startIndex, (fromIndex++) - startIndex + 1);
                    break;
                 
                }
            }

            
            return text.Substring(startIndex, (fromIndex ++)- startIndex);
        }



        public TokenType? getSymbolType(string text, List<string> symbols)
        {

            int k;

            if (int.TryParse(text, out k))
            {
                

                    return TokenType.Number;
            }

            if (text == "+")
            {
                return TokenType.Plus;
            }

            if (text == "-")
            {
                return TokenType.Minus;
            }

            if (text == "*")
            {
                return TokenType.Multiply;
            }

            if (text == "/")
            {
                return TokenType.Divide;
            }

            if (text == "if")
            { 
                return TokenType.If;
            }

            if (text == "while")
            {
                return TokenType.While;
            }

            if (text == "{")
            {
                return TokenType.BlockBegin;
            }

            if (text == "}")
            {
                return TokenType.BlockEnd;
            }

            if (text == "=")
            {
                return TokenType.Equal;
            }

            if (text=="print")
            {
                return TokenType.Print;
            }

            if (text == ">")
            {
                return TokenType.Grater;
            }

            if (text == "<")
            {
                return TokenType.Less;
            }
            if (text == ";")
            {
                return TokenType.Semicolon;
            }


            if (symbols.Any( s=>s.Equals(text)))
                {
                    return TokenType.TableSymbol;
                }

            return null;

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
        EqualtityCompare,
        Semicolon,
        While
    }

}
