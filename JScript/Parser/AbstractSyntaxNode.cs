namespace JScript.Parsers
{
    public interface ISyntaxNode
    {
        IScriptType Value(ScriptContext context);
    }

    public abstract class AbstractSyntaxNode<T> : ISyntaxNode where T : IScriptType
    {
        public abstract T Value(ScriptContext context);

        IScriptType ISyntaxNode.Value(ScriptContext context)
        {
            return this.Value(context);
        }
    }
    public enum NodeType
    {
        Declare,
        Assign,
        Operate,
        Function,
        Event,
        If,
        While,
        For,
        Foreach,
        None,
        Variable,
        Sequence,
        Return,
    }
}