using System;
using System.Collections.Generic;

namespace SyntaxAnalyzerPDA.PDA
{
    public static class StackExtension
    {
        public static int? PopValue(this Stack<int> stack, int? value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            var poppedValue = stack.Pop();
            if (poppedValue == value.Value)
            {
                return poppedValue;
            }

            if (char.IsControl((char)value.Value))
            {
                throw new Exception($"Stack error. Expected \'{value.Value}\'");
            }

            throw new Exception($"Stack error. Expected \'{(char)value.Value}\'");
        }

        public static void PushValue(this Stack<int> stack, int? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            stack.Push(value.Value);
        }
    }
}