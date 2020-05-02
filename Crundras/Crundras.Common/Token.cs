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
    }
}