using System.Linq;

namespace Crundras.Common.Tables.Literals
{
    public class IntLiteralsTable : LiteralsTable<int>
    {
        public override uint GetId(string lexeme)
        {
            if (!int.TryParse(lexeme, out int value))
                return 0;

            if (Values.ContainsValue(value))
            {
                return Values.First(
                    v => v.Value == value
                ).Key;
            }

            var id = (uint)(Values.Count + 1);
            Values[id] = value;
            return id;
        }
    }
}