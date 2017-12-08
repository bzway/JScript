using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using JScript.Script;

namespace JScript.Lexers
{
    public class Lexer
    {
        private static Regex regexIdentifier = new Regex("^[_a-zA-Z][_a-zA-Z0-9]*");
        private readonly SourceReader reader;
        public Lexer(string code)
        {
            this.reader = new SourceReader(code);
        }
        public List<Token> Current { get; private set; }
        public TokenType CurrentType { get; private set; }
        public bool NextToken()
        {
            this.Current = new List<Token>();
            this.CurrentType = TokenType.None;
            return this.StepBlock();
        }

        public bool StepBlock()
        {
            if (!this.reader.MoveNext())
            {
                return false;
            }
            var token = new Token(this.reader.Current);
            switch (token.Type)
            {
                case TokenType.Word:
                    if (!regexIdentifier.IsMatch(token.Fragment.Text))
                    {
                        throw new ScriptException("命名不正确", token);
                    }
                    this.Current.Add(token);
                    while (this.reader.MoveNext())
                    {
                        token = new Token(this.reader.Current);
                        if (token.Type == TokenType.End)
                        {
                            this.CurrentType = TokenType.Assign;
                            return true;
                        }
                        this.Current.Add(token);
                    }
                    return false;
                case TokenType.While:
                    this.Current.Add(token);
                    if (!this.reader.MoveNext())
                    {
                        return false;
                    }
                    token = new Token(this.reader.Current);
                    if (token.Type != TokenType.Left)
                    {
                        return false;
                    }
                    this.Current.Add(token);
                    while (this.reader.MoveNext())
                    {
                        token = new Token(this.reader.Current);
                        if (token.Type == TokenType.Right)
                        {
                            this.Current.Add(token);
                            this.CurrentType = TokenType.While;
                            return true;
                        }
                    }
                    return false;
                case TokenType.For:
                    this.Current.Add(token);
                    if (!this.reader.MoveNext())
                    {
                        return false;
                    }
                    token = new Token(this.reader.Current);
                    if (token.Type != TokenType.Left)
                    {
                        return false;
                    }
                    this.Current.Add(token);
                    while (this.reader.MoveNext())
                    {
                        token = new Token(this.reader.Current);
                        if (token.Type == TokenType.Right)
                        {
                            this.Current.Add(token);
                            this.CurrentType = TokenType.For;
                            return true;
                        }
                    }
                    return false;
                case TokenType.Foreach:
                    this.Current.Add(token);
                    if (!this.reader.MoveNext())
                    {
                        return false;
                    }
                    token = new Token(this.reader.Current);
                    if (token.Type != TokenType.Left)
                    {
                        return false;
                    }
                    this.Current.Add(token);
                    while (this.reader.MoveNext())
                    {
                        token = new Token(this.reader.Current);
                        if (token.Type == TokenType.Right)
                        {
                            this.Current.Add(token);
                            this.CurrentType = TokenType.Foreach;
                            return true;
                        }
                    }
                    return false;
                case TokenType.If:
                    this.Current.Add(token);
                    if (!this.reader.MoveNext())
                    {
                        return false;
                    }
                    token = new Token(this.reader.Current);
                    if (token.Type != TokenType.Left)
                    {
                        return false;
                    }
                    this.Current.Add(token);
                    while (this.reader.MoveNext())
                    {
                        token = new Token(this.reader.Current);
                        if (token.Type == TokenType.Right)
                        {
                            this.Current.Add(token);
                            this.CurrentType = TokenType.If;
                            return true;
                        }
                    }
                    return false;
                case TokenType.Return:
                    if (!this.reader.MoveNext())
                    {
                        return false;
                    }
                    token = new Token(this.reader.Current);
                    this.Current.Add(token);
                    this.CurrentType = TokenType.Return;
                    return true;
                case TokenType.Function:
                    this.Current.Add(token);
                    this.Current.Add(token);
                    if (!this.reader.MoveNext())
                    {
                        return false;
                    }
                    token = new Token(this.reader.Current);
                    if (token.Type != TokenType.Left)
                    {
                        return false;
                    }
                    this.Current.Add(token);
                    while (this.reader.MoveNext())
                    {
                        token = new Token(this.reader.Current);
                        if (token.Type == TokenType.Right)
                        {
                            this.Current.Add(token);
                            this.CurrentType = TokenType.Function;
                            return true;
                        }
                    }
                    return false;
                default:
                    return true;
            }
        }
    }
}