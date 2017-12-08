using JScript.Lexers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JScript.Script
{


    public class ScriptException : Exception
    {
        public ScriptException(string message, Token token) : base(            message + token == null ? string.Empty : token.ToString())
        {

            this.Token = token;
        }

        public Token Token { get; set; }


    }


}
