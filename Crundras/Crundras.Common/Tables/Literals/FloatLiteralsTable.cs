using System;
using System.Linq;

namespace Crundras.Common.Tables.Literals
{
    public class FloatLiteralsTable : LiteralsTable<float>
    {
        public override uint GetId(string lexeme)
        {
            if (!float.TryParse(lexeme, out float value))
                return 0;

            if (Values.ContainsValue(value))
            {
                return Values.First(
                    v => Math.Abs(v.Value - value) < float.Epsilon
                ).Key;
            }

            var id = (uint)(Values.Count + 1);
            Values[id] = value;
            return id;
        }
    }
}