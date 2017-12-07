using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JScript
{


    public class Parser
    {
        private readonly Lexer lexer;
        public Parser(string code)
        {
            this.lexer = new Lexer(code);
        }
        public SyntaxTree Parse()
        {
            SyntaxTree tree = new SyntaxTree();
            while (true)
            {
                var token = lexer.NextToken();
                switch (token.Type)
                {
                    #region Declare
                    case TokenType.Var:
                        tree.Children.Add(this.Declare(ScriptTypes.Any));
                        break;
                    case TokenType.Integer:
                        tree.Children.Add(this.Declare(ScriptTypes.Integer));
                        break;
                    case TokenType.String:
                        tree.Children.Add(this.Declare(ScriptTypes.String));
                        break;
                    case TokenType.Boolean:
                        tree.Children.Add(this.Declare(ScriptTypes.Boolean));
                        break;
                    case TokenType.Double:
                        tree.Children.Add(this.Declare(ScriptTypes.Double));
                        break;

                    #endregion

                    #region Assign
                    case TokenType.Word:
                        tree.Children.Add(this.Assign(token));
                        break;
                    #endregion

                    case TokenType.Return:
                        tree.Children.Add(this.Return());
                        break;


                    #region Function

                    case TokenType.Function:
                        tree.Children.Add(this.Function(ScriptTypes.Null));
                        break;

                    #endregion

                    #region While
                    case TokenType.While:
                        break;
                    #endregion

                    #region For
                    case TokenType.For:
                        break;
                    #endregion

                    #region Foreach
                    case TokenType.Foreach:
                        break;
                    #endregion

                    #region If
                    case TokenType.If:
                        break;
                    #endregion
                    default:
                        return tree;
                }
            }
        }
        private IASTNode Return()
        {
            var token = this.lexer.NextToken();
            return new VariableNode(token.Fragment.Text, ScriptTypes.Any);
        }

        private IASTNode Declare(ScriptTypes type)
        {
            var token = this.lexer.NextToken();
            if (token.Type == TokenType.ArrayLeft)
            {
                token = this.lexer.NextToken();
                if (token.Type != TokenType.ArrayRight)
                {
                    throw new Exception(token.ToString());
                }
                switch (type)
                {
                    case ScriptTypes.Integer:
                        type = ScriptTypes.ListInteger;
                        break;
                    case ScriptTypes.Double:
                        type = ScriptTypes.ListDouble;
                        break;
                    case ScriptTypes.String:
                        type = ScriptTypes.ListString;
                        break;
                    case ScriptTypes.Boolean:
                        type = ScriptTypes.ListBoolean;
                        break;
                    default:
                        throw new Exception(token.ToString());
                }
            }
            //得到name
            if (token.Type != TokenType.Word)
            {
                throw new Exception(token.ToString());
            }
            var name = token.Fragment.Text;
            var declareNode = new DeclareNode(type, name);
            token = this.lexer.NextToken();
            if (token.Type == TokenType.End)
            {
                return declareNode;
            }
            token = this.lexer.PrevToken();
            var assignNode = this.Assign(token);
            declareNode.Children.Add(assignNode);
            return declareNode;
        }
        private IASTNode Function(ScriptTypes type)
        {
            var token = this.lexer.NextToken();
            return null;
        }
        private IASTNode Assign(Token token)
        {
            var nextToken = this.lexer.NextToken();

            if (nextToken.Type != TokenType.Eq)
            {
                throw new Exception(token.ToString());
            }
            nextToken = this.lexer.NextToken();
            AssignNode node = new AssignNode(token.Fragment.Text, nextToken.Fragment.Text);
            return node;
        }
        private IASTNode Invoke(Token token, Token token2)
        {
            return null;
        }

        private IASTNode While()
        {
            return new WhileNode(null, null);
        }
    }
    public class SyntaxTree : ASTNode<object>
    {
        public override object Value()
        {
            object value = null;
            foreach (var item in this.Children)
            {
                value = item.ToString();
            }
            return value;
        }
    }
    public enum ASTNodeType
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
    }
    public abstract class ASTNode<T> : IASTNode
    {
        public IASTNode Left { get; set; }
        public ASTNodeType Type { get; set; }
        public IASTNode Right { get; set; }

        public List<IASTNode> Children { get; set; }
        public ASTNode()
        {
            this.Children = new List<IASTNode>();
        }

        public abstract object Value();
    }
    public class DeclareNode : ASTNode<ScriptTypes>
    {
        private ScriptTypes types;
        private string name;
        public DeclareNode(ScriptTypes types, string Name)
        {
            this.Type = ASTNodeType.Declare;
            this.Left = new SealedNode<string>(name);
            this.Right = null;
        }
        public override object Value()
        {
            return null;
        }
    }
    public class AssignNode : ASTNode<object>
    {
        public AssignNode(string name, object value)
        {
            this.Type = ASTNodeType.Assign;
            this.Left = new SealedNode<string>(name);
            this.Right = new SealedNode<object>(value);
        }
        public override object Value()
        {

            return null;
        }
    }

    public class WhileNode : ASTNode<object>
    {
        public WhileNode(ASTNode<bool> condition, ASTNode<object> block)
        {
            this.Type = ASTNodeType.While;
            this.Left = condition;
            this.Right = block;
        }
        public override object Value()
        {
            return null;
        }
    }
    public class VariableNode : ASTNode<string>
    {
        public VariableNode(string name, ScriptTypes types)
        {
            this.Type = ASTNodeType.Variable;
            this.Left = new SealedNode<string>(name);
            this.Right = new SealedNode<ScriptTypes>(types);
        }
        public override object Value()
        {
            return null;
        }
    }
    public sealed class SealedNode<T> : ASTNode<T>
    {
        private readonly T input;
        public SealedNode(T input)
        {
            this.Type = ASTNodeType.None;
            this.Left = null;
            this.Right = null;
            this.input = input;
        }
        public override object Value()
        {
            return this.input;
        }
    }
    public class OperateNode : ASTNode<decimal>
    {
        public OperateNode(IASTNode left, IASTNode right, TokenType operate)
        {
            this.Type = ASTNodeType.Operate;
            this.Left = left;
            this.Right = right;
        }
        public OperateNode(decimal left, decimal right, TokenType operate) : this(new SealedNode<decimal>(left), new SealedNode<decimal>(right), operate)
        {
        }
        public OperateNode(int left, int right, TokenType operate) : this(new SealedNode<int>(left), new SealedNode<int>(right), operate)
        {
        }
        public override object Value()
        {
            return null;
        }
    }
    public class FunctionNode : ASTNode<object>
    {
        public FunctionNode(string name, IASTNode right)
        {
            this.Type = ASTNodeType.Function;
            this.Left = new SealedNode<string>(name);
            this.Right = right;
        }
        public override object Value()
        {
            return null;
        }
    }
    public class InvokeNode : ASTNode<object>
    {
        public InvokeNode(string name, IASTNode right)
        {
            this.Type = ASTNodeType.Function;
            this.Left = new SealedNode<string>(name);
            this.Right = right;
        }
        public override object Value()
        {
            return null;
        }
    }
    public enum ScriptTypes
    {
        Any,
        Integer,
        Double,
        String,
        Boolean,
        ListInteger,
        ListDouble,
        ListString,
        ListBoolean,
        Regex,
        Null,
        Undefined,
        Void
    }
}
