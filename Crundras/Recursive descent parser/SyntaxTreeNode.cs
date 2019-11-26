using System.Collections.Generic;

namespace Recursive_descent_parser
{
    public class SyntaxTreeNode
    {
        private List<SyntaxTreeNode> children = null;

        public string Name { get; }
        public uint Id { get; }

        public SyntaxTreeNode(string name, uint id = 0)
        {
            Name = name;
            Id = id;
        }

        public void AddChild(SyntaxTreeNode node)
        {
            if (children == null)
            {
                children = new List<SyntaxTreeNode>();
            }

            children.Add(node);
        }

        public List<SyntaxTreeNode> GetChildren()
        {
            return children;
        }
    }
}