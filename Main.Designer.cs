namespace вход_курсач
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            бэкапБдToolStripMenuItem = new ToolStripMenuItem();
            ОтчётToolStripMenuItem = new ToolStripMenuItem();
            сменныйToolStripMenuItem = new ToolStripMenuItem();
            квартальныйToolStripMenuItem = new ToolStripMenuItem();
            годовойToolStripMenuItem = new ToolStripMenuItem();
            планПоставкиToolStripMenuItem = new ToolStripMenuItem();
            отчётОРаботеКомпанииToolStripMenuItem = new ToolStripMenuItem();
            пользователиToolStripMenuItem = new ToolStripMenuItem();
            учётныеЗаписиToolStripMenuItem = new ToolStripMenuItem();
            заявкиНаСозданиеНовогоПользователяToolStripMenuItem = new ToolStripMenuItem();
            графикРаботыToolStripMenuItem = new ToolStripMenuItem();
            информацияОПредприToolStripMenuItem = new ToolStripMenuItem();
            lblTotalPlan = new Label();
            lblAveragePlan = new Label();
            dgvDailyPlan = new DataGridView();
            cmbPlanMonth = new ComboBox();
            cmbPlanYear = new ComboBox();
            btnSavePlanToDB = new Button();
            pictureBox1 = new PictureBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDailyPlan).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(0, 102, 204);
            menuStrip1.ForeColor = Color.White;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, бэкапБдToolStripMenuItem, ОтчётToolStripMenuItem, планПоставкиToolStripMenuItem, отчётОРаботеКомпанииToolStripMenuItem, пользователиToolStripMenuItem, графикРаботыToolStripMenuItem, информацияОПредприToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1076, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(14, 24);
            // 
            // бэкапБдToolStripMenuItem
            // 
            бэкапБдToolStripMenuItem.Name = "бэкапБдToolStripMenuItem";
            бэкапБдToolStripMenuItem.Size = new Size(84, 24);
            бэкапБдToolStripMenuItem.Text = "бэкап бд";
            бэкапБдToolStripMenuItem.Click += бэкапБдToolStripMenuItem_Click;
            // 
            // ОтчётToolStripMenuItem
            // 
            ОтчётToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { сменныйToolStripMenuItem, квартальныйToolStripMenuItem, годовойToolStripMenuItem });
            ОтчётToolStripMenuItem.Name = "ОтчётToolStripMenuItem";
            ОтчётToolStripMenuItem.Size = new Size(62, 24);
            ОтчётToolStripMenuItem.Text = "Отчёт";
            // 
            // сменныйToolStripMenuItem
            // 
            сменныйToolStripMenuItem.Name = "сменныйToolStripMenuItem";
            сменныйToolStripMenuItem.Size = new Size(185, 26);
            сменныйToolStripMenuItem.Text = "Сменный";
            сменныйToolStripMenuItem.Click += сменныйToolStripMenuItem_Click;
            // 
            // квартальныйToolStripMenuItem
            // 
            квартальныйToolStripMenuItem.Name = "квартальныйToolStripMenuItem";
            квартальныйToolStripMenuItem.Size = new Size(185, 26);
            квартальныйToolStripMenuItem.Text = "Квартальный";
            квартальныйToolStripMenuItem.Click += квартальныйToolStripMenuItem_Click;
            // 
            // годовойToolStripMenuItem
            // 
            годовойToolStripMenuItem.Name = "годовойToolStripMenuItem";
            годовойToolStripMenuItem.Size = new Size(185, 26);
            годовойToolStripMenuItem.Text = "Годовой";
            годовойToolStripMenuItem.Click += годовойToolStripMenuItem_Click;
            // 
            // планПоставкиToolStripMenuItem
            // 
            планПоставкиToolStripMenuItem.Name = "планПоставкиToolStripMenuItem";
            планПоставкиToolStripMenuItem.Size = new Size(115, 24);
            планПоставкиToolStripMenuItem.Text = "План работы";
            планПоставкиToolStripMenuItem.Click += планПоставкиToolStripMenuItem_Click;
            // 
            // отчётОРаботеКомпанииToolStripMenuItem
            // 
            отчётОРаботеКомпанииToolStripMenuItem.Name = "отчётОРаботеКомпанииToolStripMenuItem";
            отчётОРаботеКомпанииToolStripMenuItem.Size = new Size(162, 24);
            отчётОРаботеКомпанииToolStripMenuItem.Text = "О работе компании";
            отчётОРаботеКомпанииToolStripMenuItem.Click += отчётОРаботеКомпанииToolStripMenuItem_Click;
            // 
            // пользователиToolStripMenuItem
            // 
            пользователиToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { учётныеЗаписиToolStripMenuItem, заявкиНаСозданиеНовогоПользователяToolStripMenuItem });
            пользователиToolStripMenuItem.Name = "пользователиToolStripMenuItem";
            пользователиToolStripMenuItem.Size = new Size(122, 24);
            пользователиToolStripMenuItem.Text = "Пользователи";
            // 
            // учётныеЗаписиToolStripMenuItem
            // 
            учётныеЗаписиToolStripMenuItem.Name = "учётныеЗаписиToolStripMenuItem";
            учётныеЗаписиToolStripMenuItem.Size = new Size(384, 26);
            учётныеЗаписиToolStripMenuItem.Text = "Учётные записи";
            учётныеЗаписиToolStripMenuItem.Click += учётныеЗаписиToolStripMenuItem_Click;
            // 
            // заявкиНаСозданиеНовогоПользователяToolStripMenuItem
            // 
            заявкиНаСозданиеНовогоПользователяToolStripMenuItem.Name = "заявкиНаСозданиеНовогоПользователяToolStripMenuItem";
            заявкиНаСозданиеНовогоПользователяToolStripMenuItem.Size = new Size(384, 26);
            заявкиНаСозданиеНовогоПользователяToolStripMenuItem.Text = "Заявки на создание нового пользователя";
            заявкиНаСозданиеНовогоПользователяToolStripMenuItem.Click += заявкиНаСозданиеНовогоПользователяToolStripMenuItem_Click;
            // 
            // графикРаботыToolStripMenuItem
            // 
            графикРаботыToolStripMenuItem.Name = "графикРаботыToolStripMenuItem";
            графикРаботыToolStripMenuItem.Size = new Size(129, 24);
            графикРаботыToolStripMenuItem.Text = "График работы";
            графикРаботыToolStripMenuItem.Click += графикРаботыToolStripMenuItem_Click;
            // 
            // информацияОПредприToolStripMenuItem
            // 
            информацияОПредприToolStripMenuItem.Name = "информацияОПредприToolStripMenuItem";
            информацияОПредприToolStripMenuItem.Size = new Size(226, 24);
            информацияОПредприToolStripMenuItem.Text = "Информация о предприятии";
            информацияОПредприToolStripMenuItem.Click += информацияОПредприToolStripMenuItem_Click;
            // 
            // lblTotalPlan
            // 
            lblTotalPlan.AutoSize = true;
            lblTotalPlan.BackColor = Color.Transparent;
            lblTotalPlan.Location = new Point(32, 103);
            lblTotalPlan.Name = "lblTotalPlan";
            lblTotalPlan.Size = new Size(51, 20);
            lblTotalPlan.TabIndex = 2;
            lblTotalPlan.Text = "Всего:";
            lblTotalPlan.Visible = false;
            // 
            // lblAveragePlan
            // 
            lblAveragePlan.AutoSize = true;
            lblAveragePlan.BackColor = Color.Transparent;
            lblAveragePlan.Location = new Point(32, 158);
            lblAveragePlan.Name = "lblAveragePlan";
            lblAveragePlan.Size = new Size(160, 20);
            lblAveragePlan.TabIndex = 3;
            lblAveragePlan.Text = "Средний план в день:";
            lblAveragePlan.Visible = false;
            // 
            // dgvDailyPlan
            // 
            dgvDailyPlan.BackgroundColor = Color.SteelBlue;
            dgvDailyPlan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDailyPlan.Location = new Point(394, 89);
            dgvDailyPlan.Name = "dgvDailyPlan";
            dgvDailyPlan.RowHeadersWidth = 51;
            dgvDailyPlan.Size = new Size(587, 446);
            dgvDailyPlan.TabIndex = 7;
            dgvDailyPlan.Visible = false;
            // 
            // cmbPlanMonth
            // 
            cmbPlanMonth.BackColor = SystemColors.Window;
            cmbPlanMonth.FormattingEnabled = true;
            cmbPlanMonth.Location = new Point(133, 46);
            cmbPlanMonth.Name = "cmbPlanMonth";
            cmbPlanMonth.Size = new Size(151, 28);
            cmbPlanMonth.TabIndex = 8;
            cmbPlanMonth.Visible = false;
            // 
            // cmbPlanYear
            // 
            cmbPlanYear.FormattingEnabled = true;
            cmbPlanYear.Location = new Point(364, 46);
            cmbPlanYear.Name = "cmbPlanYear";
            cmbPlanYear.Size = new Size(151, 28);
            cmbPlanYear.TabIndex = 9;
            cmbPlanYear.Visible = false;
            // 
            // btnSavePlanToDB
            // 
            btnSavePlanToDB.Location = new Point(32, 346);
            btnSavePlanToDB.Name = "btnSavePlanToDB";
            btnSavePlanToDB.Size = new Size(150, 43);
            btnSavePlanToDB.TabIndex = 10;
            btnSavePlanToDB.Text = "Сохранить план";
            btnSavePlanToDB.UseVisualStyleBackColor = true;
            btnSavePlanToDB.Visible = false;
            btnSavePlanToDB.Click += btnSavePlanToDB_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(12, 31);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1052, 514);
            pictureBox1.TabIndex = 11;
            pictureBox1.TabStop = false;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1076, 557);
            Controls.Add(btnSavePlanToDB);
            Controls.Add(cmbPlanYear);
            Controls.Add(cmbPlanMonth);
            Controls.Add(dgvDailyPlan);
            Controls.Add(lblAveragePlan);
            Controls.Add(lblTotalPlan);
            Controls.Add(menuStrip1);
            Controls.Add(pictureBox1);
            MainMenuStrip = menuStrip1;
            Name = "Main";
            Text = "Main";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDailyPlan).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem пользователиToolStripMenuItem;
        private ToolStripMenuItem учётныеЗаписиToolStripMenuItem;
        private ToolStripMenuItem заявкиНаСозданиеНовогоПользователяToolStripMenuItem;
        private ToolStripMenuItem ОтчётToolStripMenuItem;
        private ToolStripMenuItem сменныйToolStripMenuItem;
        private ToolStripMenuItem квартальныйToolStripMenuItem;
        private ToolStripMenuItem годовойToolStripMenuItem;
        private ToolStripMenuItem графикРаботыToolStripMenuItem;
        private ToolStripMenuItem информацияОПредприToolStripMenuItem;
        private ToolStripMenuItem отчётОРаботеКомпанииToolStripMenuItem;
        private Button btnSavePlanToDB;
        private ToolStripMenuItem планПоставкиToolStripMenuItem;
        private ToolStripMenuItem бэкапБдToolStripMenuItem;
        private Label lblTotalPlan;
        private Label lblAveragePlan;
        private DataGridView dgvDailyPlan;
        private ComboBox cmbPlanMonth;
        private ComboBox cmbPlanYear;
        private PictureBox pictureBox1;
    }
}