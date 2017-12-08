using JScript.Lexers;
using JScript.Parsers;
using JScript.Script;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JScript.Parsers
{
    public class VariableNode : AbstractSyntaxNode<AnyScriptType>
    {
        string name;
        public VariableNode(string name)
        {
            this.name = name;
        }
        public override AnyScriptType Value(ScriptContext context)
        {
            var result = new AnyScriptType()
            {
                Value = context[name]
            };
            return result;
        }
    }
    public class ValueNode : AbstractSyntaxNode<AnyScriptType>
    {
        private AnyScriptType value;
        public ValueNode(AnyScriptType value)
        {
            this.value = value;
        }
        public override AnyScriptType Value(ScriptContext context)
        {
            return this.value;
        }
    }
    public class AssignNode : AbstractSyntaxNode<VoidScriptType>
    {
        private readonly string name;
        private readonly object value;
        public AssignNode(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
        public override VoidScriptType Value(ScriptContext context)
        {
            context[this.name] = this.value;
            return new VoidScriptType();
        }
    }
    public class SequenceNode : AbstractSyntaxNode<IScriptType>
    {
        private readonly ISyntaxNode[] nodes;
        public SequenceNode(params ISyntaxNode[] nodes)
        {
            this.nodes = nodes;
        }
        public override IScriptType Value(ScriptContext context)
        {
            IScriptType result = new VoidScriptType();
            foreach (var item in this.nodes)
            {
                result = item.Value(context);
                if (result is ReturnScriptType)
                {
                    return result;
                }
            }
            return result;
        }
    }
    public class ForNode : AbstractSyntaxNode<IScriptType>
    {
        private SequenceNode assign;
        private ConditionNode condition;
        private SequenceNode sequence;
        private SequenceNode step;
        public ForNode(SequenceNode assign, ConditionNode condition, SequenceNode sequence, SequenceNode step)
        {
            this.assign = assign;
            this.condition = condition;
            this.sequence = sequence;
            this.step = step;
        }

        public override IScriptType Value(ScriptContext context)
        {
            for (this.assign.Value(context); this.condition.Value(context).Value; this.step.Value(context))
            {
                var result = this.sequence.Value(context);
                if (result is ReturnScriptType)
                {
                    return result;
                }
                if (result is BreakScriptType)
                {
                    break;
                }
            }
            return new VoidScriptType();
        }
    }
    public class ConditionNode : AbstractSyntaxNode<BooleanScriptType>
    {
        private readonly string name;
        private readonly string type;
        private readonly object value;
        public ConditionNode(string name, string type, object value)
        {
            this.name = name;
            this.type = type;
            this.value = value;

        }
        public override BooleanScriptType Value(ScriptContext context)
        {
            if (this.value == null)
            {
                return new BooleanScriptType() { Value = false };
            }
            var varible = context[name];
            if (varible == null)
            {
                return new BooleanScriptType() { Value = false };
            }
            switch (this.type)
            {
                case ">":

                    return new BooleanScriptType() { Value = (double.Parse(varible.ToString()) > double.Parse(this.value.ToString())) };
                default:
                    return new BooleanScriptType() { Value = false };
            }
        }
    }
    public class IfNode : AbstractSyntaxNode<IScriptType>
    {
        ConditionNode contidition;
        SequenceNode tureNode;
        SequenceNode falseNode;
        public IfNode(ConditionNode contidition, SequenceNode tureNode, SequenceNode falseNode)
        {
            this.contidition = contidition;
            this.tureNode = tureNode;
            this.falseNode = falseNode;
        }
        public override IScriptType Value(ScriptContext context)
        {
            var condition = this.contidition.Value(context);
            if (condition.Value)
            {
                return this.tureNode.Value(context);
            }
            else
            {
                return this.falseNode.Value(context);
            }
        }
    }
    public class ReturnNode : AbstractSyntaxNode<AnyScriptType>
    {
        ISyntaxNode syntaxNode;
        public ReturnNode(ISyntaxNode syntaxNode)
        {
            this.syntaxNode = syntaxNode;
        }

        public override AnyScriptType Value(ScriptContext context)
        {
            return (AnyScriptType)this.syntaxNode.Value(context);
        }
    }
    public class BreakNode : AbstractSyntaxNode<VoidScriptType>
    {
        public override VoidScriptType Value(ScriptContext context)
        {
            return new VoidScriptType();
        }
    }
    public class WhileNode : AbstractSyntaxNode<IScriptType>
    {
        ConditionNode condition;
        SequenceNode sequence;
        public WhileNode(ConditionNode condition, SequenceNode sequence)
        {
            this.condition = condition;
            this.sequence = sequence;
        }
        public override IScriptType Value(ScriptContext context)
        {
            IScriptType result = new VoidScriptType();
            while (this.condition.Value(context).Value)
            {
                result = this.sequence.Value(context);
                if (result is ReturnScriptType)
                {
                    return result;
                }
            }
            return result;
        }
    }
    public class OperateNode : AbstractSyntaxNode<AnyScriptType>
    {
        AbstractSyntaxNode<AnyScriptType> left;
        AbstractSyntaxNode<AnyScriptType> right;
        TokenType type;
        public OperateNode(AbstractSyntaxNode<AnyScriptType> left, AbstractSyntaxNode<AnyScriptType> right, TokenType type)
        {
            this.left = left;
            this.right = right;
            this.type = type;
        }
        public override AnyScriptType Value(ScriptContext context)
        {
            throw new NotImplementedException();
        }
    }
  
    public class FunctionNode : AbstractSyntaxNode<VoidScriptType>
    {
        string name;

        SequenceNode right;
        public FunctionNode(string name, SequenceNode right)
        {
            this.name = name;
            this.right = right;
        }
        public override VoidScriptType Value(ScriptContext context)
        {

            Delegate fun = new Func<string, object>(input =>
            {
                return input;
            });

            context.functions.Add(this.name, fun);
            return new VoidScriptType();
        }
    }
    public class InvokeNode : AbstractSyntaxNode<AnyScriptType>
    {
        string name;
        ISyntaxNode[] parameters;
        public InvokeNode(string name, params ISyntaxNode[] parameters)
        {
            this.name = name;
            this.parameters = parameters;
        }
        public override AnyScriptType Value(ScriptContext context)
        {
            Delegate fun = context.functions[this.name];
            List<object> list = new List<object>();
            foreach (var item in this.parameters)
            {
                list.Add(item.Value(context).Value);
            }
            var result = fun.DynamicInvoke(list);
            return new AnyScriptType() { Value = result };
        }
    }

}
