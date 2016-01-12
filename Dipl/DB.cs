using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SQLite;
using System.Security.Cryptography;

namespace Dipl
{
    class DB
    {
        public static string directory = AppDomain.CurrentDomain.BaseDirectory; //текущая директория
        public static string databaseName; //путь до бд, в общем имя бд
        public static SQLiteConnection Con;

        public static void ConnectBD(string namebd)
        {
            //SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName));
            //connection.Open();
            Con = new SQLiteConnection("Data Source=" + namebd + ";"); //foreign keys=true;
            try
            {
                Con.Open();
                //MessageBox.Show("mysql Connect: Open");
            }
            catch (Exception ex)
            {
                MessageBox.Show("mysql Connect: " + ex);
            }
        }

        public static void CloseCon()
        {
            Con.Close();
        }

        public static void ExecuteNonQuery(string command)
        {
            try
            {
                new SQLiteCommand(command, Con).ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void CreateBD(string filebd)
        {
            databaseName = filebd; //присваиваем глобальной переменной наш путь к бд, полученный ранее
            SQLiteConnection.CreateFile(databaseName);
            if (File.Exists(databaseName)) MessageBox.Show("Файл базы данных создан", "Отлично", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Возникла ошибка при создании файла базы данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ConnectBD(databaseName);
            string zapros = "CREATE TABLE user (id INTEGER PRIMARY KEY AUTOINCREMENT, login TEXT, password TEXT, role INT); "+
                "INSERT INTO 'user' ('login', 'password', 'role') VALUES ('admin', 'admin', 1);"+
                "INSERT INTO 'user' ('login', 'password', 'role') VALUES ('user', 'user', 0);";
            string zapros2 = "CREATE TABLE avto (nomer TEXT, invent INT, marka TEXT, id_type INT, id_vod INT, id_vod2 INT); "+
                "INSERT INTO 'avto' ('nomer', 'invent', 'marka', 'id_type', 'id_vod', 'id_vod2') VALUES ('А234ВБ', 0027, 'КЗС-1218-40', 1, 2, 1);" +
                "INSERT INTO 'avto' ('nomer', 'invent', 'marka', 'id_type', 'id_vod', 'id_vod2') VALUES ('Г253ВБ', 0028, 'КЗС-1238-40', 1, 2, 5);";
            string zapros3 = "CREATE TABLE tipavto (id INTEGER PRIMARY KEY AUTOINCREMENT, nametip TEXT);"+
                " INSERT INTO 'tipavto' ('nametip') VALUES ('Автомобиля'); INSERT INTO 'tipavto' ('nametip') VALUES ('Комбайна');"+
                " INSERT INTO 'tipavto' ('nametip') VALUES ('Прицепа НЕФАЗ'); INSERT INTO 'tipavto' ('nametip') VALUES ('Тракторного прицепа');" +
                " INSERT INTO 'tipavto' ('nametip') VALUES ('Трактора');";
            string zapros4 = "CREATE TABLE personal (id INTEGER PRIMARY KEY AUTOINCREMENT, fio TEXT); INSERT INTO 'personal' ('fio') VALUES ('');"+
                "INSERT INTO 'personal' ('fio') VALUES ('Боголюб С.Г.'); INSERT INTO 'personal' ('fio') VALUES ('Безверхий О.А.');";
            string zapros5 = "CREATE TABLE work (id INTEGER PRIMARY KEY AUTOINCREMENT, namework TEXT); INSERT INTO 'work' ('namework') VALUES ('ТО1');" +
                    "INSERT INTO 'work' ('namework') VALUES ('ТО2'); INSERT INTO 'work' ('namework') VALUES ('Текущий');";
            string zapros6 = "CREATE TABLE otchetto (id INTEGER PRIMARY KEY AUTOINCREMENT, id_avto TEXT, date TEXT, id_work TEXT, pokazaniya INT, nextto INT, id_person TEXT, operation TEXT);";
            string zapros7 = "CREATE TABLE otchetrem (id INTEGER PRIMARY KEY AUTOINCREMENT, id_avto TEXT, date TEXT, id_work TEXT, pokazaniya INT, id_person TEXT, operation TEXT);";
            /*SQLiteCommand command = new SQLiteCommand(zapros, Con);
            command.ExecuteNonQuery();
            command = new SQLiteCommand(zapros2, Con);
            command.ExecuteNonQuery();
            command = new SQLiteCommand(zapros3, Con);
            command.ExecuteNonQuery();*/
            try
            {
                ExecuteNonQuery(zapros);
                ExecuteNonQuery(zapros2);
                ExecuteNonQuery(zapros3);
                ExecuteNonQuery(zapros4);
                ExecuteNonQuery(zapros5);
                ExecuteNonQuery(zapros6);
                ExecuteNonQuery(zapros7);
                MessageBox.Show("Создал таблицу и вставил данные");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }    
                /*("INSERT INTO 'user' ('login', 'password', 'role') VALUES ('admin', 'admin', 1);", Con);
                MessageBox.Show("Данные вставленны");*/
        }

        public static bool Login(string login, string password)
        {
            if (String.IsNullOrEmpty(login))
            {
                MessageBox.Show("Поле логин не должно быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //if (Con != null) MessageBox.Show("Norma");
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'user' WHERE login='" + login + "' and password='" + password + "'", Con);
                command.ExecuteNonQuery();
                SQLiteDataReader dr = command.ExecuteReader();

                int count = 0;

                while (dr.Read())
                {
                    count++;
                }

                switch(count)
                {
                    case 1: MessageBox.Show("Логин и пароль совпали"); return true;
                    case 0 : MessageBox.Show("Пользователь не найден"); break;
                    default : MessageBox.Show("Количество пользователей "+count); break;
                }
            }
            return false;
        }

        public static int UserAdmin(string login, string password)
        {
            int role=0;
            SQLiteCommand command = new SQLiteCommand("SELECT role FROM 'user' WHERE login='" + login + "' and password='" + password + "'", Con);
            command.ExecuteNonQuery();
            SQLiteDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                role = dr.GetInt32(0);
            }
            //MessageBox.Show(role);
            return role;
        }

        public static void InsertTO(string id_avto, string date, string id_work, int pokazanuya, int nextto, string idperson, string operation)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO 'otchetto' ('id_avto', 'date', 'id_work', 'pokazaniya', 'nextto', 'id_person', 'operation') VALUES ('"+id_avto+"', '"+date+"', '"+id_work+"', "+pokazanuya+", "+nextto+", '"+idperson+"', '"+operation+"');", Con);
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Данные успешно занесены");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
                
        public static void InsertRemont(string id_avto, string date, string id_work, int pokazanuya, string idperson, string operation)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO 'otchetrem' ('id_avto', 'date', 'id_work', 'pokazaniya', 'id_person', 'operation') VALUES ('" + id_avto + "', '" + date + "', '" + id_work + "', " + pokazanuya + ", '" + idperson + "', '" + operation + "');", Con);
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Данные успешно занесены");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void InsertUser(string login, string pass, int role)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'user' WHERE login='" + login + "'", Con);
            command.ExecuteNonQuery();
            SQLiteDataReader dr = command.ExecuteReader();

            int count = 0;

            while (dr.Read())
            {
                count++;
            }

            if (count >= 1) 
            {
                MessageBox.Show("Пользователь с таким именем есть!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                command = new SQLiteCommand("INSERT INTO 'user' ('login', 'password', 'role') VALUES ('" + login + "', '" + pass + "', '" + role + "');", Con);
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные о пользователе успешно внесены");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static void InsertPersonal(string fio)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'personal' WHERE fio='" + fio + "'", Con);
            command.ExecuteNonQuery();
            SQLiteDataReader dr = command.ExecuteReader();

            int count = 0;

            while (dr.Read())
            {
                count++;
            }

            if (count >= 1)
            {
                MessageBox.Show("Водитель с такими данными уже есть!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                command = new SQLiteCommand("INSERT INTO 'personal' ('fio') VALUES ('"+fio+"');", Con);
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные о водителе успешно внесены");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static void InsertVidRabot(string rabota)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'work' WHERE namework='" + rabota + "'", Con);
            command.ExecuteNonQuery();
            SQLiteDataReader dr = command.ExecuteReader();

            int count = 0;

            while (dr.Read())
            {
                count++;
            }

            if (count >= 1)
            {
                MessageBox.Show("Такой вид работы уже есть!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                command = new SQLiteCommand("INSERT INTO 'work' ('namework') VALUES ('" + rabota + "');", Con);
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Вид работы успешно внесен");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static void EditUser(string deflogin, string login, string pass, int role)
        {
            if (String.IsNullOrEmpty(login) && String.IsNullOrEmpty(pass))
            {
                MessageBox.Show(login);
                MessageBox.Show(pass);
                MessageBox.Show("Поля не должны быть пустыми", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SQLiteCommand command = new SQLiteCommand("UPDATE 'user' SET 'login'='"+login+"', 'password'='"+pass+"', 'role'='"+role+"' WHERE login='"+deflogin+"';", Con);
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные о пользователе успешно изменены");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static string Category(int id_type)
        {
            string nametip="";
            SQLiteCommand command = new SQLiteCommand("SELECT nametip FROM 'tipavto' WHERE id=" + id_type + "", Con);
            command.ExecuteNonQuery();
            SQLiteDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                nametip = dr.GetString(0);
            }
            return nametip;
        }

        public static string Vodut(int id_vod)
        {
            string vod = "";
            SQLiteCommand command = new SQLiteCommand("SELECT fio FROM 'personal' WHERE id=" + id_vod + "", Con);
            command.ExecuteNonQuery();
            SQLiteDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                vod = dr.GetString(0);
            }
            return vod;
        }

        public static int IdVodut(string fio)
        {
            int id=0;
            SQLiteCommand command = new SQLiteCommand("SELECT id FROM 'personal' WHERE fio='" + fio + "'", Con);
            command.ExecuteNonQuery();
            SQLiteDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                id = dr.GetInt32(0);
            }
            //Console.WriteLine("id = "+id);
            return id;
        }

        public static int IdCategory(string nametip)
        {
            int id = 0;
            SQLiteCommand command = new SQLiteCommand("SELECT id FROM 'tipavto' WHERE nametip='" + nametip + "'", Con);
            command.ExecuteNonQuery();
            SQLiteDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                id = dr.GetInt32(0);
            }
            return id;
        }

        public static void InsertAvto(string nomer, int invent, string marka, int id_type, int id_vod, int id_vod2)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO 'avto' ('nomer', 'invent', 'marka', 'id_type', 'id_vod', 'id_vod2') VALUES ('" + nomer + "', " + invent + ", '" + marka + "', "+id_type+", "+id_vod+", "+id_vod2+");", Con);
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Данные о технике успешно внесены");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void EditAvto(string defnomer, string nomer, int invent, string marka, int id_type, int id_vod, int id_vod2)
        {
            SQLiteCommand command = new SQLiteCommand("UPDATE 'avto' SET 'nomer'='"+nomer+"', 'invent'='"+invent+"', 'marka'='"+marka+"', 'id_type'='"+id_type+"', 'id_vod'='"+id_vod+"', 'id_vod2'='"+id_vod2+"' WHERE nomer='" + defnomer + "';", Con);
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Данные о технике успешно изменены");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
