using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;

namespace laba1
{
    internal class Parser
    {
        static int index = 0;
        static Lexeme nx = null;
        static List<Lexeme> lexemes = new List<Lexeme>();
        static public string result = "";

        public Parser(List<Lexeme> lex)
        {
           lexemes = lex;

        }

        static void Stmt()
        {

            if (nx != null && nx.Type == LexemeType.If)
            {
                result += "if ";
                Scan();
                if (nx != null && nx.Type != LexemeType.SkobaOpen)
                    Error();
                else if (nx == null)
                {
                    result += "";
                }
                else
                {
                    result += "'(' ";
                    Scan();
                    Expr();
                    if (nx != null && nx.Type != LexemeType.SkobaClose)
                        Error();
                    else if (nx == null)
                    {
                        result += "";
                    }
                    else
                    {
                        result += "')' ";
                        Scan();
                        result += "STMT ";
                        if (nx == null || nx.Type == LexemeType.Else)
                        {
                            result += "ε ";
                        }

                        Stmt();
                        if (nx != null && nx.Type == LexemeType.Else)
                        {
                            result += "else ";
                            Scan();
                            result += "STMT ";
                            if (nx == null)
                            {
                                result += "ε ";
                            }

                            Stmt();
                        }
                        else if (nx == null)
                        {
                            result += "";
                        }
                    }
                }
            }
            else if (nx == null)
            {
                result += "";
            }

        }


        static void Expr()
        {
            result += "EXPR ";
            Variable();
            Operator();
            Value();
        }

        static void Variable()
        {
            if (nx != null && nx.Type != LexemeType.Letter)
                Error();
            else if (nx == null)
            {
                result += "";
            }
            else
            {

                result += "VARIABLE ";
                Scan();
            }

        }

        static void Operator()
        {
            if (nx != null && nx.Type != LexemeType.Operator)
                Error();
            else if (nx == null)
            {
                result += "";
            }
            else
            {
                result += "OPERATOR ";
                Scan();
            }
        }

        static void Value()
        {
            if (nx != null && nx.Type != LexemeType.Number)
                Error();
            else if (nx == null)
            {
                result += "";
            }
            else
            {
                result += "VALUE ";
                Scan();
            }
        }

        static void Error()
        {
            Console.WriteLine("Ошибка синтаксиса");
        }

        static void Scan()
        {
            if (index < lexemes.Count)
            {
                // Пропускаем пробелы
                while (index < lexemes.Count && lexemes[index].Type == LexemeType.Delimiter)
                {
                    index++;
                }

                if (index < lexemes.Count)
                {
                    nx = lexemes[index];
                    index++;
                }
                else
                {
                    nx = null;
                    return;// Конец входного выражения
                }
            }
            else
            {
                nx = null;
                return;// Конец входного выражения
            }
        }

        public void CheckInput(DataGridView dataGridView1)
        {
            result = "";

            Scan();  // Начинаем сканирование с первого символа
            result += "STMT ";
            Stmt();  // Начинаем анализ с начального символа грамматики (STMT)

            //if (nx != null)  // Если после анализа остались символы, то входное выражение некорректно
            //    Console.WriteLine("Ошибка: некорректное входное выражение");
            //else
            //    Console.WriteLine("Входное выражение корректно");

            //Console.WriteLine(result);
            dataGridView1.Rows.Add(result);

            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

        }

    }
}
