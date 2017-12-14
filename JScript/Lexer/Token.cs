using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;


namespace JScript.Lexers
{
    public struct Token
    {
        public readonly Fragment Fragment;
        public readonly TokenType Type;
        public Token(Fragment fragment)
        {
            this.Fragment = fragment;
            switch (this.Fragment.Type)
            {
                case FragmentType.None:
                    this.Type = TokenType.None;
                    break;
                case FragmentType.Word:
                    switch (this.Fragment.Text)
                    {
                        case "if":
                            this.Type = TokenType.If;
                            break;
                        case "else":
                            this.Type = TokenType.Else;
                            break;
                        case "elseif":
                            this.Type = TokenType.ElseIf;
                            break;
                        case "for":
                            this.Type = TokenType.For;
                            break;
                        case "foreach":
                            this.Type = TokenType.Foreach;
                            break;
                        case "in":
                            this.Type = TokenType.Break;
                            break;
                        case "while":
                            this.Type = TokenType.While;
                            break;
                        case "break":
                            this.Type = TokenType.Break;
                            break;
                        case "continue":
                            this.Type = TokenType.Continue;
                            break;
                        case "true":
                            this.Type = TokenType.Break;
                            break;
                        case "false":
                            this.Type = TokenType.Break;
                            break;
                        case "null":
                            this.Type = TokenType.Break;
                            break;
                        case "function":
                            this.Type = TokenType.Function;
                            break;
                        case "return":
                            this.Type = TokenType.Return;
                            break;
                        case "this":
                            this.Type = TokenType.Break;
                            break;
                        case "var":
                            this.Type = TokenType.Var;
                            break;
                        case "bool":
                            this.Type = TokenType.Boolean;
                            break;
                        case "double":
                            this.Type = TokenType.Double;
                            break;
                        case "string":
                            this.Type = TokenType.String;
                            break;
                        default:
                            this.Type = TokenType.Word;
                            break;
                    }
                    break;
                case FragmentType.Boundary:
                    switch (this.Fragment.Text[0])
                    {
                        case '+':
                            this.Type = TokenType.OpreationAdd;
                            break;
                        case '-':
                            this.Type = TokenType.OpreationSub;
                            break;
                        case '*':
                            this.Type = TokenType.OpreationMul;
                            break;
                        case '/':
                            this.Type = TokenType.OpreationDiv;
                            break;
                        case '%':
                            this.Type = TokenType.OpreationMod;
                            break;
                        case '.':
                            this.Type = TokenType.Dot;
                            break;
                        case '(':
                            this.Type = TokenType.OpenParen;
                            break;
                        case ')':
                            this.Type = TokenType.CloseParen;
                            break;
                        case '[':
                            this.Type = TokenType.OpenSquare;
                            break;
                        case ']':
                            this.Type = TokenType.CloseSquare;
                            break;
                        case ',':
                            this.Type = TokenType.Comma;
                            break;
                        case '{':
                            this.Type = TokenType.OpenCurly;
                            break;
                        case '}':
                            this.Type = TokenType.CloseCurly;
                            break;
                        case '!':
                            this.Type = TokenType.Not;
                            break;
                        case '&':
                            this.Type = TokenType.And;
                            break;
                        case '|':
                            this.Type = TokenType.Or;
                            break;
                        case '^':
                            this.Type = TokenType.Xor;
                            break;
                        case ';':
                            this.Type = TokenType.End;
                            break;
                        case '=':
                            this.Type = TokenType.Equal;
                            break;
                        default:
                            this.Type = TokenType.None;
                            break;
                    }
                    break;
                default:
                    this.Type = TokenType.None;
                    break;
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