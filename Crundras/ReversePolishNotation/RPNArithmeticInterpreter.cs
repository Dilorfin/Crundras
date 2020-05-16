using Crundras.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversePolishNotation
{
    public class RPNArithmeticInterpreter
    {
        private uint GetLiteralId(TokenTable tokenTable, string value)
        {
            if (!tokenTable.LiteralsTable.ContainsValue(value))
            {
                tokenTable.LiteralsTable.Add((uint)(tokenTable.LiteralsTable.Count + 1), value);
            }
            return tokenTable.LiteralsTable.First(pair => pair.Value == value).Key;
        }

        public void Interpret(TokenTable tokenTable, LinkedList<RPNToken> tokens)
        {
            var stack = new Stack<RPNToken>();
            foreach (var token in tokens)
            {
                if (Token.IsLiteral(token.LexemeCode))
                {
                    stack.Push(token);
                }
                else
                {
                    if (token.LexemeCode == TokenTable.GetLexemeId("@"))
                    {
                        Console.WriteLine(tokenTable.LiteralsTable[stack.Pop().Id.Value]);
                    }
                    else if(token.Name == "NEG")
                    {
                        var valueToken = stack.Pop();
                        var value = tokenTable.LiteralsTable[valueToken.Id.Value];
                        if (value.StartsWith('-'))
                        {
                            value.TrimStart('-');
                        }
                        else value = '-' + value;

                        valueToken.Id = GetLiteralId(tokenTable, value);
                        stack.Push(valueToken);
                    }
                    else
                    {
                        var a = stack.Pop();
                        var b = stack.Pop();
                        var firstValue = double.Parse(tokenTable.LiteralsTable[b.Id.Value]);
                        var secondValue = double.Parse(tokenTable.LiteralsTable[a.Id.Value]);
                        double resultValue = 0;
                        switch (token.LexemeCode)
                        {
                            case 15:  // +
                                resultValue = firstValue + secondValue;
                                break;
                            case 16:  // -
                                resultValue = firstValue - secondValue;
                                break;
                            case 17:  // *
                                resultValue = firstValue * secondValue;
                                break;
                            case 18:  // **
                                resultValue = Math.Pow(firstValue, secondValue);
                                break;
                            case 19:  // /
                                resultValue = firstValue / secondValue;
                                break;
                            case 20:  // %
                                resultValue = firstValue % secondValue;
                                break;
                            case 21:  // <
                                resultValue = firstValue < secondValue ? 1 : 0;
                                break;
                            case 22:  // >
                                resultValue = firstValue > secondValue ? 1 : 0;
                                break;
                            case 23:  // <=
                                resultValue = firstValue <= secondValue ? 1 : 0;
                                break;
                            case 24:  // ==
                                resultValue = Math.Abs(firstValue - secondValue) < double.Epsilon ? 1 : 0;
                                break;
                            case 25:  // >=
                                resultValue = firstValue >= secondValue ? 1 : 0;
                                break;
                            case 26:  // !=
                                resultValue = Math.Abs(firstValue - secondValue) > double.Epsilon ? 1 : 0;
                                break;   
                        }
                        var result = new RPNToken(Math.Max(a.LexemeCode, b.LexemeCode), GetLiteralId(tokenTable, resultValue.ToString()));
                        stack.Push(result);
                    }
                }
            }
        }
    }
}