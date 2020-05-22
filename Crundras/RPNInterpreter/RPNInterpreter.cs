using Crundras.Common;
using Crundras.Common.Tables;
using RPNTranslator;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace RPNInterpreter
{
    public class RPNInterpreter
    {
        public void Interpret(TablesCollection tables, LinkedList<RPNToken> tokens)
        {
            var stack = new Stack<RPNToken>();
            var tokensArray = tokens.ToImmutableArray();

            for (int i = 0; i < tokensArray.Length; i++)
            {
                RPNToken token = tokensArray[i];
                if (Token.IsIdentifierOrLiteral(token.LexemeCode)
                    || Token.IsLabel(token.LexemeCode))
                {
                    stack.Push(token);
                }
                else
                {
                    if (token.LexemeCode == LexemesTable.GetLexemeId("@"))
                    {
                        Console.Write("Output: ");
                        var rpnToken = stack.Pop();
                        if (!rpnToken.Id.HasValue)
                        {
                            throw new Exception("");
                        }

                        if (Token.IsIdentifier(rpnToken.LexemeCode))
                        {
                            Console.WriteLine(tables.IdentifiersTable[rpnToken.Id.Value].Value);
                        }
                        else if (Token.IsIntLiteral(rpnToken.LexemeCode))
                        {
                            Console.WriteLine(tables.IntLiteralsTable[rpnToken.Id.Value]);
                        }
                        else if (Token.IsFloatLiteral(rpnToken.LexemeCode))
                        {
                            Console.WriteLine(tables.FloatLiteralsTable[rpnToken.Id.Value]);
                        }
                    }
                    else if (token.LexemeCode == LexemesTable.GetLexemeId("$"))
                    {
                        Console.Write("Input: ");
                        var identToken = stack.Pop();
                        if (!identToken.Id.HasValue || !Token.IsIdentifier(identToken.LexemeCode))
                        {
                            throw new Exception("");
                        }

                        var inputLine = Console.ReadLine();

                        if (Token.IsFloatLiteral(tables.IdentifiersTable[identToken.Id.Value].Type)
                            && !float.TryParse(inputLine, out _))
                        {
                            throw new Exception("");
                        }
                        if (Token.IsIntLiteral(tables.IdentifiersTable[identToken.Id.Value].Type)
                                 && !int.TryParse(inputLine, out _))
                        {
                            throw new Exception("");
                        }
                        tables.IdentifiersTable[identToken.Id.Value].Value = inputLine;
                        stack.Push(identToken);
                    }
                    else if (token.LexemeCode == LexemesTable.GetLexemeId("goto"))
                    {
                        var labelToken = stack.Pop();
                        if (!labelToken.Id.HasValue || !Token.IsLabel(labelToken.LexemeCode))
                        {
                            throw new Exception($"Expected label but got '{LexemesTable.GetLexemeName(labelToken.LexemeCode)}'");
                        }

                        i = (int)tables.LabelsTable[labelToken.Id.Value].Position;
                    }
                    else if (token.LexemeCode == LexemesTable.GetLexemeId("if"))
                    {
                        var valueToken = stack.Pop();
                        if (!valueToken.Id.HasValue ||
                            (!Token.IsLiteral(valueToken.LexemeCode) && !Token.IsIdentifier(valueToken.LexemeCode)))
                        {
                            throw new Exception($"Expected literal or identifier but got '{LexemesTable.GetLexemeName(valueToken.LexemeCode)}'");
                        }

                        var labelToken = stack.Pop();
                        if (!labelToken.Id.HasValue || !Token.IsLabel(labelToken.LexemeCode))
                        {
                            throw new Exception($"Expected label but got '{LexemesTable.GetLexemeName(labelToken.LexemeCode)}'");
                        }

                        bool value = false;
                        if (Token.IsIntLiteral(valueToken.LexemeCode))
                        {
                            value = tables.IntLiteralsTable[valueToken.Id.Value] == 0;
                        }
                        else if (Token.IsFloatLiteral(valueToken.LexemeCode))
                        {
                            value = Math.Abs(tables.FloatLiteralsTable[valueToken.Id.Value]) < double.Epsilon;
                        }
                        else if (Token.IsIdentifier(valueToken.LexemeCode))
                        {
                            value = Math.Abs(double.Parse(tables.IdentifiersTable[valueToken.Id.Value].Value)) < double.Epsilon;
                        }
                        else
                        {
                            throw new Exception("???");
                        }

                        if (value)
                        {
                            i = (int)tables.LabelsTable[labelToken.Id.Value].Position;
                        }
                    }
                    else if (token.Name == "NEG")
                    {
                        var rpnToken = stack.Pop();
                        if (!rpnToken.Id.HasValue)
                        {
                            throw new Exception("");
                        }

                        if (Token.IsIdentifier(rpnToken.LexemeCode))
                        {
                            var value = tables.IdentifiersTable[rpnToken.Id.Value].Value;
                            if (value.StartsWith('-'))
                            {
                                value.TrimStart('-');
                            }
                            else value = '-' + value;

                            tables.IdentifiersTable[rpnToken.Id.Value].Value = value;
                        }
                        else if (Token.IsIntLiteral(rpnToken.LexemeCode))
                        {
                            var value = -tables.IntLiteralsTable[rpnToken.Id.Value];
                            rpnToken.Id = tables.IntLiteralsTable.GetId(value);
                        }
                        else if (Token.IsFloatLiteral(rpnToken.LexemeCode))
                        {
                            var value = -tables.FloatLiteralsTable[rpnToken.Id.Value];
                            rpnToken.Id = tables.FloatLiteralsTable.GetId(value);
                        }

                        stack.Push(rpnToken);
                    }
                    else if (token.Name == "=")
                    {
                        var identToken = stack.Pop();
                        if (!Token.IsIdentifier(identToken.LexemeCode))
                        {
                            throw new Exception("");
                        }
                        if (!identToken.Id.HasValue)
                        {
                            throw new Exception("");
                        }

                        var type = tables.IdentifiersTable[identToken.Id.Value].Type;
                        if (type == 0)
                        {
                            throw new Exception($"Undefined variable: '{tables.IdentifiersTable[identToken.Id.Value].Name}'.");
                        }

                        var valueToken = stack.Pop();

                        string value;
                        if (Token.IsIntLiteral(valueToken.LexemeCode))
                        {
                            value = tables.IntLiteralsTable[valueToken.Id.Value].ToString();
                        }
                        else if (Token.IsFloatLiteral(valueToken.LexemeCode))
                        {
                            value = tables.FloatLiteralsTable[valueToken.Id.Value].ToString();
                        }
                        else if (Token.IsIdentifier(valueToken.LexemeCode))
                        {
                            value = tables.IdentifiersTable[valueToken.Id.Value].Value;
                        }
                        else
                        {
                            throw new Exception("");
                        }

                        tables.IdentifiersTable[identToken.Id.Value].Value = value;
                        stack.Push(identToken);
                    }
                    else if (stack.Count >= 2)
                    {
                        var a = stack.Pop();
                        var b = stack.Pop();

                        if (!a.Id.HasValue || !b.Id.HasValue)
                        {
                            throw new Exception("");
                        }

                        double firstValue = 0;
                        double secondValue = 0;
                        uint firstType = 0;
                        uint secondType = 0;

                        if (Token.IsIdentifier(a.LexemeCode))
                        {
                            firstValue = double.Parse(tables.IdentifiersTable[a.Id.Value].Value);
                            firstType = tables.IdentifiersTable[a.Id.Value].Type;
                        }
                        else if (Token.IsIntLiteral(a.LexemeCode))
                        {
                            firstValue = tables.IntLiteralsTable[a.Id.Value];
                            firstType = a.LexemeCode;
                        }
                        else if (Token.IsFloatLiteral(a.LexemeCode))
                        {
                            firstValue = tables.FloatLiteralsTable[a.Id.Value];
                            firstType = a.LexemeCode;
                        }

                        if (Token.IsIdentifier(b.LexemeCode))
                        {
                            secondValue = double.Parse(tables.IdentifiersTable[b.Id.Value].Value);
                            secondType = tables.IdentifiersTable[b.Id.Value].Type;
                        }
                        else if (Token.IsIntLiteral(b.LexemeCode))
                        {
                            secondValue = tables.IntLiteralsTable[b.Id.Value];
                            secondType = b.LexemeCode;
                        }
                        else if (Token.IsFloatLiteral(b.LexemeCode))
                        {
                            secondValue = tables.FloatLiteralsTable[b.Id.Value];
                            secondType = b.LexemeCode;
                        }

                        if (firstType == 0 || secondType == 0)
                        {
                            throw new Exception("");
                        }

                        double resultValue = 0;
                        switch (token.LexemeCode)
                        {
                            case 17:  // +
                                resultValue = secondValue + firstValue;
                                break;
                            case 18:  // -
                                resultValue = secondValue - firstValue;
                                break;
                            case 19:  // *
                                resultValue = secondValue * firstValue;
                                break;
                            case 20:  // **
                                resultValue = Math.Pow(secondValue, firstValue);
                                break;
                            case 21:  // /
                                resultValue = secondValue / firstValue;
                                break;
                            case 22:  // %
                                resultValue = secondValue % firstValue;
                                break;
                            case 23:  // <
                                resultValue = secondValue < firstValue ? 1 : 0;
                                break;
                            case 24:  // >
                                resultValue = secondValue > firstValue ? 1 : 0;
                                break;
                            case 25:  // <=
                                resultValue = secondValue <= firstValue ? 1 : 0;
                                break;
                            case 26:  // ==
                                resultValue = Math.Abs(firstValue - secondValue) < double.Epsilon ? 1 : 0;
                                break;
                            case 27:  // >=
                                resultValue = secondValue >= firstValue ? 1 : 0;
                                break;
                            case 28:  // !=
                                resultValue = Math.Abs(firstValue - secondValue) > double.Epsilon ? 1 : 0;
                                break;
                        }

                        uint resultType = Math.Max(firstType, secondType);

                        uint id;
                        if (Token.IsIntLiteral(resultType))
                        {
                            id = tables.IntLiteralsTable.GetId((int)resultValue);
                        }
                        else
                        {
                            id = tables.FloatLiteralsTable.GetId((float)resultValue);
                        }

                        stack.Push(new RPNToken(resultType, id));
                    }
                }
            }
        }
    }
}