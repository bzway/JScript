using JScript.Lexers;
using JScript.Script;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JScript.Parsers
{
    public class Parser
    {
        private Dictionary<string, List<TokenDefinition>> tokenDefinitions = new Dictionary<string, List<TokenDefinition>>();

        private readonly Lexer lexer;
        public Parser(string code)
        {
            tokenDefinitions.Add("program",
                new List<TokenDefinition>()
                {
                    TokenDefinition.Assgin,
                    TokenDefinition.Function,
                    TokenDefinition.If,
                    TokenDefinition.For,
                });
            this.lexer = new Lexer(code);
        }
        public SyntaxTree Parse()
        {
            SyntaxTree tree = new SyntaxTree();
            foreach (var item in tokenDefinitions["program"])
            {
                if ()
                {

                }
            }
            return tree;
        }

        private ISyntaxNode[] StepParameter()
        {
            throw new NotImplementedException();
        }

        private ISyntaxNode StepAssgin()
        {
            var left = this.lexer.Current.Fragment.Text;
            OperateType opt = this.StepOperate();
            ISyntaxNode right = this.StepExpression();
            return new AssignNode(left, opt, right);
        }

        private ISyntaxNode StepExpression()
        {
            throw new NotImplementedException();
        }

        private OperateType StepOperate()
        {
            OperateType operate = OperateType.None;
            while (this.lexer.NextToken())
            {
                switch (this.lexer.Current.Type)
                {
                    case TokenType.OpreationAdd:
                        var next = this.StepOperate();
                        switch (next)
                        {
                            case OperateType.Add:
                                return OperateType.AddAdd;
                            case OperateType.None:
                                return OperateType.Add;
                            case OperateType.Assign:
                                break;
                            case OperateType.Invoke:
                                break;
                            default:
                                break;
                        }

                        break;
                    case TokenType.OpreationSub:
                        break;
                    case TokenType.OpreationMul:
                        break;
                    case TokenType.OpreationDiv:
                        break;
                    case TokenType.OpreationMod:
                        break;
                    case TokenType.Not:
                        break;
                    case TokenType.And:
                        break;
                    case TokenType.Or:
                        break;
                    case TokenType.Xor:
                        break;
                    case TokenType.OpenParen:
                        operate = OperateType.Invoke;
                        break;
                    case TokenType.CloseParen:
                        break;
                    case TokenType.End:
                        break;
                    case TokenType.Equal:
                        break;
                    case TokenType.OpenSquare:
                        break;
                    case TokenType.CloseSquare:
                        break;
                    case TokenType.Dot:
                        break;
                    case TokenType.Comma:
                        break;
                    default:
                        break;
                }
            }
            throw new NotImplementedException();
        }

        private ISyntaxNode StepReturn()
        {
            var expressionNode = this.StepExpression();
            ReturnNode returnNode = new ReturnNode(expressionNode);
            return returnNode;
        }

        private ISyntaxNode Function(ScriptTypes type)
        {
            var token = this.lexer.NextToken();
            return null;
        }

        private ISyntaxNode Invoke(Token token, Token token2)
        {
            return null;
        }

        private ISyntaxNode StepIf()
        {
            var condition = (AbstractSyntaxNode<BooleanScriptType>)this.StepExpression();
            var blockTrue = this.Parse();

            var blockFalse = this.Parse();

            return new IfNode(condition, new SequenceNode(blockTrue.Children.ToArray()), new SequenceNode(blockFalse.Children.ToArray()));
        }
        private ISyntaxNode StepWhile()
        {
            var condition = (AbstractSyntaxNode<BooleanScriptType>)this.StepExpression();
            var block = this.Parse();
            return new WhileNode(condition, new SequenceNode(block.Children.ToArray()));
        }
        private ISyntaxNode StepFor()
        {
            return new WhileNode(null, null);
        }
    }
}