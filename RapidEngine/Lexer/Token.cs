using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;


namespace JScript.Lexers
{
    public class TokenDefinition
    {
        public static List<TokenDefinition> Group(string group)
        {

            return new List<TokenDefinition>();
        }
        
        readonly Func<string, List<Token>> func;
        private TokenDefinition(string regex, TokenType type)
        {
            this.func = (input) =>
            {
                var matcher = new Regex(string.Format("^{0}", regex));
                var result = matcher.Match(input);
                return new List<Token>();
            };
        }

        private TokenDefinition(Func<string, List<Token>> func)
        {
            this.func = func;
        }

        public List<Token> IsMatch(string input)
        {
            return this.func(input);
        }

        public static TokenDefinition Comment = new TokenDefinition(@"#.*", TokenType.Comment);
        public static TokenDefinition String = new TokenDefinition(@"([""'])(?:\\\1|.)*?\1", TokenType.String);
        public static TokenDefinition Integer = new TokenDefinition(@"[-+]?\d+", TokenType.Integer);
        public static TokenDefinition Double = new TokenDefinition(@"[-+]?\d*\.\d+", TokenType.Double);
        public static TokenDefinition Boolean = new TokenDefinition(@"(true|false)", TokenType.Boolean);
        public static TokenDefinition Null = new TokenDefinition(@"null", TokenType.Null);
        public static TokenDefinition Dot = new TokenDefinition(@"\.", TokenType.Dot);
        public static TokenDefinition Comma = new TokenDefinition(@",", TokenType.Comma);
        public static TokenDefinition Assgin = new TokenDefinition(@"(?<name>[_a-zA-Z][_a-zA-Z0-9]*)\s*(?<assign>=|\+=|-=|\*=|\/=|\%=)\s*(?<body>[\s\S]*)\s*;", TokenType.Assign);
        public static TokenDefinition If = new TokenDefinition(@"(?<if>if)\s?\((?<bool>[\s\S]*?)\)\s?\{(?<body>[\s\S]*$?)\}", TokenType.If);
        public static TokenDefinition ElseIf = new TokenDefinition(@"else if\s?\(", TokenType.ElseIf);
        public static TokenDefinition Else = new TokenDefinition(@"else", TokenType.Else);
        public static TokenDefinition For = new TokenDefinition(@"(?<for>for)\s?\((?<step>[\s\S]*?)\)\s?\{\s?(?<body>[\s\S]*?)\s?\}", TokenType.For);
        public static TokenDefinition Function = new TokenDefinition(@"(?<function>function)\s*(?<name>[\s\S]*?)\((?<parameters>[\s\S]*?)\)\s*\{\s*(?<body>[\s\S]*?)\}\n", TokenType.Function);
    }
    public class Token
    {
        public readonly TokenType Type;
        public readonly int Start;
        public readonly int End;
        public readonly int Line;
        public readonly string Text;
        public Token()
        {

        }
    }
}

public enum TokenType
{
    Comment,
    Integer,
    String,
    Boolean,
    Double,
    Null,
    Break,
    None,
    Var,
    OpreationAdd,
    OpreationSub,
    OpreationMul,
    OpreationDiv,
    OpreationMod,
    Function,
    Continue,
    While,
    For,
    Foreach,
    ElseIf,
    Else,
    If,
    /// <summary>
    /// (
    /// </summary>
    OpenParen,
    /// <summary>
    /// )
    /// </summary>
    CloseParen,
    /// <summary>
    /// {
    /// </summary>
    OpenCurly,
    /// <summary>
    /// }
    /// </summary>
    CloseCurly,
    /// <summary>
    /// [
    /// </summary>
    OpenSquare,
    /// <summary>
    /// ]
    /// </summary>
    CloseSquare,
    End,
    Not,
    And,
    Or,
    Xor,
    Word,
    Equal,
    Return,
    Assign,
    /// <summary>
    /// .
    /// </summary>
    Dot,
    /// <summary>
    /// ,
    /// </summary>
    Comma,
}
}