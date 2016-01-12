using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Dipl
{
    class PDF
    {
        public static void CreatePDF(string namefile, string category)
        {
            //var Документ = new iTextSharp.text.Document();
            var Документ = new Document(); //создаем pdf документ iTextSharp.text.
            var Писатель = PdfWriter.GetInstance(Документ, new System.IO.FileStream(namefile, System.IO.FileMode.Create)); // в текущем каталоге, если файл есть - создаст новый
            Документ.SetPageSize(PageSize.A4.Rotate());
            Документ.AddAuthor("Безверхий О.А.");
            Документ.AddTitle("Отчёт");
            Документ.AddCreator("Программа учёта ТО и Ремонта");
            Документ.Open();
            //Rectangle rec2 = new Rectangle(PageSize.A4);
            //var БазовыйШрифт = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\comic.ttf", "CP1251", BaseFont.EMBEDDED);
            var БазовыйШрифт = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\times.ttf", "CP1251", BaseFont.EMBEDDED);
            var Шрифт = new Font(БазовыйШрифт, 12, iTextSharp.text.Font.NORMAL);

            Paragraph para = new Paragraph("КАРТА УЧЕТА ТЕХОБСЛУЖИВАНИЯ " + category.ToUpper() + "\n", Шрифт);
            para.Alignment = Element.ALIGN_CENTER;
            Документ.Add(para);

            para = new Paragraph("МАРКА: КЗС-1218-40 гос. № А283БВ 28rus инв. № 27" + "\n", Шрифт);
            para.Alignment = Element.ALIGN_LEFT;
            Документ.Add(para);

            para = new Paragraph("МЕХАНИЗАТОР: Вася Петя" + "\n\n", Шрифт);
            Документ.Add(para);

            /*para = new Paragraph(""+"\n", Шрифт);
            Документ.Add(para);*/

            var Tabla = new PdfPTable(6);
            Tabla.HorizontalAlignment = Element.ALIGN_CENTER; //по центру
            var Yacheuka = new PdfPCell(new Phrase("Ячейка", Шрифт));
            //Single[] ШиринаКолонок = { 10.0F, 30.0F, 30.0F, 30.0F };
            //Tabla.SetTotalWidth(ШиринаКолонок);
            //Yacheuka.FixedHeight = 20.0F; //высота ячейки
            //выравнивание содержимого ячеек
            Yacheuka.HorizontalAlignment = Element.ALIGN_LEFT;
            Yacheuka.VerticalAlignment = Element.ALIGN_MIDDLE;
            //Yacheuka.

            //создадим заголовок таблицы и заполним первую строку таблицы
            String[] НазванСтолб = { "ДАТА ТО", "ВИД ТО", "ПОКАЗАНИЯ М/Ч", "СЛЕДУЮЩ. ТО", "ПРОВЕЛ ТО МЕХАНИЗАТОР", "ОПЕРАЦИИ, МАТЕРИАЛЫ" };
            for (int i = 0; i <= 5; i++)
            {
                Yacheuka.Phrase = new Phrase(НазванСтолб[i], Шрифт);
                Tabla.AddCell(Yacheuka);
            }

            //строковые массивы
            String[] ДатаТО = { "1", "2", "3" };
            String[] ВидТО = { "Россия", "Белоруссия", "Китай" };
            String[] Показания = { "Россия", "Белоруссия 12312312312312312", "Китай" };
            String[] СледТО = { "Москва", "Минск", "Пекин" };
            String[] Провел = { "не знаю", "не знаю", "не знаю" };
            String[] Операции = { "не знаю", "не знаю", "не знаю" };

            //в цикле зафигачим каждую ячейку
            for (int i = 0; i <= 2; i++)
            {
                Yacheuka.Phrase = new Phrase(ДатаТО[i], Шрифт);
                Tabla.AddCell(Yacheuka);
                Yacheuka.Phrase = new Phrase(ВидТО[i], Шрифт);
                Tabla.AddCell(Yacheuka);
                Yacheuka.Phrase = new Phrase(Показания[i], Шрифт);
                Tabla.AddCell(Yacheuka);
                Yacheuka.Phrase = new Phrase(СледТО[i], Шрифт);
                Tabla.AddCell(Yacheuka);
                Yacheuka.Phrase = new Phrase(Провел[i], Шрифт);
                Tabla.AddCell(Yacheuka);
                Yacheuka.Phrase = new Phrase(Операции[i], Шрифт);
                Tabla.AddCell(Yacheuka);
            }
            Документ.Add(Tabla);
            Документ.Add(new iTextSharp.text.Paragraph("Текст после таблицы", Шрифт));
            Документ.Close();
            Писатель.Close();
            System.Diagnostics.Process.Start("Отчет.pdf");
        }
    }
}
