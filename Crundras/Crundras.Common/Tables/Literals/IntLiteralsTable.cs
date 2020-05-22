using System.Linq;

namespace Crundras.Common.Tables.Literals
{
    public class IntLiteralsTable : LiteralsTable<int>
    {
        public override uint GetId(string lexeme)
        {
            return int.TryParse(lexeme, out int value) ? GetId(value) : 0;
        }

        public override uint GetId(int value)
        {
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