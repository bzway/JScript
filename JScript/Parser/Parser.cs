using JScript.Lexers;
using JScript.Script;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JScript.Parsers
{
    public class Parser
    {
        private static Regex regexIdentifier = new Regex("^[_a-zA-Z][_a-zA-Z0-9]*");

        private readonly Lexer lexer;
        public Parser(string code)
        {
            this.lexer = new Lexer(code);
        }
        public SyntaxTree Parse()
        {
            SyntaxTree tree = new SyntaxTree();
            while (this.lexer.NextToken())
            {
                var token = this.lexer.Current;
                switch (token.Type)
                {

                    #region Assign
                    case TokenType.Word:
                        if (!regexIdentifier.IsMatch(token.Fragment.Text))
                        {
                            throw new ScriptException("", token);
                        }
                        //得到下一个token判断是变量还是函数
                        if (!this.lexer.NextToken())
                        {
                            throw new ScriptException("", token);
                        }

                        switch (this.lexer.Current.Type)
                        {
                            case TokenType.Dot://对象引用
                                break;
                            case TokenType.OpenParen://函数引用
                                ISyntaxNode[] parameters = this.StepParameter();
                                ISyntaxNode right = new InvokeNode(token.Fragment.Text, parameters);
                                tree.Children.Add(right);
                                break;
                            case TokenType.OpenSquare://数组引用
                                break;
                            case TokenType.Equal://赋值
                                var node = new AssignNode(token.Fragment.Text, OperateType.Assign, this.StepExpression());
                                break;
                            default:
                                break;
                        }
                        if (this.lexer.Current.Type == TokenType.OpenParen)
                        {

                        }
                        break;

                    #endregion

                    #region Return
                    case TokenType.Return:
                        var returnNode = this.StepReturn();
                        tree.Children.Add(returnNode);
                        break;
                    #endregion

                    #region Function

                    case TokenType.Function:
                        tree.Children.Add(this.Function(ScriptTypes.Null));
                        break;

                    #endregion

                    #region While
                    case TokenType.While:
                        var whileNode = this.StepWhile();
                        tree.Children.Add(whileNode);
                        break;
                    #endregion

                    #region For
                    case TokenType.For:
                        var forNode = this.StepFor();
                        tree.Children.Add(forNode);
                        break;
                    #endregion

                    #region Foreach
                    case TokenType.Foreach:
                        break;
                    #endregion

                    #region If
                    case TokenType.If:
                        var ifNode = this.StepIf();
                        tree.Children.Add(ifNode);
                        break;
                    #endregion
                    default:
                        break;
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
            return new WhileNode(null, null);
        }
        private ISyntaxNode StepWhile()
        {
            return new WhileNode(null, null);
        }
        private ISyntaxNode StepFor()
        {
            return new WhileNode(null, null);
        }
    }
}