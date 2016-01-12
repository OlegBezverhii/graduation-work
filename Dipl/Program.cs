using System;
using System.Windows.Forms;
using System.IO;

namespace Dipl
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        //static string directory = AppDomain.CurrentDomain.BaseDirectory;
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Proverka();
            //MessageBox.Show("Запусти программу");
            //Launch();
        }

        public static void Launch()
        {
            LoginForm2 form = new LoginForm2();
            form.ShowDialog();
            /*DialogResult result;
            result = form.ShowDialog();
            if (result == DialogResult.OK) //Зафигачить проверку  && !String.IsNullOrEmpty(Login.Type)
            {
                TO mainform = new TO();
                mainform.ShowDialog();
            }*/
        }

        public static void Proverka()
        {
            
            string[] files = Directory.GetFiles(DB.directory, "*.db"); //получаю в текущей директории все файлы баз данных
            if (files.Length == 0)
            {
                OpCreat opcreat = new OpCreat();
                opcreat.ShowDialog(); //форма создания бд или выбора другой бд
            }
            else
            {
                //MessageBox.Show("Всего файлов " + files.Length);
                if (files.Length == 1)  //он один, приравниваем и конектимся
                {
                    MessageBox.Show("Файл один " + files[0]);
                    DB.ConnectBD(files[0]);
                    Launch();
                }
                else  // фигачим форму с выбором файлов
                {
                    MessageBox.Show("Файлов больше одного, выберите один из них");
                    OpenFileDialog ofd2 = new OpenFileDialog();  //открываем окно выбора файла БД
                    ofd2.Filter = "database (*.db)|*.db";
                    ofd2.Title = "Открыть базу данных";
                    ofd2.InitialDirectory = DB.directory;

                    if (ofd2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string filenam = ofd2.FileName;
                        MessageBox.Show(filenam);
                        Launch();
                    }
                }
            }
        }
    }
}