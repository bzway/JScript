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
                            case TokenType.OpenParen:
                                break;
                            case TokenType.CloseParen:
                                break;
                            case TokenType.OpenCurly:
                                break;
                            case TokenType.End:
                                break;
                            case TokenType.Not:
                                break;
                            case TokenType.And:
                                break;
                            case TokenType.Or:
                                break;
                            case TokenType.Xor:
                                break;
                            case TokenType.Word:
                                break;
                            case TokenType.Eq:
                                break;
                            case TokenType.Return:
                                break;
                            case TokenType.Assign:
                                break;
                            case TokenType.OpenSquare:
                                break;
                            case TokenType.CloseSquare:
                                break;
                            default:
                                break;
                        }
                        if (this.lexer.Current.Type== TokenType.OpenParen)
                        {

                        }

                    case TokenType.Assign:
                        var assignNode = this.StepAssgin();
                        tree.Children.Add(assignNode);
                        break;
                    #endregion

                    case TokenType.Return:
                        var returnNode = this.StepReturn();
                        tree.Children.Add(returnNode);
                        break;

                    #region Function

                    case TokenType.Function:
                        tree.Children.Add(this.Function(ScriptTypes.Null));
                        break;

                    #endregion

                    #region While
                    case TokenType.While:
                        var  whileNode = this.StepWhile();
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
        private ISyntaxNode StepAssgin()
        {
            var left = this.lexer.Current[0].Fragment.Text;
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