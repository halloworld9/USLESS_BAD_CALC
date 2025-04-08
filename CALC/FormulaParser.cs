using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALC
{
    class FormulaParser
    {
        static string ConvertInfixToPostfix(string s)
        {
            if (s == null)
                return "";

            StringBuilder sb = new StringBuilder();

            Stack<char> stack = new Stack<char>();
            char[] opers = { '-', '+', '/', '*' };
            char[] prior = { '/', '*' };
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == '(')
                    stack.Push(c);
                else if ('0' <= c && c <= '9')
                {
                    for (; i < s.Length && '0' <= s[i] && s[i] <= '9'; i++)
                        sb.Append(s[i]);

                    i--;
                    sb.Append(' ');
                }
                else if (c == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        sb.Append(stack.Pop()).Append(" ");
                    }
                    stack.Pop();
                }
                else if (c == '+' || c == '-')
                {
                    while (stack.Count > 0 && opers.Contains(stack.Peek()))
                        s.Append(stack.Pop()).Append(' ');
                    stack.Push(c);
                }
                else if (prior.Contains(c))
                {
                    while (stack.Count > 0 && prior.Contains(stack.Peek()))
                        s.Append(stack.Pop()).Append(' ');
                    stack.Push(c);
                }

            }
            while (stack.Count > 0)
                sb.Append(stack.Pop()).Append(' ');
            return sb.ToString().Trim();
        }

        static double DoOperation(string operation, string num1, string num2)
        {
            var n1 = double.Parse(num1);
            var n2 = double.Parse(num2);
            switch (operation)
            {
                case "-":
                    return n1 - n2;
                case "+":
                    return n1 + n2;
                case "*":
                    return n1 * n2;
                case "/":
                    return n1 / n2;

            }
            throw new InvalidDataException(operation);
            return 0;
        }

        public static string Calculate(string s)
        {
            var strs = ConvertInfixToPostfix(s).Split(' ');
            string[] opers = { "+", "-", "/", "*" };
            Stack<string> stack = new Stack<string>();
            for (int i = 0; i < strs.Length; i++)
            {
                if (opers.Contains(strs[i]))
                {
                    var n1 = stack.Pop();
                    var n2 = stack.Pop();
                    stack.Push(DoOperation(strs[i], n2, n1).ToString());
                }
                else
                {
                    stack.Push(strs[i]);
                }
            }
            return stack.Pop();
        }
    }
}
