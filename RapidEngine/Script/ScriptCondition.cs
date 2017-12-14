using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JScript.Script
{
    public class ScriptCondition : ScriptMethod
    {
        public ScriptCondition(string name, Delegate condition)
            : base(name, condition)
        {
            Name = name;
            Method = condition;
            Types = new ScriptTypes[] { };
            ReturnType = ScriptTypes.Boolean;
        }

        public ScriptCondition(string name, Delegate condition, ScriptTypes[] types)
            : base(name, condition, types, ScriptTypes.Void)
        {
            Name = name;
            Method = condition;
            Types = types;
            ReturnType = ScriptTypes.Boolean;
        }
    }
}
