using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ClosedXML.Excel;

namespace Dipl
{
    public partial class TO : Form
    {
        //string login;

        public TO()
        {
            InitializeComponent();
            combocat();
            combovid();
            combowho();
            radioButton1.Checked=true;
            radioButton3.Checked = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //category = comboBox1.Text;
            MessageBox.Show(comboBox2.Text);
            fill_combo(comboBox2.Text);
            label2.Text = "Информация";
            dataGridView1.DataSource = new object();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label2.Text=LabelCar(comboBox1.Text);
            if (radioButton1.Checked) TabTO(comboBox1.Text);
            if (radioButton2.Checked) TabRem(comboBox1.Text);
        }

        void combocat()
        {
            string Query = "SELECT * FROM tipavto";
            try
            {
                SQLiteCommand command = new SQLiteCommand(Query, DB.Con);
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string name = dr.GetString(1);
                    comboBox2.Items.Add(name);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void combovid()
        {
            string Query = "SELECT * FROM work";
            try
            {
                SQLiteCommand command = new SQLiteCommand(Query, DB.Con);
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string name = dr.GetString(1);
                    comboBoxVid.Items.Add(name);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void combowho()
        {
            string Query = "SELECT * FROM personal ORDER BY fio ASC";
            try
            {
                SQLiteCommand command = new SQLiteCommand(Query, DB.Con);
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string name = dr.GetString(1);
                    comboBoxWho.Items.Add(name);
                    comboVodSea.Items.Add(name);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void fill_combo(string selectcat)
        {
            comboBox1.Items.Clear();
            string Query = "SELECT nomer FROM avto WHERE id_type IN (SELECT id FROM tipavto WHERE nametip='"+ selectcat +"')";
            try
            {
                SQLiteCommand command = new SQLiteCommand(Query, DB.Con);
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string name = dr.GetString(0);
                    comboBox1.Items.Add(name);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        static string LabelCar(string nomer) //в DB его
        {
            string Query = "SELECT marka, invent, id_vod, id_vod2 FROM avto WHERE nomer='" + nomer + "'";
            string stroka1="";
            string vod1="", vod2="", stroka2="";
            try
            {    
                SQLiteCommand command = new SQLiteCommand(Query, DB.Con);
                SQLiteDataReader dr = command.ExecuteReader();
                foreach (DbDataRecord record in dr)
                {
                    string marka = record["marka"].ToString();
                    string invt = record["invent"].ToString();
                    vod1 = record["id_vod"].ToString();
                    vod2 = record["id_vod2"].ToString();
                    stroka2 = "Марка: "+marka +" гос. № " +nomer+ " инв. №: " + invt + "\n";
                }
                string Query1 = "SELECT fio FROM personal WHERE id='" + vod1 + "'";
                command = new SQLiteCommand(Query1, DB.Con);
                dr = command.ExecuteReader();
                foreach (DbDataRecord record in dr)
                {
                    vod1 = record["fio"].ToString();
                }

                Query1 = "SELECT fio FROM personal WHERE id='" + vod2 + "'";
                command = new SQLiteCommand(Query1, DB.Con);
                dr = command.ExecuteReader();
                foreach (DbDataRecord record in dr)
                {
                    vod2 = record["fio"].ToString();
                }
                if (vod2 == "" || vod1 == "") stroka1 = stroka2 + "Водитель: " + vod1 + vod2;
                else stroka1 = stroka2 + "Механизаторы : " + vod1 + " " + vod2;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return stroka1;
        }

        void TabRem(string nomer)
        {
            string Query = "SELECT date 'Дата ремонта', id_work 'Вид ремонта', pokazaniya 'Показания КМ', id_person 'Провел ремонт', operation 'Операция, материалы' FROM otchetrem WHERE id_avto='" + nomer + "' ORDER BY date";
            SQLiteCommand cmdDataBase = new SQLiteCommand(Query, DB.Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = cmdDataBase;
            DataTable dbdataset = new DataTable();
            sda.Fill(dbdataset);
            BindingSource bSource = new BindingSource();

            bSource.DataSource = dbdataset;
            dataGridView1.DataSource = bSource;
            sda.Update(dbdataset);
        }

        void TabTO(string nomer)
        {
            string Query = "SELECT date 'Дата ТО', id_work 'Вид ТО', pokazaniya 'Показания КМ', nextto 'Следующий ТО', id_person 'Провел ТО', operation 'Операция, материалы' FROM otchetto WHERE id_avto='" + nomer + "' ORDER BY date";
            SQLiteCommand cmdDataBase = new SQLiteCommand(Query, DB.Con);
            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            sda.SelectCommand = cmdDataBase;
            DataTable dbdataset = new DataTable();
            sda.Fill(dbdataset);
            BindingSource bSource = new BindingSource();

            bSource.DataSource = dbdataset;
            dataGridView1.DataSource = bSource;
            sda.Update(dbdataset);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            labelnext.Hide();
            numericNext.Hide();
            labeldata.Text = "Дата ремонта";
            lablevid.Text = "Вид ремонта";
            labelwho.Text = "Провёл ремонт";
            label1.Text = "Карта учёта ремонта ";//+ Form1.selectcat
            TabRem(comboBox1.Text);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            labelnext.Show();
            numericNext.Show();
            labeldata.Text = "Дата ТО";
            lablevid.Text = "Вид ТО";
            labelwho.Text = "Провёл ТО";
            label1.Text = "Карта учёта ТО ";// +Form1.selectcat;
            TabTO(comboBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) otcTO();
            else otcRem();
        }

        void otcTO()
        {
            if (String.IsNullOrEmpty(comboBox1.Text) && String.IsNullOrEmpty(dateTimePicker1.Text) && String.IsNullOrEmpty(comboBoxVid.Text) && String.IsNullOrEmpty(comboBoxWho.Text) && String.IsNullOrEmpty(textOperation.Text))
            {
                MessageBox.Show("Заполните все поля");
                /*MessageBox.Show(comboBox1.Text);
                MessageBox.Show(dateTimePicker1.Text);
                MessageBox.Show(comboBoxVid.Text);
                MessageBox.Show(numericPokaz.Text);
                MessageBox.Show(numericNext.Text);
                MessageBox.Show(comboBoxWho.Text);
                MessageBox.Show(textOperation.Text);*/
            }
            else
            {
                int pok = (int)numericPokaz.Value;
                //int pok = Convert.ToInt32(numericPokaz.Text);
                int next = (int)numericNext.Value;
                DB.InsertTO(comboBox1.Text, dateTimePicker1.Text, comboBoxVid.Text, pok, next, comboBoxWho.Text, textOperation.Text);
                TabTO(comboBox1.Text);
            }
        }

        void otcRem()
        {
            if (String.IsNullOrEmpty(comboBox1.Text) && String.IsNullOrEmpty(dateTimePicker1.Text) && String.IsNullOrEmpty(comboBoxVid.Text) && String.IsNullOrEmpty(numericPokaz.Text) && String.IsNullOrEmpty(comboBoxWho.Text) && String.IsNullOrEmpty(textOperation.Text))
            {
                MessageBox.Show("Заполните все поля");
            }
            else 
            {
                int pok = (int)numericPokaz.Value;
                DB.InsertRemont(comboBox1.Text, dateTimePicker1.Text, comboBoxVid.Text, pok, comboBoxWho.Text, textOperation.Text);
                TabRem(comboBox1.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked && !String.IsNullOrEmpty(comboBox1.Text) && !String.IsNullOrEmpty(comboBox2.Text) && label2.Text != "Информация") CreateTO();
            else
            {
                if (radioButton2.Checked && !String.IsNullOrEmpty(comboBox1.Text) && !String.IsNullOrEmpty(comboBox2.Text) && label2.Text != "Информация") CreateRem();
                else MessageBox.Show("Не хватает данных");
            }
        }

        void CreateTO()
        {
            string namefile = comboBox1.Text + "-ТО.pdf";
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

            Paragraph para = new Paragraph("КАРТА УЧЕТА ТЕХОБСЛУЖИВАНИЯ " + comboBox2.Text.ToUpper() + "\n", Шрифт);
            para.Alignment = Element.ALIGN_CENTER;
            Документ.Add(para);

            para = new Paragraph(label2.Text, Шрифт);
            para.Alignment = Element.ALIGN_LEFT;
            Документ.Add(para);

            /*para = new Paragraph("МАРКА: КЗС-1218-40 гос. № А283БВ 28rus инв. № 27" + "\n", Шрифт);
            para.Alignment = Element.ALIGN_LEFT;
            Документ.Add(para);

            para = new Paragraph("МЕХАНИЗАТОР: Вася Петя" + "\n\n", Шрифт);
            Документ.Add(para);*/

            para = new Paragraph(""+"\n", Шрифт);
            Документ.Add(para);

            /*var Tabla = new PdfPTable(6);
            Документ.Add(Tabla);*/
            PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                table.AddCell(new Phrase(dataGridView1.Columns[j].HeaderText, Шрифт));
            }
            table.HeaderRows = 1;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int k = 0; k < dataGridView1.Columns.Count; k++)
                {
                    if (dataGridView1[k, i].Value != null)
                    {
                        table.AddCell(new Phrase(dataGridView1[k, i].Value.ToString(), Шрифт));
                    }
                }
            }
            Документ.Add(table);

            para = new Paragraph("" + "\n", Шрифт);
            Документ.Add(para);

            para = new Paragraph("Подпись руководителя: ", Шрифт);
            para.Alignment = Element.ALIGN_LEFT;
            Документ.Add(para);

            //Документ.Add(new iTextSharp.text.Paragraph("Текст после таблицы", Шрифт));
            Документ.Close();
            Писатель.Close();
            System.Diagnostics.Process.Start(namefile);
        }

        void CreateRem()
        {
            string namefile = comboBox1.Text + "-Рем.pdf";
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

            Paragraph para = new Paragraph("КАРТА УЧЕТА РЕМОНТА " + comboBox2.Text.ToUpper() + "\n", Шрифт);
            para.Alignment = Element.ALIGN_CENTER;
            Документ.Add(para);

            para = new Paragraph(label2.Text, Шрифт);
            para.Alignment = Element.ALIGN_LEFT;
            Документ.Add(para);

            /*para = new Paragraph("МАРКА: КЗС-1218-40 гос. № А283БВ 28rus инв. № 27" + "\n", Шрифт);
            para.Alignment = Element.ALIGN_LEFT;
            Документ.Add(para);

            para = new Paragraph("МЕХАНИЗАТОР: Вася Петя" + "\n\n", Шрифт);
            Документ.Add(para);*/

            /**/
            para = new Paragraph("" + "\n", Шрифт);
            Документ.Add(para);

            /*var Tabla = new PdfPTable(6);
            Документ.Add(Tabla);*/
            PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                table.AddCell(new Phrase(dataGridView1.Columns[j].HeaderText, Шрифт));
            }
            table.HeaderRows = 1;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int k = 0; k < dataGridView1.Columns.Count; k++)
                {
                    if (dataGridView1[k, i].Value != null)
                    {
                        table.AddCell(new Phrase(dataGridView1[k, i].Value.ToString(), Шрифт));
                    }
                }
            }
            Документ.Add(table);

            para = new Paragraph("" + "\n", Шрифт);
            Документ.Add(para);

            para = new Paragraph("Подпись руководителя: ", Шрифт);
            para.Alignment = Element.ALIGN_LEFT;
            Документ.Add(para);

            //Документ.Add(new iTextSharp.text.Paragraph("Текст после таблицы", Шрифт));
            Документ.Close();
            Писатель.Close();
            System.Diagnostics.Process.Start(namefile);
        }

        //вторая вкладка
        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked) TabDate(dateTimePicker2.Text);
            if (radioButton4.Checked) TabVod(comboVodSea.Text);
            if (radioButton5.Checked) TabDateVod(dateTimePicker2.Text,comboVodSea.Text);
        }

        void TabDate(string date)
        {
            string Query = "SELECT id_avto 'Гос.номер', date 'Дата ТО', id_work 'Вид ТО', pokazaniya 'Показания КМ', nextto 'Следующий ТО', id_person 'Провел ТО', operation 'Операция, материалы' FROM otchetto WHERE date='" + date + "' ORDER BY id_avto";
            string Query1 = "SELECT id_avto 'Гос.номер', date 'Дата ремонта', id_work 'Вид ремонта', pokazaniya 'Показания КМ', id_person 'Провел ремонт', operation 'Операция, материалы' FROM otchetrem WHERE date='" + date + "' ORDER BY id_avto";

            SQLiteCommand cmdDataBase = new SQLiteCommand(Query, DB.Con);
            SQLiteCommand cmdDataBase2 = new SQLiteCommand(Query1, DB.Con);

            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            SQLiteDataAdapter sda2 = new SQLiteDataAdapter();

            sda.SelectCommand = cmdDataBase;
            sda2.SelectCommand = cmdDataBase2;

            DataTable dbdataset = new DataTable();
            DataTable dbdataset2 = new DataTable();

            sda.Fill(dbdataset);
            sda2.Fill(dbdataset2);

            BindingSource bSource = new BindingSource();
            BindingSource bSource2 = new BindingSource();

            bSource.DataSource = dbdataset;
            bSource2.DataSource = dbdataset2;

            dataGridView2.DataSource = bSource;
            dataGridView3.DataSource = bSource2;

            sda.Update(dbdataset);
            sda.Update(dbdataset2);
        }

        void TabDateVod(string date, string vodutel)
        {
            string Query = "SELECT id_avto 'Гос.номер', date 'Дата ТО', id_work 'Вид ТО', pokazaniya 'Показания КМ', nextto 'Следующий ТО', id_person 'Провел ТО', operation 'Операция, материалы' FROM otchetto WHERE date='" + date + "' and id_person LIKE '%" + vodutel + "%' ORDER BY date";
            string Query1 = "SELECT id_avto 'Гос.номер', date 'Дата ремонта', id_work 'Вид ремонта', pokazaniya 'Показания КМ', id_person 'Провел ремонт', operation 'Операция, материалы' FROM otchetrem WHERE date='" + date + "' and id_person LIKE'%" + vodutel + "%' ORDER BY date";

            SQLiteCommand cmdDataBase = new SQLiteCommand(Query, DB.Con);
            SQLiteCommand cmdDataBase2 = new SQLiteCommand(Query1, DB.Con);

            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            SQLiteDataAdapter sda2 = new SQLiteDataAdapter();

            sda.SelectCommand = cmdDataBase;
            sda2.SelectCommand = cmdDataBase2;

            DataTable dbdataset = new DataTable();
            DataTable dbdataset2 = new DataTable();

            sda.Fill(dbdataset);
            sda2.Fill(dbdataset2);

            BindingSource bSource = new BindingSource();
            BindingSource bSource2 = new BindingSource();

            bSource.DataSource = dbdataset;
            bSource2.DataSource = dbdataset2;

            dataGridView2.DataSource = bSource;
            dataGridView3.DataSource = bSource2;

            sda.Update(dbdataset);
            sda.Update(dbdataset2);
        }

        void TabVod(string vodutel)
        {
            //MessageBox.Show(vodutel);
            string Query = "SELECT id_avto 'Гос.номер', date 'Дата ТО', id_work 'Вид ТО', pokazaniya 'Показания КМ', nextto 'Следующий ТО', id_person 'Провел ТО', operation 'Операция, материалы' FROM otchetto WHERE id_person LIKE '%" + vodutel + "%' ORDER BY id_avto";
            string Query1 = "SELECT id_avto 'Гос.номер', date 'Дата ремонта', id_work 'Вид ремонта', pokazaniya 'Показания КМ', id_person 'Провел ремонт', operation 'Операция, материалы' FROM otchetrem WHERE id_person LIKE'%" + vodutel + "%' ORDER BY id_avto";

            SQLiteCommand cmdDataBase = new SQLiteCommand(Query, DB.Con);
            SQLiteCommand cmdDataBase2 = new SQLiteCommand(Query1, DB.Con);

            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            SQLiteDataAdapter sda2 = new SQLiteDataAdapter();

            sda.SelectCommand = cmdDataBase;
            sda2.SelectCommand = cmdDataBase2;

            DataTable dbdataset = new DataTable();
            DataTable dbdataset2 = new DataTable();

            sda.Fill(dbdataset);
            sda2.Fill(dbdataset2);

            BindingSource bSource = new BindingSource();
            BindingSource bSource2 = new BindingSource();

            bSource.DataSource = dbdataset;
            bSource2.DataSource = dbdataset2;

            dataGridView2.DataSource = bSource;
            dataGridView3.DataSource = bSource2;

            sda.Update(dbdataset);
            sda.Update(dbdataset2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked && !String.IsNullOrEmpty(comboBox1.Text) && !String.IsNullOrEmpty(comboBox2.Text) && label2.Text != "Информация")
                Excel("ТО");
            else
            {
                if (radioButton2.Checked && !String.IsNullOrEmpty(comboBox1.Text) && !String.IsNullOrEmpty(comboBox2.Text) && label2.Text != "Информация")
                    Excel("Ремонт");
                else MessageBox.Show("Не хватает данных");
            }
        }

        public static DataTable ToDataTable(DataGridView dataGridView, string tableName)
        {

            DataGridView dgv = dataGridView;
            DataTable table = new DataTable(tableName);

            for (int iCol = 0; iCol < dgv.Columns.Count; iCol++)
            {
                table.Columns.Add(dgv.Columns[iCol].Name);
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {

                DataRow datarw = table.NewRow();

                for (int iCol = 0; iCol < dgv.Columns.Count; iCol++)
                {
                    datarw[iCol] = row.Cells[iCol].Value;
                }

                table.Rows.Add(datarw);
            }
            return table;
        } 
        
        void Excel(string tip)
        {
            string namefile ="";
            string namelist = comboBox1.Text;
            XLWorkbook wb = new XLWorkbook(); //создаем книгу
            IXLWorksheet ws = wb.Worksheets.Add(namelist); //добавляем листы

            if (tip == "ТО")
            { 
                ws.Cell("A1").Value = "КАРТА УЧЕТА ТЕХОБСЛУЖИВАНИЯ " + comboBox2.Text.ToUpper();
                namefile = comboBox1.Text + "-ТО.xlsx";
            }
            if(tip == "Ремонт")
            {   
                ws.Cell("A1").Value = "КАРТА УЧЕТА РЕМОНТА " + comboBox2.Text.ToUpper();
                namefile = comboBox1.Text + "-Рем.xlsx";
            }
            ws.Cell("A1").Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A1:F1").Merge();

            //рабиваем на две подстроки
            string words = label2.Text;
            string[] split = words.Split(new Char[] { '\n' });
            //забиваем дальше
            ws.Cell("A3").Value = split[0];
            //ws.Cell("A3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Range("A3:F3").Merge();
            ws.Cell("A5").Value = split[1];
            ws.Range("A5:F5").Merge();

            DataTable dt = ToDataTable(dataGridView1, namelist);

            /*
            //выводим названия столбцов
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                MessageBox.Show(dt.Columns[i].ToString());
            }
            //выводим значения таблицы
            for (int b = 0; b < dt.Rows.Count; b++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    MessageBox.Show(dt.Rows[b][dt.Columns[i]].ToString());
                }
            }
            
            //выводим названия столбцов
            foreach (DataColumn column in dt.Columns)
            {
                MessageBox.Show(column.Caption.ToString());    
            }
            //выводим значения первого столбца
            foreach (DataRow row in dt.Rows)
            {
                MessageBox.Show(row[0].ToString());
            }
            /*foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //MessageBox.Show(column.ToString());
                    MessageBox.Show(row[column].ToString());
                }
            }*/
            //MessageBox.Show("Кол-во строк: "+dt.Rows.Count.ToString());
            //MessageBox.Show("Кол-во столбцов: "+ dt.Columns.Count.ToString());
            ws.Cell(7, 1).InsertTable(dt);

            wb.SaveAs(namefile); //сохраняем в файл
            System.Diagnostics.Process.Start(namefile);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = ToDataTable(dataGridView1, comboBox1.Text);

            if (radioButton1.Checked && !String.IsNullOrEmpty(comboBox1.Text) && !String.IsNullOrEmpty(comboBox2.Text) && label2.Text != "Информация")
                Word.Otchet("ТО", comboBox1.Text, comboBox2.Text, label2.Text, dt);
            else
            {
                if (radioButton2.Checked && !String.IsNullOrEmpty(comboBox1.Text) && !String.IsNullOrEmpty(comboBox2.Text) && label2.Text != "Информация")
                    Word.Otchet("Ремонт", comboBox1.Text, comboBox2.Text, label2.Text, dt);
                else MessageBox.Show("Не хватает данных");
            }
        }


    }
}
