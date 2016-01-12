using System;
using ClosedXML.Excel;

namespace Dipl
{
    class Excel
    {
        void CreateExc()
        {
            XLWorkbook wb = new XLWorkbook(); //создаем книгу
            IXLWorksheet ws = wb.Worksheets.Add("Лист 1"); //добавляем листы

            ws.Cell("A1").Value = "КАРТА УЧЕТА ТЕХОБСЛУЖИВАНИЯ";
            ws.Cell("A3").Value = "МАРКА: ";
            ws.Cell("A5").Value = "Механ";
            //ws.Cell("A7").Value = "Дата ТО";


            wb.SaveAs("Otchet.xlsx"); //сохраняем в файл
        }
    }
}
