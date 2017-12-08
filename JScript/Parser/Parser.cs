using JScript.Lexers;
using JScript.Script;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JScript.Parsers
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
            while (this.lexer.NextToken())
            {
                var token = this.lexer.CurrentType;
                switch (token)
                {

                    #region Assign
                    case TokenType.Assign:
                        AssignNode assignNode = new AssignNode(this.lexer.Current[0].Fragment.Text, this.lexer.Current[2].Fragment.Text);
                        tree.Children.Add(assignNode);
                        break;
                    #endregion

                    case TokenType.Return:
                        VariableNode variableNode = new VariableNode(this.lexer.Current[0].Fragment.Text);
                        ReturnNode returnNode = new ReturnNode(variableNode);
                        tree.Children.Add(returnNode);
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
                        break;
                }
            }
            return tree;
        }
        private ISyntaxNode Return()
        {
            var token = this.lexer.NextToken();
            return new VariableNode(this.lexer.Current[1].Fragment.Text);
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

        private ISyntaxNode While()
        {
            return new WhileNode(null, null);
        }
    }
}