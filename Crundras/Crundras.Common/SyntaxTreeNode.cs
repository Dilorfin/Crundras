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
        /// lexeme code
        /// </summary>
        public uint LexemeCode { get; }

        /// <summary>
        /// id of identifier or literal
        /// </summary>
        public uint? Id { get; }

        public SyntaxTreeNode(uint lexemeCode, uint? id = null)
        {
            LexemeCode = lexemeCode;
            Id = id;
        }

        public SyntaxTreeNode(Token token)
        {
            LexemeCode = token.Code;
            Id = token.ForeignId;
        }

        public void AddChild(SyntaxTreeNode node)
        {
            Children ??= new List<SyntaxTreeNode>();

            node.SetParent(this);
            Children.Add(node);
        }

        public void AddChildren(IEnumerable<SyntaxTreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                AddChild(node);
            }
        }

        private void SetParent(SyntaxTreeNode parent)
        {
            Parent = parent;
        }
    }
}