using System.Collections.Generic;

namespace Crundras.Common
{
    public class SyntaxTreeNode
    {
        /// <summary>
        /// parent node
        /// </summary>
        public SyntaxTreeNode Parent { get; private set; }

        /// <summary>
        /// list of child nodes
        /// </summary>
        public List<SyntaxTreeNode> Children { get; private set; }

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
            Children ??= new List<SyntaxTreeNode>();

            node.SetParent(this);
            Children.Add(node);
        }

        private void SetParent(SyntaxTreeNode parent)
        {
            Parent = parent;
        }
    }
}