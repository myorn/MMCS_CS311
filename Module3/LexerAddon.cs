using System;
using System.IO;
using SimpleScanner;
using ScannerHelper;
using System.Collections.Generic;

namespace  GeneratedLexer
{
    
    public class LexerAddon
    {
        public Scanner myScanner;
        private byte[] inputText = new byte[255];

        public int idCount = 0;
        public int minIdLength = Int32.MaxValue;
        public double avgIdLength = 0;
        public int maxIdLength = 0;
        public int sumInt = 0;
        public double sumDouble = 0.0;
        public List<string> idsInComment = new List<string>();
        

        public LexerAddon(string programText)
        {
            
            using (StreamWriter writer = new StreamWriter(new MemoryStream(inputText)))
            {
                writer.Write(programText);
                writer.Flush();
            }
            
            MemoryStream inputStream = new MemoryStream(inputText);
            
            myScanner = new Scanner(inputStream);
        }

        public void Lex()
        {
            // ����� ������������ ����� �������������� � ������������ � ������� 3.14 (� �� 3,14 ��� � ������� Culture)
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            int tok = 0;
			double sumIdLength = 0;
            do {
                tok = myScanner.yylex();
				if (tok == (int)Tok.EOF)
				{
					break;
				}
				else if (tok == (int)Tok.ID)
				{
                    // считаем сколько переменных встретилось
					++idCount;
					String id = myScanner.yytext;
                    // считаем их суммарную длину 
					sumIdLength += id.Length;
                    // находим минимум и максимум длины
					if (id.Length < minIdLength)
					{
						minIdLength = id.Length;
					}
					if (id.Length > maxIdLength)
					{
						maxIdLength = id.Length;
					}
				}
                // суммируем целые числа
				else if (tok == (int)Tok.INUM)
				{
					sumInt += myScanner.LexValueInt;
				}
                // суммируем числа с плавающей точкой
				else if (tok == (int)Tok.RNUM)
				{
					sumDouble += myScanner.LexValueDouble;
				}
			} while (true);
            // вычисляем среднюю длину названия переменной
			avgIdLength = sumIdLength / idCount;
        }
    }
}

