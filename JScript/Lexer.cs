using System;
using System.Text.RegularExpressions;

namespace JScript
{

    public class Lexer
    {

    } 
    public class Position
    {
        public int Line { get; set; }
        public int Col { get; set; }
    }
    public class Token
    {
        public Position Position { get; set; }
        public string Value { get; set; }
        public TokenType Type { get; set; }
    }
    public class TokenDefinition
    {
        public TokenType Type { get; set; }
        public Regex Regex { get; set; }

    }
    public enum TokenType
    {
        Char,
        Symbol,
        Number,
        Decimal,
        Identifier,
        Keyword,
        QuotedString,
        WhiteSpace,
        EndOfLine,
        Comment,
        Start,
        End
    }
}
