using System;
using System.Collections.Generic;
using System.Linq;

namespace Crundras.Common.Tables
{
    public class LabelsTable
    {
        public class Label
        {
            public string Name { get; set; }
            public uint Position { get; set; }
        }

        private readonly Dictionary<uint, Label> labels = new Dictionary<uint, Label>();

        public Label this[uint index] => labels.ContainsKey(index) ? labels[index] : null;

        public int Count => labels.Count;

        public uint GetId(string lexeme)
        {
            var label = labels.FirstOrDefault(p => p.Value.Name == lexeme);
            if (label.Value != null)
            {
                return label.Key;
            }

            var index = (uint)(labels.Count + 1);
            labels[index] = new Label { Name = lexeme };
            return index;
        }

        public void Display()
        {
            Console.WriteLine("Identifiers table:");
            foreach (var (key, value) in labels)
            {
                Console.WriteLine($"{key,3}{value.Name,15}{value.Position,3}");
            }
            Console.WriteLine();
        }
    }
}