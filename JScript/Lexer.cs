using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace JScript
{

    public class Lexer
    {

        private readonly Fragment[] fragments;
        public Lexer(string code)
        {

            var reader = new SourceReader(code);
            this.fragments = reader.ToArray();
            this.Current = -1;
        }
        public int Current { get; private set; }

        public Token NextToken()
        {
            this.Current++;
            if (this.Current < 0 || this.Current >= this.fragments.Length)
            {
                return new Token();
            }
            var item = fragments[this.Current];
            return new Token(item);
        }

        public Token PrevToken()
        {
            this.Current--;
            if (this.Current < 0 || this.Current >= this.fragments.Length)
            {
                return new Token();
            }
            var item = fragments[this.Current];
            return new Token(item);
        }
    }
    public class SourceReader : IEnumerable<Fragment>, IEnumerator<Fragment>
    {
        private int Index;
        private int Line;
        private int Column;
        private readonly string code;
        public SourceReader(string code)
        {
            this.code = code;
            this.Reset();
        }
        public Fragment Current { get; private set; }
        object IEnumerator.Current => this.Current;
        public void Dispose()
        {
        }
        public bool MoveNext()
        {
            var letter = this.ReadLetter();
            if (letter == char.MaxValue)
            {
                this.Current = new Fragment(this.Column - 1, this.Column - 1, this.Line, string.Empty, FragmentType.None);
                return false;
            }
            //过滤掉开头的空格
            while (char.IsWhiteSpace(letter))
            {
                letter = this.ReadLetter();
            }
            var start = this.Column - 1;
            var temp = string.Empty;

            if (letter == char.MaxValue)
            {
                temp += letter;
                this.Current = new Fragment(start, this.Column - 1, this.Line, temp, FragmentType.None);
                return true;
            }
            if (!char.IsLetterOrDigit(letter))
            {
                temp += letter;
                this.Current = new Fragment(start, this.Column - 1, this.Line, temp, FragmentType.Boundary);
                return true;
            }

            do
            {
                if (!char.IsLetterOrDigit(letter))
                {
                    this.Current = new Fragment(start, this.Column - 1, this.Line, temp, FragmentType.Word);
                    this.Index--;
                    this.Column--;
                    return true;
                }
                temp += letter;
                letter = this.ReadLetter();
            } while (true);
        }
        char ReadLetter()
        {
            if (this.Index >= this.code.Length)
            {
                return char.MaxValue;
            }
            var cp = this.code[this.Index];
            this.Index++;
            this.Column++;
            if (cp == '\n')
            {
                this.Line++;
                this.Column = 0;
            }
            return cp;
        }
        public void Reset()
        {
            this.Index = 0;
            this.Line = 1;
            this.Column = 0;
        }
        public IEnumerator<Fragment> GetEnumerator()
        {
            return this;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }

    public struct Fragment
    {
        public readonly int Start;
        public readonly int End;
        public readonly int Line;
        public readonly string Text;
        public readonly FragmentType Type;
        public Fragment(int Start, int End, int Line, string Text, FragmentType Type)
        {
            this.Start = Start;
            this.End = End;
            this.Line = Line;
            this.Text = Text;
            this.Type = Type;
        }
        public override string ToString()
        {
            return string.Format("{0}\tat line {1} from {2} to {3}", this.Text, this.Line, this.Start, this.End);
        }
    }
    public enum FragmentType
    {
        None,
        Word,
        Boundary,
    }

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
                        case '-':
                        case '*':
                        case '/':
                        case '%':
                            this.Type = TokenType.Opreation;
                            break;
                        case '[':
                            this.Type = TokenType.ArrayLeft;
                            break;
                        case ']':
                            this.Type = TokenType.ArrayRight;
                            break;
                        case '(':
                            this.Type = TokenType.Left;
                            break;
                        case ')':
                            this.Type = TokenType.Right;
                            break;
                        case '{':
                            this.Type = TokenType.BlockStart;
                            break;
                        case '}':
                            this.Type = TokenType.BlockEnd;
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
                            this.Type = TokenType.Eq;
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
    public class TokenDefinition
    {
        public TokenType Type { get; set; }
        public Regex Regex { get; set; }
        public TokenDefinition(string reg, TokenType type)
        {
            this.Regex = new Regex(reg, RegexOptions.Compiled);
            this.Type = type;
        }

    }
    public enum TokenType
    {
        BlockEnd,
        Comment,
        Integer,
        String,
        Boolean,
        Double,
        Null,
        Break,
        None,
        Var,
        Opreation,
        Function,
        Continue,
        While,
        For,
        Foreach,
        ElseIf,
        Else,
        If,
        ArrayLeft,
        ArrayRight,
        Left,
        Right,
        BlockStart,
        End,
        Not,
        And,
        Or,
        Xor,
        Word,
        Eq,
        Return,
    }
}