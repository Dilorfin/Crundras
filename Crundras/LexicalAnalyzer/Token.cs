namespace LexicalAnalyzer
{
    public struct Token
    {
        /// <summary>
        /// line of source file where was current token
        /// </summary>
        public uint Line;
        /// <summary>
        /// just lexeme
        /// </summary>
        public string Lexeme;
        /// <summary>
        /// token code in tokens table in specification
        /// </summary>
        public uint Code;
        /// <summary>
        /// id of lexeme in otherwise table
        /// </summary>
        public uint ForeignId;
    }
}