using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string expression = @"!((A&B)|C|D)";
            List<Token> tokens = new();
            StringReader reader = new StringReader(expression);

            Token t = null;
            do
            {
                t = new Token(reader);
                tokens.Add(t);
            } while (t.type != Token.TokenType.EXPR_END);

            List<Token> polishNotation = TransformToPolishNotation(tokens);
            var enumerator = polishNotation.GetEnumerator();
            enumerator.MoveNext();
            var proccessedExpresion = ProcessToExpression(ref enumerator);
        }

        static Expression ProcessToExpression(ref List<Token>.Enumerator notatedTokens)
        {
            if (notatedTokens.Current.type == Token.TokenType.LITERAL)
            {
                var expression = Expression.CreateBoolVar(notatedTokens.Current.value);
                notatedTokens.MoveNext();
                return expression;
            }
            else
            {
                if (notatedTokens.Current.value == "NOT")
                {
                    notatedTokens.MoveNext();
                    Expression expression = ProcessToExpression(ref notatedTokens);
                    return Expression.CreateNot(expression);
                }
                else if (notatedTokens.Current.value == "AND")
                {
                    notatedTokens.MoveNext();
                    Expression left = ProcessToExpression(ref notatedTokens);
                    Expression right = ProcessToExpression(ref notatedTokens);
                    return Expression.CreateAnd(left, right);
                }
                else if (notatedTokens.Current.value == "OR")
                {
                    notatedTokens.MoveNext();
                    Expression left = ProcessToExpression(ref notatedTokens);
                    Expression right = ProcessToExpression(ref notatedTokens);
                    return Expression.CreateOr(left, right);
                }
            }
            return null;
        }

        static List<Token> TransformToPolishNotation(List<Token> infixTokenList)
        {
            Queue<Token> outputQueue = new Queue<Token>();
            Stack<Token> stack = new Stack<Token>();

            int index = 0;
            while (infixTokenList.Count > index)
            {
                Token t = infixTokenList[index];

                switch (t.type)
                {
                    case Token.TokenType.LITERAL:
                        outputQueue.Enqueue(t);
                        break;
                    case Token.TokenType.BINARY_OP:
                    case Token.TokenType.UNARY_OP:
                    case Token.TokenType.OPEN_PAREN:
                        stack.Push(t);
                        break;
                    case Token.TokenType.CLOSE_PAREN:
                        while (stack.Peek().type != Token.TokenType.OPEN_PAREN)
                        {
                            outputQueue.Enqueue(stack.Pop());
                        }
                        stack.Pop();
                        if (stack.Count > 0 && stack.Peek().type == Token.TokenType.UNARY_OP)
                        {
                            outputQueue.Enqueue(stack.Pop());
                        }
                        break;
                    default:
                        break;
                }

                ++index;
            }
            while (stack.Count > 0)
            {
                outputQueue.Enqueue(stack.Pop());
            }

            return outputQueue.Reverse().ToList();
        }

    }
}
