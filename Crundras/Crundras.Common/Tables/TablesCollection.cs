using Crundras.Common.Tables.Literals;

namespace Crundras.Common.Tables
{
    public class TablesCollection
    {
        public TokenTable TokenTable { get; }
        public FloatLiteralsTable FloatLiteralsTable { get; }
        public IntLiteralsTable IntLiteralsTable { get; }
        public IdentifiersTable IdentifiersTable { get; }

        public TablesCollection()
        {
            this.FloatLiteralsTable = new FloatLiteralsTable();
            this.IntLiteralsTable = new IntLiteralsTable();
            this.IdentifiersTable = new IdentifiersTable();
            this.TokenTable = new TokenTable();
        }
    }
}