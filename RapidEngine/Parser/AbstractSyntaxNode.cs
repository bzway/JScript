namespace JScript.Parsers
{
    public interface ISyntaxNode
    {
        IScriptType Execute(ScriptContext context);
    }

    public abstract class AbstractSyntaxNode<T> : ISyntaxNode where T : IScriptType
    {
        public abstract T Execute(ScriptContext context);

        IScriptType ISyntaxNode.Execute(ScriptContext context)
        {
            return this.Execute(context);
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