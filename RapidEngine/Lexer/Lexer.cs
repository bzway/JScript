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
        private readonly Token[] tokens;
        public Lexer(string code)
        {
            SourceReader reader = new SourceReader(code);
            List<Token> list = new List<Token>();
            while (reader.MoveNext())
            {
                list.Add(new Token(reader.Current));
            }
            this.tokens = list.ToArray();
            this.currentIndex = -1;
        }
        public Token Current { get; private set; }
        private int currentIndex;
        public bool NextToken()
        {
            this.currentIndex++;
            if (this.currentIndex < 0 || this.currentIndex >= this.tokens.Length)
            {
                return false;
            }
            this.Current = this.tokens[this.currentIndex];
            return true;
        }
        public bool PreToken()
        {
            this.currentIndex--;
            if (this.currentIndex < 0 || this.currentIndex >= this.tokens.Length)
            {
                return false;
            }
            this.Current = this.tokens[this.currentIndex];
            return true;
        }
    }
}