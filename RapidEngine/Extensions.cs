using System.Collections.Generic;
using System;
using System.Linq;
using JScript.Parsers;
using JScript.Script;

namespace JScript
{
    public static class Extensions
    {
        public static ScriptTypes GetScriptTypes(this Type o)
        {

            return o == typeof(string) ? ScriptTypes.String
                : o == typeof(int) ? ScriptTypes.Integer
                : o == typeof(double) ? ScriptTypes.Double
                : o == typeof(bool) ? ScriptTypes.Boolean
                : o == typeof(List<string>) ? ScriptTypes.ListString
                : o == typeof(List<int>) ? ScriptTypes.ListInteger
                : o == typeof(List<double>) ? ScriptTypes.ListDouble
                : o == typeof(List<bool>) ? ScriptTypes.ListBoolean
                : ScriptTypes.Any;

        }
    }


  

}