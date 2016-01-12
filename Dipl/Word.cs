using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Novacode;

namespace Dipl
{
    class Word
    {
        public static void Otchet(string tip, string comboBox1, string comboBox2, string label2, DataTable dt)
        {
            string namefile = "";
            string headertext = "";

            if (tip == "ТО")
            {
                namefile = comboBox1 + "-ТО.docx";
            }
            if (tip == "Ремонт")
            {
                namefile = comboBox1 + "-Рем.docx";
            }

            var doc = DocX.Create(namefile);

            if (tip == "ТО")
            {
                //doc.InsertParagraph("КАРТА УЧЕТА ТЕХОБСЛУЖИВАНИЯ" + comboBox2.ToUpper());
                headertext = "КАРТА УЧЕТА ТЕХОБСЛУЖИВАНИЯ" + comboBox2.ToUpper();
            }
            if (tip == "Ремонт")
            {
                //doc.InsertParagraph("КАРТА УЧЕТА РЕМОНТА " + comboBox2.ToUpper());
                headertext = "КАРТА УЧЕТА РЕМОНТА " + comboBox2.ToUpper();
            }

            Paragraph title = doc.InsertParagraph(headertext);
            title.Alignment = Alignment.center;
            doc.InsertParagraph("");

            //рабиваем на две подстроки
            string words = label2;
            string[] split = words.Split(new Char[] { '\n' });
            //забиваем дальше
            doc.InsertParagraph(split[0]);
            doc.InsertParagraph("");
            doc.InsertParagraph(split[1]);
            doc.InsertParagraph("");
            //мутим таблицу
            int row = dt.Rows.Count + 1;
            //Console.WriteLine(row);
            int columns = dt.Columns.Count;
            Table table = doc.AddTable(row,columns);
            table.Alignment = Alignment.center;

            //выводим названия столбцов
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                table.Rows[0].Cells[i].Paragraphs.First().Append(dt.Columns[i].ToString());
            }
            //int strok = row - 1;
            //выводим значения таблицы
            for (int b = 0; b < dt.Rows.Count; b++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    int s = b + 1;
                    //Console.WriteLine("[{0}][{1}]", b, i);
                    table.Rows[s].Cells[i].Paragraphs.First().Append(dt.Rows[b][dt.Columns[i]].ToString());
                }
            }

            doc.InsertTable(table);

            doc.InsertParagraph("");
            doc.InsertParagraph("Подпись руководителя: ");
            doc.Save();//сохраняем в файл
            System.Diagnostics.Process.Start(namefile);
        }
    }
}
