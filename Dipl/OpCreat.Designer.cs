namespace Dipl
{
    partial class OpCreat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.butcreatebd = new System.Windows.Forms.Button();
            this.butselectbd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(42, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Файл базы данных не найден.\r\nВыберите одно из действий";
            // 
            // butcreatebd
            // 
            this.butcreatebd.Location = new System.Drawing.Point(45, 94);
            this.butcreatebd.Name = "butcreatebd";
            this.butcreatebd.Size = new System.Drawing.Size(75, 39);
            this.butcreatebd.TabIndex = 1;
            this.butcreatebd.Text = "Создать БД";
            this.butcreatebd.UseVisualStyleBackColor = true;
            this.butcreatebd.Click += new System.EventHandler(this.butcreatebd_Click);
            // 
            // butselectbd
            // 
            this.butselectbd.Location = new System.Drawing.Point(169, 94);
            this.butselectbd.Name = "butselectbd";
            this.butselectbd.Size = new System.Drawing.Size(75, 39);
            this.butselectbd.TabIndex = 2;
            this.butselectbd.Text = "Выбрать БД";
            this.butselectbd.UseVisualStyleBackColor = true;
            this.butselectbd.Click += new System.EventHandler(this.butselectbd_Click);
            // 
            // OpCreat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 175);
            this.Controls.Add(this.butselectbd);
            this.Controls.Add(this.butcreatebd);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpCreat";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Создать или выбрать БД";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butcreatebd;
        private System.Windows.Forms.Button butselectbd;
    }
}