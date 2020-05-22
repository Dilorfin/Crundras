using System;
using System.Linq;

namespace Crundras.Common.Tables.Literals
{
    public class FloatLiteralsTable : LiteralsTable<float>
    {
        public override uint GetId(string lexeme)
        {
            return float.TryParse(lexeme, out float value) ? GetId(value) : 0;
        }

        public override uint GetId(float value)
        {
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