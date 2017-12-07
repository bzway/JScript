using System.Collections.Generic;
using System;
using System.Linq;

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

    public class ScriptVariable
    {
        public object Value { get; set; }

        public ScriptTypes Type { get; set; }

        public string Name { get; set; }

        public ScriptVariable()
        {
            Value = null;
            Type = ScriptTypes.Undefined;
        }

        public ScriptVariable(object value)
        {
            Value = value;
            Type = this.Value.GetType().GetScriptTypes();
        }

        public ScriptVariable(object value, ScriptTypes type)
        {
            Value = value;
            Type = type;
        }

        public ScriptVariable(string name, object value)
        {
            Name = name;
            Value = value;
            Type = this.Value.GetType().GetScriptTypes();
        }

        public ScriptVariable(string name, object value, ScriptTypes type)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public T Return<T>()
        {
            var returnT = typeof(T).GetScriptTypes();
            switch (returnT)
            {
                case ScriptTypes.String:
                case ScriptTypes.Integer:
                case ScriptTypes.Double:
                case ScriptTypes.Boolean:
                case ScriptTypes.Regex:
                    return (T)this.Value;
                case ScriptTypes.ListString:
                    return (T)(object)((List<ScriptVariable>)this.Value).Select(x => x.Value.ToString()).ToList();
                case ScriptTypes.ListInteger:
                    return (T)(object)((List<ScriptVariable>)(this.Value)).Select(x => x).ToList();
                case ScriptTypes.ListDouble:
                    return (T)(object)((List<ScriptVariable>)(this.Value)).Select(x => x).ToList();
                case ScriptTypes.ListBoolean:
                    return (T)(object)((List<ScriptVariable>)(this.Value)).Select(x => x).ToList();
                default:
                    return default(T);
            }
        }

        public ScriptVariable Cast<ReturnT>(Lexer lexer)
        {
            var outputType = typeof(ReturnT).GetScriptTypes();
            switch (outputType)
            {
                case ScriptTypes.String:
                    switch (this.Type)
                    {
                        case ScriptTypes.Integer:
                        case ScriptTypes.Double:
                            this.Value = this.Value.ToString();
                            break;
                        case ScriptTypes.Boolean:
                            this.Value = (bool)this.Value ? "true" : "false";
                            break;
                        case ScriptTypes.Null:
                            this.Value = "null";
                            break;
                    }
                    this.Type = ScriptTypes.String;
                    break;
                case ScriptTypes.Integer:
                    switch (this.Type)
                    {
                        case ScriptTypes.String:
                            int tryInt = 0;
                            if (int.TryParse(this.Value.ToString(), out tryInt))
                            {
                                this.Value = tryInt;
                            }
                            else
                            {
                                goto castError;
                            }
                            break;
                        case ScriptTypes.Double:
                            double tryDouble = 0;
                            if (double.TryParse(this.Value.ToString(), out tryDouble))
                            {
                                this.Value = tryDouble;
                            }
                            else
                            {
                                goto castError;
                            }
                            break;
                        case ScriptTypes.Boolean:
                            this.Value = (bool)this.Value ? 1 : 0;
                            break;
                    }
                    this.Type = ScriptTypes.Integer;
                    break;
                case ScriptTypes.Double:
                    switch (this.Type)
                    {
                        case ScriptTypes.String:
                        case ScriptTypes.Integer:
                            double tryDouble = 0;
                            if (double.TryParse(this.Value.ToString(), out tryDouble))
                            {
                                this.Value = tryDouble;
                            }
                            else
                            {
                                goto castError;
                            }
                            break;
                        case ScriptTypes.Boolean:
                            this.Value = (bool)this.Value ? 1.0 : 0.0;
                            break;
                    }
                    this.Type = ScriptTypes.Double;
                    break;
                case ScriptTypes.Boolean:

                    switch (this.Type)
                    {
                        case ScriptTypes.String:
                            this.Value = this.Value.ToString() == "true";
                            break;
                    }
                    this.Type = ScriptTypes.Boolean;
                    break;
                case ScriptTypes.ListString:
                case ScriptTypes.ListInteger:
                case ScriptTypes.ListDouble:
                case ScriptTypes.ListBoolean:

                    break;
                case ScriptTypes.Void:
                    this.Value = default(ReturnT);
                    break;
            }
            return this;
            castError:
            throw new Exception("");
            //lexer.Prev();
            //lexer.Prev();
            //throw new ScriptException(
            //    message: String.Format("Unable to cast value '{0}' from '{1}' to '{2}' on Line {3} Col {4}",
            //        Value.ToString(),
            //        Type.ToString(),
            //        outputType.ToString(),
            //        lexer.LineNumber,
            //        lexer.Position),
            //    row: lexer.LineNumber,
            //    column: lexer.Position,
            //    method: lexer.TokenContents
            //);
        }
    }

    public interface IASTNode
    {
        IASTNode Left { get; }
        IASTNode Right { get; }
        ASTNodeType Type { get; }

        object Value();
        List<IASTNode> Children { get; }
    }



    public class ScriptException : Exception
    {
        public ScriptException(string message, int column = 0, int row = 0, string method = null) : base(message)
        {
            Row = row;
            Column = column;
            Method = method;
        }

        public string Method { get; set; }

        public int Column { get; set; }

        public int Row { get; set; }
    }


    public class ScriptClass
    {
        public string Name { get; set; }
        public Delegate Function { get; set; }

        internal Action<ScriptException> Error;
        internal Dictionary<string, ScriptVariable> dictionary = new Dictionary<string, ScriptVariable>();
        internal List<ScriptVariable> Variables = new List<ScriptVariable>();
        internal List<ScriptVariable> Properties = new List<ScriptVariable>();
        internal List<ScriptMethod> Methods = new List<ScriptMethod>();

        public ScriptClass()
        {

        }

        public ScriptClass(string name)
        {
            Name = name;
        }

        public void SetVariable(string name, ScriptVariable value)
        {
            var property = Variables.FirstOrDefault(p => p.Name == name);
            if (property == null)
            {
                value.Name = name;
                Variables.Add(value);
            }
            else
            {
                if (property.Type == value.Type)
                {
                    property.Value = value;
                }
                else
                {
                    throw new ScriptException(
                        message: String.Format("'{0}' requires a data type of {1}",
                            name,
                            value.Type),
                        row: 0,
                        column: 0
                    );
                }
            }
        }

        public ScriptVariable GetProperty(string name)
        {
            return Variables.Where(v => v.Name == name).FirstOrDefault();
        }

        public void DeleteProperty(string name)
        {
            Variables.RemoveAll(property => property.Name == name);
        }

        public void AddProperty(string name, string value)
        {
            Properties.Add(new ScriptVariable(name, value, ScriptTypes.String));
        }

        public void AddProperty(string name, int value)
        {
            Properties.Add(new ScriptVariable(name, value, ScriptTypes.Integer));
        }

        public void AddProperty(string name, double value)
        {
            Properties.Add(new ScriptVariable(name, value, ScriptTypes.Double));
        }

        public void AddProperty(string name, bool value)
        {
            Properties.Add(new ScriptVariable(name, value, ScriptTypes.Double));
        }

        public void AddProperty(string name, Regex value)
        {
            Properties.Add(new ScriptVariable(name, value, ScriptTypes.Double));
        }

        public void AddProperty(string name, List<string> value)
        {
            Properties.Add(new ScriptVariable(name, value, ScriptTypes.Double));
        }

        public void AddProperty(string name, List<int> value)
        {
            Properties.Add(new ScriptVariable(name, value, ScriptTypes.Double));
        }

        public void AddProperty(string name, List<double> value)
        {
            Properties.Add(new ScriptVariable(name, value, ScriptTypes.Double));
        }

        public void AddProperty(string name, List<bool> value)
        {
            Properties.Add(new ScriptVariable(name, value, ScriptTypes.Double));
        }

        private void AddProperty(string name, object value)
        {
            Properties.Add(new ScriptVariable(name, value));
        }

        /// <summary>
        /// Add a user defined function to the script engine.
        /// </summary>
        /// <param name="name">Function name in the script. Example "dialog"</param>
        /// <param name="function">A Func definition.</param>
        public void AddFunction<TResult>(string name, Func<TResult> function)
        {
            ScriptTypes[] args = { };
            var tr = ScriptType.ToEnum(typeof(TResult));
            Methods.Add(new ScriptFunction(name, function, args, tr));
        }

        public void AddFunction<T1, TResult>(string name, Func<T1, TResult> function)
        {
            var t1 = ScriptType.ToEnum(typeof(T1));
            var tr = ScriptType.ToEnum(typeof(TResult));
            ScriptTypes[] args = { t1 };
            Methods.Add(new ScriptFunction(name, function, args, tr));
        }

        public void AddFunction<T1, T2, TResult>(string name, Func<T1, T2, TResult> function)
        {
            var t1 = ScriptType.ToEnum(typeof(T1));
            var t2 = ScriptType.ToEnum(typeof(T2));
            var tr = ScriptType.ToEnum(typeof(TResult));
            ScriptTypes[] args = { t1, t2 };
            Methods.Add(new ScriptFunction(name, function, args, tr));
        }

        /// <summary>
        /// Add an action with 0 arguments. Actions do not have a return type.
        /// </summary>
        /// <param name="name">The action's name in the script.</param>
        /// <param name="action">Executed when action name is found and argument types match.</param>
        public void AddAction(string name, Action action)
        {
            Methods.Add(new ScriptFunction(name, action));
        }


        public void AddAction<T1>(string name, Action<T1> action)
        {
            var t1 = ScriptType.ToEnum(typeof(T1));
            ScriptTypes[] args = { t1 };
            Methods.Add(new ScriptFunction(name, action, args));
        }


        public void AddAction<T1, T2>(string name, Action<T1, T2> action)
        {
            var t1 = ScriptType.ToEnum(typeof(T1));
            var t2 = ScriptType.ToEnum(typeof(T2));
            ScriptTypes[] args = { t1, t2 };
            Methods.Add(new ScriptFunction(name, action, args));
        }


        public void AddAction<T1, T2, T3>(string name, Action<T1, T2, T3> action)
        {
            var t1 = ScriptType.ToEnum(typeof(T1));
            var t2 = ScriptType.ToEnum(typeof(T2));
            var t3 = ScriptType.ToEnum(typeof(T3));
            ScriptTypes[] args = { t1, t2, t3 };
            Methods.Add(new ScriptFunction(name, action, args));
        }

        /// <summary>
        /// Add a user defined function to the script engine.
        /// </summary>
        /// <param name="name">Function name in the script. Example "dialog"</param>
        /// <param name="function">A Predicate definition.</param>
        public void AddCondition<T1>(string name, Func<T1, bool> condition)
        {
            var t1 = ScriptType.ToEnum(typeof(T1));
            ScriptTypes[] args = { t1 };
            Methods.Add(new ScriptCondition(name, condition, args));
        }

        public void AddCondition<T1, T2>(string name, Func<T1, T2, bool> condition)
        {
            var t1 = ScriptType.ToEnum(typeof(T1));
            var t2 = ScriptType.ToEnum(typeof(T2));
            ScriptTypes[] args = { t1, t2 };
            Methods.Add(new ScriptCondition(name, condition, args));
        }

        internal List<ScriptClass> Classes = new List<ScriptClass>();

        public ScriptClass AddClass(string name)
        {
            var newClass = new ScriptClass(name);
            Classes.Add(newClass);
            return newClass;
        }

        public ScriptClass AddClass(string name, Delegate function)
        {
            var newClass = new ScriptClass(name);
            Classes.Add(newClass);
            return newClass;
        }
    }
}