using System;
using System.Collections.Generic;
using System.Linq;

namespace Crundras.Common.Tables
{
    public class IdentifiersTable
    {
        public class Identifier
        {
            public uint Type { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private readonly Dictionary<uint, Identifier> identifiers = new Dictionary<uint, Identifier>();

        public Identifier this[uint index] => identifiers.ContainsKey(index) ? identifiers[index] : null;

        public uint GetId(string name)
        {
            var (key, value) = identifiers.FirstOrDefault(p => p.Value.Name == name);
            if (value != null)
            {
                return key;
            }

            var index = (uint)(identifiers.Count + 1);
            identifiers[index] = new Identifier { Name = name };
            return index;
        }

        public void Display()
        {
            Console.WriteLine("Identifiers table:");
            foreach (var (key, value) in identifiers)
            {
                Console.WriteLine($"{key,3}{value.Name,15}{value.Type,3}{value.Value,10}");
            }
            Console.WriteLine();
        }
    }
}