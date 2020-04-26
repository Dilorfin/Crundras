using System.Collections.Generic;

namespace SyntaxAnalyzerPDA
{
    public class SyntaxTreeNode
    {
        private List<SyntaxTreeNode> children = null;

        public SyntaxTreeNode Parent { get; private set; } = null;

        public string Name { get; }
        public uint Id { get; }

        public SyntaxTreeNode(string name, uint id = 0)
        {
            Name = name;
            Id = id;
        }

        public void AddChild(SyntaxTreeNode node)
        {
            children ??= new List<SyntaxTreeNode>();

            node.SetParent(this);
            children.Add(node);
        }

        private void SetParent(SyntaxTreeNode parent)
        {
            Parent = parent;
        }

        public List<SyntaxTreeNode> GetChildren()
        {
            return children;
        }
    }
}