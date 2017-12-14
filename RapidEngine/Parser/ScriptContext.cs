using System;
using System.Collections.Generic;
using System.Text;

namespace JScript.Parsers
{


    public class ScriptContext
    {
        public ScriptContext parent { get; set; }
        public string Name { get; set; }
        public Delegate Function { get; set; }
        internal Dictionary<string, Delegate> functions;
        internal Dictionary<string, object> Variables;
        public object this[string name]
        {
            get
            {
                if (this.Variables.ContainsKey(name))
                {
                    return this.Variables[name];
                }
                return null;
            }
            set
            {
                if (this.Variables.ContainsKey(name))
                {
                    this.Variables[name] = value;
                }
                else
                {
                    this.Variables.Add(name, value);
                }
            }
        }
        public ScriptContext()
        {
            this.functions = new Dictionary<string, Delegate>();
            this.Variables = new Dictionary<string, object>();
        }
    }
    public interface IScriptType
    {
        object Value { get; }
    }
    public interface IScriptType<T> : IScriptType
    {
        T Value { get; }
    }

    public struct AnyScriptType : IScriptType<object>
    {
        public object Value { get; set; }
    }
    public struct IntegerScriptType : IScriptType<int>
    {
        public int Value { get; set; }

        object IScriptType.Value => this.Value;
    }
    public struct DoubleScriptType : IScriptType<double>
    {
        public double Value { get; set; }
        object IScriptType.Value => this.Value;
    }
    public struct StringScriptType : IScriptType<string>
    {
        public string Value { get; set; }
        object IScriptType.Value => this.Value;
    }
    public struct BooleanScriptType : IScriptType<bool>
    {
        public bool Value { get; set; }
        object IScriptType.Value => this.Value;
    }
    public struct ListIntegerScriptType : IScriptType<int[]>
    {
        public int[] Value { get; set; }
        object IScriptType.Value => this.Value;
    }
    public struct ListDoubleScriptType : IScriptType<double[]>
    {
        public double[] Value { get; set; }
        object IScriptType.Value => this.Value;
    }
    public struct ListStringScriptType : IScriptType<string[]>
    {
        public string[] Value { get; set; }
        object IScriptType.Value => this.Value;

    }
    public struct ListBooleanScriptType : IScriptType<bool[]>
    {
        public bool[] Value { get; set; }
        object IScriptType.Value => this.Value;

    }
    public struct UndefinedScriptType : IScriptType<bool>
    {
        public bool Value => true;
        object IScriptType.Value => this.Value;

    }
    public struct VoidScriptType : IScriptType<bool>
    {
        public bool Value => true;
        object IScriptType.Value => this.Value;

    }
    public struct BreakScriptType : IScriptType<bool>
    {
        public bool Value => true;
        object IScriptType.Value => this.Value;
    }
    public struct ReturnScriptType : IScriptType<AnyScriptType>
    {
        public AnyScriptType Value { get; set; }
        object IScriptType.Value => this.Value;
    }


}