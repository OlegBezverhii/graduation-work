using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dipl
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
            listuser();
            listavto();
            listtipavto();
            listvod();
        }

        void listuser()
        {
            listBox1.Items.Clear();
            string Query = "SELECT * FROM user";
            try
            {
                SQLiteCommand command = new SQLiteCommand(Query, DB.Con);
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string login = dr.GetString(1);
                    listBox1.Items.Add(login);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void listavto()
        {
            listBox2.Items.Clear();
            string Query = "SELECT nomer FROM avto";
            try
            {
                SQLiteCommand command = new SQLiteCommand(Query, DB.Con);
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string nomer = dr.GetString(0);
                    listBox2.Items.Add(nomer);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void listtipavto()
        {
            string Query = "SELECT nametip FROM tipavto";
            try
            {
                SQLiteCommand command = new SQLiteCommand(Query, DB.Con);
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    comboBox3.Items.Add(dr.GetString(0));
                    comboBox4.Items.Add(dr.GetString(0));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void listvod()
        {
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            comboBox7.Items.Clear();
            comboBox8.Items.Clear();
            string Query = "SELECT fio FROM personal ORDER BY fio ASC";
            try
            {
                SQLiteCommand command = new SQLiteCommand(Query, DB.Con);
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    comboBox5.Items.Add(dr.GetString(0));
                    comboBox6.Items.Add(dr.GetString(0));
                    comboBox7.Items.Add(dr.GetString(0));
                    comboBox8.Items.Add(dr.GetString(0));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string pass = textBox2.Text;
            int role;
            if(comboBox1.Text=="Пользователь") role = 0; else role = 1;
            DB.InsertUser(login, pass, role);
            listuser();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DB.InsertPersonal(textBox5.Text);
            listvod();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DB.InsertVidRabot(textBox6.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int role;
            if (comboBox2.Text == "Пользователь") role = 0; else role = 1;
            DB.EditUser(listBox1.Text, textBox4.Text, textBox3.Text, role);
            listuser();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT login, role FROM 'user' WHERE login='" + listBox1.Text + "'", DB.Con);
            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    textBox4.Text = dr.GetString(0);
                    if (dr.GetInt32(1) == 0) comboBox2.SelectedItem = "Пользователь"; else comboBox2.SelectedItem = "Администратор";
                }
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'avto' WHERE nomer='" + listBox2.Text + "'", DB.Con);
            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    textBox11.Text = dr.GetString(0);
                    numericUpDown2.Value = dr.GetInt32(1);
                    textBox10.Text = dr.GetString(2);
                    comboBox4.SelectedItem = DB.Category(dr.GetInt32(3));
                    comboBox7.SelectedItem = DB.Vodut(dr.GetInt32(4));
                    comboBox8.SelectedItem = DB.Vodut(dr.GetInt32(5));
                }
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox7.Text) && String.IsNullOrEmpty(textBox8.Text) && String.IsNullOrEmpty(comboBox5.Text) && String.IsNullOrEmpty(numericUpDown1.Text) && String.IsNullOrEmpty(comboBox3.Text))
            { MessageBox.Show("Поля не должны быть пустыми", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else
            {
                int vod = DB.IdVodut(comboBox5.Text);
                int vod2 = DB.IdVodut(comboBox6.Text);
                int category = DB.IdCategory(comboBox3.Text);
                int invent = (int)numericUpDown1.Value;
                MessageBox.Show("Номер " + textBox7.Text + " № "+invent+" Марка "+textBox8.Text+" Категор "+category+" Водил "+vod+" №2= "+vod2);
                DB.InsertAvto(textBox7.Text, invent, textBox8.Text, category, vod, vod2);
                listavto();
            }
        }
        
        private void button6_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox11.Text) && String.IsNullOrEmpty(textBox10.Text) && String.IsNullOrEmpty(comboBox7.Text) && String.IsNullOrEmpty(numericUpDown2.Text) && String.IsNullOrEmpty(comboBox4.Text))
            { MessageBox.Show("Поля не должны быть пустыми", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else 
            {
                int invent = (int)numericUpDown2.Value;
                int vod = DB.IdVodut(comboBox7.Text);
                int vod2 = DB.IdVodut(comboBox8.Text);
                int category = DB.IdCategory(comboBox4.Text);
                //MessageBox.Show("Номер " + lis.Text + " № " + invent + " Марка " + textBox8.Text + " Категор " + category + " Водил " + vod + " №2= " + vod2);
                DB.EditAvto(listBox2.Text, textBox11.Text, invent, textBox10.Text, category, vod, vod2);
                listavto();
            }
        }

    }
}
