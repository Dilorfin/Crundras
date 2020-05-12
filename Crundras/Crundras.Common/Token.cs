namespace Crundras.Common
{
    public struct Token
    {
        /// <summary>
        /// line of source file where was current token
        /// </summary>
        public uint Line;
        /// <summary>
        /// token code in tokens table in specification
        /// </summary>
        public uint Code;
        /// <summary>
        /// id of lexeme in otherwise table (or null)
        /// </summary>
        public uint? ForeignId;

        public static bool IsIdentifier(uint code)
        {
            return code == 1;
        }
        public static bool IsLiteral(uint code)
        {
            return code == 2 || code == 3;
        }
        public static bool IsIdentifierOrLiteral(uint code)
        {
            return IsLiteral(code) || IsIdentifier(code);
        }
    }
}