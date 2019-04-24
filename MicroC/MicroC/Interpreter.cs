using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroC
{
    public class Interpreter
    {

        Dictionary<string, int> symbolsTable;
        private List<string> symbolList;
        private Parser _parser;
        private string _text;
        int _progIndex = 0;

        public Interpreter(string text)
        {      
            _parser = new Parser();
            _text = text;
            symbolsTable = GetSymbolTable(text, ref _progIndex);
            symbolList = symbolsTable.Keys.ToList();
        }


        public void Interpret()
        {
            EvaluateBlock(_text, ref _progIndex);
        }


        public Dictionary<string, int> GetSymbolTable(string text, ref int  fromIndex)
        {
            var symbolsTable=new Dictionary<string, int>();

            if (_parser.getNextToken(text, ref fromIndex).Equals("var"))
            {
                var vars = text.Substring(fromIndex, text.IndexOf(';') - fromIndex  );
                var variables = vars.Split(',');

            
                foreach (var variable in variables)
                {
                    symbolsTable.Add(variable.Trim(), 0);
                }

                fromIndex = text.IndexOf(';') +1;
            }

            return symbolsTable;
        }



        void EvaluateBlock(string text, ref int  startIndex)
        {
            var token_type = _parser.getSymbolType(_parser.getNextToken(text, ref startIndex), symbolList);
            if ( token_type==
                TokenType.BlockBegin)
            {

                var token = _parser.getNextToken(text, ref startIndex);
                var symbol_type = _parser.getSymbolType(token, symbolList);

                while ( symbol_type != TokenType.BlockEnd)
                {             
                    switch (symbol_type)
                    {
                        case TokenType.TableSymbol:
                             _parser.getNextToken(text, ref startIndex);
                            symbolsTable[token] = EvaluateMathExpression(text, ref startIndex);
                            _parser.getNextToken(text, ref startIndex);
                            break;
                        case TokenType.Semicolon:
                            break;
                        case TokenType.Print:
                            EvaluatePrint(text, ref startIndex);
                            break;
                        case TokenType.If:
                            EvaluateIf(text, ref startIndex);
                            break;

                        case TokenType.While:
                            EvaluateWhile(text, ref startIndex);
                            break;
                    }

                    token = _parser.getNextToken(text, ref startIndex);
                    symbol_type =  _parser.getSymbolType(token, symbolList);
                }
            }
        }

        void EvaluateIf(string text, ref int startIndex)
        {
            var exprResult = EvaluateBoolExpression(text, ref startIndex);

            if (exprResult)
            {
                EvaluateBlock(text, ref startIndex);
            }
            else
            {
                int openCount = 0;
                int closeCount = 0;           
                do
                {
                   var token = _parser.getNextToken(text, ref startIndex);
                    var token_type = _parser.getSymbolType(token, symbolList);

                    if (token_type == TokenType.BlockEnd)
                    {
                        closeCount++;
                    }

                    if (token_type == TokenType.BlockBegin)
                    {
                        openCount++;
                    }

                } while (openCount != closeCount);
            }
        }



        void EvaluateWhile(string text, ref int startIndex)
        {
            int startIndexThis = startIndex;
            metka:
            startIndex = startIndexThis;
            var exprResult = EvaluateBoolExpression(text, ref startIndex);
           
            if (exprResult)
            {
                EvaluateBlock(text, ref startIndex);
                goto metka;

            }
            else
            {
                int openCount = 0;
                int closeCount = 0;

                do
                {
                    var token = _parser.getNextToken(text, ref startIndex);
                    var token_type = _parser.getSymbolType(token, symbolList);

                    if (token_type == TokenType.BlockEnd)
                    {
                        closeCount++;
                    }

                    if (token_type == TokenType.BlockBegin)
                    {
                        openCount++;
                    }

                } while (openCount != closeCount);
            }
        }

        int getOperandValue(string text, ref int startIndex)
        {
            var token = _parser.getNextToken(text, ref startIndex);

            switch (_parser.getSymbolType(token, this.symbolList))
            {
                case TokenType.Number:
                    return int.Parse(token);
                case TokenType.TableSymbol:
                    return symbolsTable[token];
            }

            return 0;
        }


        int getMultiplyOperand(string text, ref int startIndex)
        {
        
            int a, b;

            a = getOperandValue(text, ref startIndex);

            var token = _parser.getNextToken(text, ref startIndex);
            var token_type = _parser.getSymbolType(token, this.symbolList);
            switch (token_type)
            {
                case TokenType.Plus:
                    startIndex--;
                    return a;
                case TokenType.Minus:
                    startIndex--;
                    return a;
                case TokenType.Semicolon:
                    startIndex--;
                    return a;
                case TokenType.Multiply:
                    b = getOperandValue(text, ref startIndex);
                    a = a * b;
                    break;
                case TokenType.Divide:
                    b = getOperandValue(text, ref startIndex);
                    a = a / b;
                    break;
            }

            return a;
        }

        int EvaluateMathExpression(string text, ref int startIndex)
        {
            int a, b, c;

            a = getOperandValue(text, ref startIndex);

            while (true)
            {
                var token = _parser.getNextToken(text, ref startIndex);
                var token_type = _parser.getSymbolType(token, this.symbolList);
               

                switch (token_type)
                {
                    case TokenType.Semicolon:
                        startIndex--;
                        return a;
                    case TokenType.BlockBegin:
                    case TokenType.Less:
                    case TokenType.Grater:
                        startIndex--;
                        return a;
                    case TokenType.Multiply:
                        b = getOperandValue(text, ref startIndex);
                        a = a * b;
                        break;
                    case TokenType.Divide:
                        b = getOperandValue(text, ref startIndex);
                        a = a / b;
                        break;


                    case TokenType.Plus:
                        b = getMultiplyOperand(text, ref startIndex);
                        token = _parser.getNextToken(text, ref startIndex);
                        token_type = _parser.getSymbolType(token, this.symbolList);
                        if (token_type == TokenType.Semicolon)
                        {
                            startIndex--;
                            return   a + b;
                        }

                        break;
                    case TokenType.Minus:
                        b = getMultiplyOperand(text, ref startIndex);
                        token = _parser.getNextToken(text, ref startIndex);
                        token_type = _parser.getSymbolType(token, this.symbolList);
                        if (token_type == TokenType.Semicolon)
                        {
                            startIndex--;
                            return a - b;
                        }
                        
                        break;

                }
            }
            return 0;
        }

        bool EvaluateBoolExpression(string text, ref int startIndex)
        {
            var operand1 = EvaluateMathExpression(text, ref startIndex);
            var token = _parser.getNextToken(text, ref startIndex);
            var tokenType = _parser.getSymbolType(token, symbolList);
            var boolOperator = tokenType;
            var operand2 = EvaluateMathExpression(text, ref startIndex);

            switch (boolOperator)
            {
                case TokenType.Equal:
                    return operand2 == operand1;
                case TokenType.Grater:
                    return operand1 > operand2;
                case TokenType.Less:
                    return operand1 < operand2;
            }

            return false;
        }

        void EvaluatePrint(string text, ref int startIndex)
        {         
            do
            {
                Console.Out.Write(EvaluateMathExpression(text, ref startIndex));
                var token = _parser.getNextToken(text, ref startIndex);
                var tokenType =   _parser.getSymbolType(token, symbolList);
                startIndex--;
                if (tokenType == TokenType.Semicolon)
                {
                    break;
                }
               
            } while (true);

            Console.Out.Write("\n");
        }
    }
}
