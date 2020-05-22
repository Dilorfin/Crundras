using System;
using System.Collections.Generic;

namespace Crundras.Common.Tables.Literals
{
    public abstract class LiteralsTable<TValue>
    {
        protected readonly Dictionary<uint, TValue> Values = new Dictionary<uint, TValue>();

        public TValue this[uint index]
        {
            get => Values[index];
            set => Values[index] = value;
        }

        public abstract uint GetId(string lexeme);
        public abstract uint GetId(TValue value);

        public void Display()
        {
            Console.WriteLine($"{GetType().Name} table:");
            foreach (var value in Values)
            {
                Console.WriteLine($"{value.Key,3}{value.Value,15}");
            }
            Console.WriteLine();
        }
    }
}