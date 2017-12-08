using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;


namespace JScript.Lexers
{
    public enum FragmentType
    {
        None,
        Word,
        Boundary,
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
   
}
   