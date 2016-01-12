using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dipl
{
    public partial class OpCreat : Form
    {
        public OpCreat()
        {
            InitializeComponent();
        }

        private void butcreatebd_Click(object sender, EventArgs e)
        {
            DB.CreateBD(DB.directory+"agro.db");
            //MessageBox.Show("Типа создал БД");
            this.Hide();
            Program.Launch();
        }

        private void butselectbd_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd1 = new OpenFileDialog();  //открываем окно выбора файла БД
            ofd1.Filter = "database (*.db)|*.db";
            ofd1.Title = "Открыть базу данных";
            ofd1.InitialDirectory = DB.directory;

            if (ofd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filenam = ofd1.FileName;
                MessageBox.Show(filenam);
                this.Hide();
                Program.Launch();
            }
        }
    }
}
