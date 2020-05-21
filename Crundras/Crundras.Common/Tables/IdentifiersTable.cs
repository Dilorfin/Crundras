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

            public override bool Equals(object obj)
            {
                if (obj is null || !(obj is Identifier identifier)) 
                    return false;

                return identifier.Name == this.Name 
                       && identifier.Type == this.Type;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Type, Name);
            }
        }

        private readonly Dictionary<uint, Identifier> identifiers = new Dictionary<uint, Identifier>();

        public Identifier this[uint index] => identifiers.ContainsKey(index) ? identifiers[index] : null;

        public uint GetId(string name)
        {
            var identifier = new Identifier { Name = name };

            if (identifiers.ContainsValue(identifier))
            {
                return identifiers.First(p => p.Value.Equals(identifier)).Key;
            }

            var index = (uint)(identifiers.Count + 1);
            identifiers[index] = identifier;
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