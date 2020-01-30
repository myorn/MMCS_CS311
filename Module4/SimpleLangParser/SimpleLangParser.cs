﻿﻿using System;
using System.Collections.Generic;
using System.Text;
using SimpleLexer;
namespace SimpleLangParser
{
    public class ParserException : System.Exception
    {
        public ParserException(string msg)
            : base(msg)
        {
        }

    }

    public class Parser
    {
        private SimpleLexer.Lexer l;

        public Parser(SimpleLexer.Lexer lexer)
        {
            l = lexer;
        }

        public void Progr()
        {
            Block();
        }

        public void Expr() 
        {
            // здесь мы проверяем на выржение
            if (l.LexKind == Tok.ID || l.LexKind == Tok.INUM)
            {
                // если число, то дальше ожидаем плюс или минус
                l.NextLexem();
				if (l.LexKind == Tok.PLUS || l.LexKind == Tok.MINUS)
				{
					l.NextLexem();
                    // рекуррентно вызовем функию для того, чтобы продолжить проверку 
					Expr();
				}
            }
            else
            {
                SyntaxError("expression expected");
            }
        }

        public void Assign() 
        {
            l.NextLexem();  // пропуск id
            if (l.LexKind == Tok.ASSIGN)
            {
                l.NextLexem();
            }
            else {
                SyntaxError(":= expected");
            }
            Expr();
        }

		public void For()
		{
            // цикл for, выглядит как паскалевский
			if (l.LexKind != Tok.FOR)
			{
				SyntaxError("for expected");
			}
			l.NextLexem();
			Assign();
			if (l.LexKind != Tok.TO)
			{
				SyntaxError("to expected");
			}
			l.NextLexem();
			Expr();
			if (l.LexKind != Tok.DO)
			{
				SyntaxError("do expected");
			}
			l.NextLexem();
			if (l.LexKind == Tok.BEGIN)
			{
				Block();
			}
			else
			{
				Statement();
			}

		}

		public void StatementList() 
        {
            // список высказываний через запятую
            Statement();
            while (l.LexKind == Tok.SEMICOLON)
            {
                l.NextLexem();
                Statement();
            }
        }

        public void Statement() 
        {
            // высказывания разных типов
            switch (l.LexKind)
            {
                case Tok.BEGIN:
                    {
                        Block(); 
                        break;
                    }
                case Tok.CYCLE:
                    {
                        Cycle(); 
                        break;
                    }
                case Tok.ID:
                    {
                        Assign();
                        break;
                    }
				case Tok.FOR:
					{
						For();
						break;
					}
                default:
                    {
                        SyntaxError("Operator expected");
                        break;
                    }
            }
        }

        public void Block() 
        {
            // блок begin end
            l.NextLexem();    // пропуск begin
            StatementList();
            if (l.LexKind == Tok.END)
            {
                l.NextLexem();
            }
            else
            {
                SyntaxError("end expected");
            }

        }

        public void Cycle() 
        {
            // циклический обход
            l.NextLexem();  // пропуск cycle
            Expr();
            Statement();
        }

        public void SyntaxError(string message) 
        {
            var errorMessage = "Syntax error in line " + l.LexRow.ToString() + ":\n";
            errorMessage += l.FinishCurrentLine() + "\n";
            errorMessage += new String(' ', l.LexCol - 1) + "^\n";
            if (message != "")
            {
                errorMessage += message;
            }
            throw new ParserException(errorMessage);
        }
   
    }
}
