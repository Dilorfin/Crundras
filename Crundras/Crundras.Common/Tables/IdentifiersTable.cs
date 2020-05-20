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
                if (obj is null || obj.GetType() != typeof(Identifier))
                    return false;

                return (obj as Identifier)?.Name == this.Name;
            }

            protected bool Equals(Identifier other)
            {
                return Name == other.Name;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }
        }

        private readonly Dictionary<uint, Identifier> identifiers = new Dictionary<uint, Identifier>();

        public Identifier this[uint index]
        {
            get => identifiers.ContainsKey(index) ? identifiers[index] : null;
            set
            {
                if (identifiers.ContainsKey(index))
                {
                    identifiers[index] = value;
                }
            }
        }

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