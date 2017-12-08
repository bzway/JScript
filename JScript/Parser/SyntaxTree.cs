using System.Collections.Generic;

namespace JScript.Parsers
{
    public class SyntaxTree : AbstractSyntaxNode<IScriptType>
    {
        public List<ISyntaxNode> Children { get; set; }
        public SyntaxTree()
        {
            this.Children = new List<ISyntaxNode>();
        }
        public override IScriptType Value(ScriptContext context)
        {
            IScriptType result = new VoidScriptType();
            foreach (var item in this.Children)
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

}