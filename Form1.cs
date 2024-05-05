﻿using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.Timer;

namespace laba1
{
    public partial class Form1 : Form
    {
        private string currentFilePath = null;

        private ToolStripLabel dateLabel;
        private ToolStripLabel timeLabel;
        private ToolStripLabel layoutLabel;

        List<Lexeme> lexemes = new List<Lexeme>();

        private void UpdateStatusLabels(object sender, EventArgs e)
        {
            dateLabel.Text = "" + DateTime.Now.ToLongDateString();
            timeLabel.Text = "" + DateTime.Now.ToLongTimeString();

            var currentLayout = InputLanguage.CurrentInputLanguage.LayoutName;
            layoutLabel.Text = "Раскладка: " + currentLayout;
        }
        public void Back()
        {
           // FastColoredTextBox tb = inputTextBox;
           
            //if (inputTextBox.UndoEnabled)
                inputTextBox.Undo();
            
        }
        public void Next()
        {
            //FastColoredTextBox tb = inputTextBox;
            //if (inputTextBox.RedoEnabled)
                inputTextBox.Redo();
        }
        public void In() { inputTextBox.Paste(); }
        public void Copy() { if (inputTextBox.SelectionLength > 0) { inputTextBox.Copy(); } }
        public void Cut() { if (inputTextBox.SelectionLength > 0) { inputTextBox.Cut(); } }
        public void Help() { MessageBox.Show("Описание функций меню\r\n\r\nФайл - производит действия с файлами\r\n\r\nСоздать - создает файл\r\nОткрыть - открывает файл\r\nСохранить - сохраняет изменения в файле\r\nСохранить как - сохраняет изменения в новый файл\r\nВыход - осуществляет выход из программы\r\n\r\nПравка - осуществляет изменения в файле\r\n\r\nОтменить - отменяет последнее изменение\r\nПовторить - повторяет последнее действие\r\nВырезать - вырезает выделенный фрагмент\r\nКопировать - копирует выделенный фрагмент\r\nВставить - вставляет выделенный фрагмент\r\nУдалить - удаляет выделенный фрагмент\r\nВыделить все - выделяет весь текст\r\n\r\nСправка - показывает справочную информацию\r\n\r\nВызов справки - описывает функции меню\r\nО программе - содержит информацию о программе", "Справка"); }
        public void About() { MessageBox.Show("Данная программа является результатом первой лабораторной работы Разработка пользовательского интерфейса (GUI) для языкового процессора по дисциплине Теория формальных языков и компиляторов. Целью работы было Разработать приложение – текстовый редактор. В дальнейшем он будет дополнен функциями языкового процессора.", "О программе"); }
        public void Create()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, "");
                currentFilePath = saveFileDialog.FileName;
            }
        }
        public void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                inputTextBox.Text = File.ReadAllText(openFileDialog.FileName);
                currentFilePath = openFileDialog.FileName;
            }
        }
        public void Save()
        {
            if (currentFilePath != null)
            {
                File.WriteAllText(currentFilePath, inputTextBox.Text);

            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, inputTextBox.Text);
                    currentFilePath = saveFileDialog.FileName;
                }
            }
        }
        private void Form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        private void Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string filePath in filePaths)
            {

                string fileContent = File.ReadAllText(filePath);

                inputTextBox.AppendText(fileContent + Environment.NewLine);
            }
        }
        public Form1()
        {
            InitializeComponent();

            this.AllowDrop = true;
            this.DragEnter += Form_DragEnter;
            this.DragDrop += Form_DragDrop;

            dateLabel = new ToolStripLabel();
            dateLabel.Text = "";
            timeLabel = new ToolStripLabel();
            timeLabel.Text = "";
            layoutLabel = new ToolStripLabel();

            statusStrip1.Items.Add(dateLabel);
            statusStrip1.Items.Add(timeLabel);
            statusStrip1.Items.Add(layoutLabel);

            var timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += UpdateStatusLabels;
            timer.Start();

            
            //label1.Visible = true;
            dataGridView2.Visible = true;
            dataGridView1.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip t1 = new System.Windows.Forms.ToolTip();
            t1.SetToolTip(buttonCreate, "Создать");

            System.Windows.Forms.ToolTip t2 = new System.Windows.Forms.ToolTip();
            t2.SetToolTip(buttonOpen, "Открыть");

            System.Windows.Forms.ToolTip t3 = new System.Windows.Forms.ToolTip();
            t3.SetToolTip(buttonSave, "Сохранить");

            System.Windows.Forms.ToolTip t4 = new System.Windows.Forms.ToolTip();
            t4.SetToolTip(buttonCopy, "Копировать");

            System.Windows.Forms.ToolTip t5 = new System.Windows.Forms.ToolTip();
            t5.SetToolTip(buttonCut, "Вырезать");

            System.Windows.Forms.ToolTip t6 = new System.Windows.Forms.ToolTip();
            t6.SetToolTip(buttonIn, "Вставить");

            System.Windows.Forms.ToolTip t7 = new System.Windows.Forms.ToolTip();
            t7.SetToolTip(buttonBack, "Отменить");

            System.Windows.Forms.ToolTip t8 = new System.Windows.Forms.ToolTip();
            t8.SetToolTip(buttonNext, "Повторить");

            System.Windows.Forms.ToolTip t9 = new System.Windows.Forms.ToolTip();
            t9.SetToolTip(buttonInfo, "О программе");

            System.Windows.Forms.ToolTip t10 = new System.Windows.Forms.ToolTip();
            t10.SetToolTip(buttonHelp, "Вызов справки");

            System.Windows.Forms.ToolTip t11 = new System.Windows.Forms.ToolTip();
            t11.SetToolTip(buttonPlay, "Пуск");
        }
        private void buttonHelp_Click(object sender, EventArgs e) { Help(); }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e) { Help(); }
        private void buttonInfo_Click(object sender, EventArgs e) { About(); }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) { About(); }
        private void buttonCopy_Click(object sender, EventArgs e) { Copy(); }
        private void buttonCut_Click(object sender, EventArgs e) { Cut(); }
        private void buttonIn_Click(object sender, EventArgs e) { In(); }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e) { Cut(); }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e) { Copy(); }
        private void inToolStripMenuItem_Click(object sender, EventArgs e) { In(); }
        private void buttonBack_Click(object sender, EventArgs e) { Back(); }
        private void buttonNext_Click(object sender, EventArgs e)
        {

            Next();
        }
        private void backToolStripMenuItem_Click(object sender, EventArgs e) { Back(); }
        private void nextToolStripMenuItem_Click(object sender, EventArgs e) { Next(); }
        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputTextBox.SelectedText != "")
            {
                inputTextBox.Text = inputTextBox.Text.Remove(inputTextBox.SelectionStart, inputTextBox.SelectionLength);
            }
        }
        private void allToolStripMenuItem_Click(object sender, EventArgs e) { inputTextBox.SelectAll(); }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();

        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e) { Open(); }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, inputTextBox.Text);
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) { Save(); }
        private void createToolStripMenuItem_Click(object sender, EventArgs e) { Create(); }
        private void buttonCreate_Click(object sender, EventArgs e) { Create(); }
        private void buttonOpen_Click(object sender, EventArgs e) { Open(); }
        private void buttonSave_Click(object sender, EventArgs e) { Save(); }
        private void SplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // Обновление размеров элементов управления при изменении размера панелей
            //inputTextBox.Width = splitContainer1.Panel1.Width;
            //inputTextBox.Height = splitContainer1.Panel1.Height;

            //outputTextBox.Width = splitContainer1.Panel2.Width;
            //outputTextBox.Height = splitContainer1.Panel2.Height;
        }
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Сохранить изменения перед выходом?", "Подтверждение", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                Save();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            //Application.Exit();
        }

        void start()
        {

                string input = inputTextBox.Text;

            

            Dictionary<LexemeType, int> lexemeCodes = new Dictionary<LexemeType, int>()
            {
                { LexemeType.If, 1 },//
                { LexemeType.Else, 2 },//
                { LexemeType.Letter, 3 },//
                { LexemeType.Number, 4 },//
                { LexemeType.Operator, 5 },
                { LexemeType.SkobaOpen, 6 },//
                { LexemeType.SkobaClose, 7 },//
                { LexemeType.Delimiter, 8 },//
                { LexemeType.Invalid, 404 }//
            };

            string[] ifs = { "if" };
            string[] elses = { "else" };
            string[] operators = { "==", "<=", ">=", "<", ">", "!=" };
            string[] skobaopens = { "(" };
            string[] skobacloses = { ")" };
            string[] delimiters = { " " };




            int position = 0;
            while (position < input.Length)
            {
                bool found = false;

                //if
                foreach (string op in ifs)
                {
                    if (input.Substring(position).StartsWith(op))
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.If], LexemeType.If, input, position, position + op.Length - 1));
                        position += op.Length;
                        found = true;
                        break;
                    }
                }

                if (found) continue;

                //else
                foreach (string op in elses)
                {
                    if (input.Substring(position).StartsWith(op))
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Else], LexemeType.Else, input, position, position + op.Length - 1));
                        position += op.Length;
                        found = true;
                        break;
                    }
                }

                if (found) continue;

                //=
                foreach (string op in operators)
                {
                    if (input.Substring(position).StartsWith(op))
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Operator], LexemeType.Operator, input, position, position + op.Length - 1));
                        position += op.Length;
                        found = true;
                        break;
                    }
                }

                if (found) continue;

                //(
                foreach (string op in skobaopens)
                {
                    if (input.Substring(position).StartsWith(op))
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.SkobaOpen], LexemeType.SkobaOpen, input, position, position + op.Length - 1));
                        position += op.Length;
                        found = true;
                        break;
                    }
                }

                if (found) continue;

                //)
                foreach (string op in skobacloses)
                {
                    if (input.Substring(position).StartsWith(op))
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.SkobaClose], LexemeType.SkobaClose, input, position, position + op.Length - 1));
                        position += op.Length;
                        found = true;
                        break;
                    }
                }

                if (found) continue;

                //_
                foreach (string delimiter in delimiters)
                {
                    if (input.Substring(position).StartsWith(delimiter))
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Delimiter], LexemeType.Delimiter, input, position, position + delimiter.Length - 1));
                        position += delimiter.Length;
                        found = true;
                        break;
                    }
                }

                if (found) continue;

                //letter
                if (char.IsLetter(input[position]))
                {
                    int start = position;
                    while (position < input.Length && char.IsLetter(input[position]))
                    {
                        position++;
                    }
                    string identifier = input.Substring(start, position - start);
                    lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Letter], LexemeType.Letter, input, start, position - 1));
                }
                //value
                else if (char.IsDigit(input[position]))
                {
                    int start = position;
                    while (position < input.Length && char.IsDigit(input[position]))
                    {
                        position++;
                    }
                    string number = input.Substring(start, position - start);
                    lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Number], LexemeType.Number, input, start, position - 1));
                }
                //error
                else
                {
                    string invalid = input[position].ToString();
                    lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Invalid], LexemeType.Invalid, input, position, position));
                    position++;
                }
            }

            dataGridView2.Rows.Clear();//*
            dataGridView1.Rows.Clear();//*

            foreach (Lexeme lexeme in lexemes)
            {
                dataGridView1.Rows.Add(lexeme.Code, lexeme.Type, lexeme.Token, lexeme.StartPosition, lexeme.EndPosition);
            }

            Parser parser = new Parser(lexemes);

            
            parser.CheckInput(dataGridView2);

        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {

            start();                              
        }

        private void пускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start();
        }

        private void постановкаЗадачиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Тема работы: Объявление целочисленной константы с инициализацией на языке Rust\r\n\r\nОсобенности языка: \r\nКонстанты – это элементы данных, значения которых известны и в процессе выполнения программы не изменяются.\r\nДля описания констант в языке Rust используется служебное слово const.\r\nФормат записи: const имя_константы:тип_данных=значение;.\r\n\r\n Примеры верных строк из языка:\r\n 1. const abc:i32 = 123; \r\n 2. const bcd:i32=123; \r\n3. const cde:i32 = -123;", "Постановка задачи");

        }

        private void грамматикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Разработка грамматики\r\nОпределим грамматику целочисленных констант языка RUST G[‹Def›] в нотации Хомского с продукциями P:\r\n1) DEF -> ‘const’ CONST\r\n2) CONST -> ‘_’ ID\r\n3) ID ->letter IDREM\r\n4) IDREM -> letter IDREM\r\n5) IDREM -> ‘:’ TYPE\r\n6) TYPE -> ‘i32’ EQUAL\r\n7) EQUAL -> ‘=’ NUM\r\n8) NUM -> [+ | -] NUMBER\r\n9) NUMBER -> digit NUMBERREM\r\n10) NUMBERREM -> digit NUMBERREM\r\n11) NUMBERREM -> ;\r\n•\t‹Digit› → “0” | “1” | “2” | “3” | “4” | “5” | “6” | “7” | “8” | “9”\r\n•\t‹Letter› → “a” | “b” | “c” | ... | “z” | “A” | “B” | “C” | ... | “Z”\r\nСледуя введенному формальному определению грамматики, представим G[‹Def›] ее составляющими:\r\n•\tZ = ‹Def›;\r\n•\tVT = {a, b, c, ..., z, A, B, C, ..., Z, _, =, +, -, ;, ., 0, 1, 2, ..., 9};\r\n•\tVN = {DEF, CONST, ID, IDREM, TYPE, EQUAL, NUM, NUMBER, NUMBERREM}.\r\n", "Грамматика");
        }

        private void классификацияГрамматикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Согласно классификации Хомского, грамматика G[‹Def›] является автоматной.\r\nПравила (1)-(11) относятся к классу праворекурсивных продукций (A → aB | a | ε):\r\n1) DEF -> ‘const’ CONST\r\n2) CONST -> ‘_’ ID\r\n3) ID ->letter IDREM\r\n4) IDREM -> letter IDREM\r\n5) IDREM -> ‘:’ TYPE\r\n6) TYPE -> ‘i32’ EQUAL\r\n7) EQUAL -> ‘=’ NUM\r\n8) NUM -> [+ | -] NUMBER\r\n9) NUMBER -> digit NUMBERREM\r\n10) NUMBERREM -> digit NUMBERREM\r\n11) NUMBERREM -> ;\r\n", "Классификация грамматики");
        }

        private void методАнализаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Граф автоматной грамматики", "Метод анализа");
        }

        private void диагностикаИНейтрализацияОшибокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("По методу Айронса", "Диагностика и нейтрализация ошибок");
        }

        private void тестовыйПримерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("", "Тестовый пример");
            Process.Start("test.html");
        }

        private void списокЛитературыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. Шорников Ю.В. Теория и практика языковых процессоров : учеб. пособие / Ю.В. Шорников. – Новосибирск: Изд-во НГТУ, 2004.\r\n2. Gries D. Designing Compilers for Digital Computers. New York, Jhon Wiley, 1971. 493 p.\r\n3. Теория формальных языков и компиляторов [Электронный ресурс] / Электрон. дан. URL: https://dispace.edu.nstu.ru/didesk/course/show/8594, свободный. Яз.рус. (дата обращения 01.04.2021).\r\n", "Список литературы");
        }

        private void исходныйКодПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("", "Исходный код программы");
            Process.Start("listing.html");
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}