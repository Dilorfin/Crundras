namespace Crundras.Common
{
    public struct Token
    {
        /// <summary>
        /// line of source file where was current token
        /// </summary>
        public uint Line { get; set; }

        /// <summary>
        /// token code in tokens table in specification
        /// </summary>
        public uint Code { get; set; }

        /// <summary>
        /// id of lexeme in otherwise table (or null)
        /// </summary>
        public uint? ForeignId { get; set; }

        public static bool IsIntLiteral(uint code)
        {
            return code == 3;
        }
        public static bool IsFloatLiteral(uint code)
        {
            return code == 4;
        }
        public static bool IsLiteral(uint code)
        {
            return IsFloatLiteral(code) || IsIntLiteral(code);
        }
        public static bool IsIdentifier(uint code)
        {
            return code == 1;
        }
        public static bool IsIdentifierOrLiteral(uint code)
        {
            return IsLiteral(code) || IsIdentifier(code);
        }
    }
}