using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;


namespace JScript.Lexers
{
    public class SourceReader
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
    }
}