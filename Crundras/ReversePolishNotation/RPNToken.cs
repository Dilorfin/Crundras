using Crundras.Common;
using Crundras.Common.Tables;

namespace ReversePolishNotation
{
    public class RPNToken
    {
        public string Name { get; set; }

        public uint LexemeCode { get; set; }

        public uint? Id { get; set; }

        public RPNToken(uint lexemeCode, string name, uint? id = null)
        {
            Name = name;
            LexemeCode = lexemeCode;
            Id = id;
        }

        public RPNToken(uint lexemeCode, uint? id = null)
        {
            Name = LexemesTable.GetLexemeName(lexemeCode);
            LexemeCode = lexemeCode;
            Id = id;
        }
        public RPNToken(SyntaxTreeNode treeNode)
        {
            Name = LexemesTable.GetLexemeName(treeNode.LexemeCode);
            LexemeCode = treeNode.LexemeCode;
            Id = treeNode.Id;
        }
    }
}