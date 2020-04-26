using System.Collections.Generic;

namespace Recursive_descent_parser
{
    public class SyntaxTreeNode
    {
        private List<SyntaxTreeNode> children = null;

        /// <summary>
        /// node name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// id of identifier or literal
        /// </summary>
        public uint Id { get; }

        public SyntaxTreeNode(string name, uint id = 0)
        {
            Name = name;
            Id = id;
        }

        public void AddChild(SyntaxTreeNode node)
        {
            children ??= new List<SyntaxTreeNode>();

            children.Add(node);
        }

        public List<SyntaxTreeNode> GetChildren()
        {
            return children;
        }
    }
}