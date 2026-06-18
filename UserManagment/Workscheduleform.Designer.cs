using static System.Net.Mime.MediaTypeNames;

namespace вход_курсач
{
    partial class WorkScheduleForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dataGridViewSchedule = new DataGridView();
            panelEditor = new Panel();
            labelEmployee = new Label();
            comboBoxEmployee = new ComboBox();
            labelDate = new Label();
            dateTimePickerWork = new DateTimePicker();
            labelShift = new Label();
            comboBoxShift = new ComboBox();
            buttonAdd = new Button();
            buttonDelete = new Button();
            panelFilter = new Panel();
            labelFilterMonth = new Label();
            checkBoxFilterMonth = new CheckBox();
            dateTimePickerMonth = new DateTimePicker();
            buttonRefresh = new Button();
            labelHint = new Label();
            button1 = new Button();
            textBox1 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSchedule).BeginInit();
            panelEditor.SuspendLayout();
            panelFilter.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewSchedule
            // 
            dataGridViewSchedule.AllowUserToAddRows = false;
            dataGridViewSchedule.AllowUserToDeleteRows = false;
            dataGridViewSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewSchedule.ColumnHeadersHeight = 29;
            dataGridViewSchedule.Location = new Point(12, 80);
            dataGridViewSchedule.MultiSelect = false;
            dataGridViewSchedule.Name = "dataGridViewSchedule";
            dataGridViewSchedule.ReadOnly = true;
            dataGridViewSchedule.RowHeadersWidth = 51;
            dataGridViewSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSchedule.Size = new Size(960, 420);
            dataGridViewSchedule.TabIndex = 0;
            dataGridViewSchedule.SelectionChanged += dataGridViewSchedule_SelectionChanged;
            // 
            // panelEditor
            // 
            panelEditor.BorderStyle = BorderStyle.FixedSingle;
            panelEditor.Controls.Add(labelEmployee);
            panelEditor.Controls.Add(comboBoxEmployee);
            panelEditor.Controls.Add(labelDate);
            panelEditor.Controls.Add(dateTimePickerWork);
            panelEditor.Controls.Add(labelShift);
            panelEditor.Controls.Add(comboBoxShift);
            panelEditor.Controls.Add(buttonAdd);
            panelEditor.Controls.Add(buttonDelete);
            panelEditor.Location = new Point(12, 515);
            panelEditor.Name = "panelEditor";
            panelEditor.Size = new Size(960, 70);
            panelEditor.TabIndex = 1;
            // 
            // labelEmployee
            // 
            labelEmployee.AutoSize = true;
            labelEmployee.Location = new Point(8, 22);
            labelEmployee.Name = "labelEmployee";
            labelEmployee.Size = new Size(85, 20);
            labelEmployee.TabIndex = 0;
            labelEmployee.Text = "Сотрудник:";
            // 
            // comboBoxEmployee
            // 
            comboBoxEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEmployee.FormattingEnabled = true;
            comboBoxEmployee.Location = new Point(90, 18);
            comboBoxEmployee.Name = "comboBoxEmployee";
            comboBoxEmployee.Size = new Size(300, 28);
            comboBoxEmployee.TabIndex = 1;
            // 
            // labelDate
            // 
            labelDate.AutoSize = true;
            labelDate.Location = new Point(405, 22);
            labelDate.Name = "labelDate";
            labelDate.Size = new Size(44, 20);
            labelDate.TabIndex = 2;
            labelDate.Text = "Дата:";
            // 
            // dateTimePickerWork
            // 
            dateTimePickerWork.Format = DateTimePickerFormat.Short;
            dateTimePickerWork.Location = new Point(450, 18);
            dateTimePickerWork.Name = "dateTimePickerWork";
            dateTimePickerWork.Size = new Size(130, 27);
            dateTimePickerWork.TabIndex = 2;
            // 
            // labelShift
            // 
            labelShift.AutoSize = true;
            labelShift.Location = new Point(595, 22);
            labelShift.Name = "labelShift";
            labelShift.Size = new Size(57, 20);
            labelShift.TabIndex = 3;
            labelShift.Text = "Смена:";
            // 
            // comboBoxShift
            // 
            comboBoxShift.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxShift.FormattingEnabled = true;
            comboBoxShift.Location = new Point(648, 18);
            comboBoxShift.Name = "comboBoxShift";
            comboBoxShift.Size = new Size(120, 28);
            comboBoxShift.TabIndex = 3;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(785, 17);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(80, 30);
            buttonAdd.TabIndex = 4;
            buttonAdd.Text = "➕ Добавить";
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.BackColor = Color.FromArgb(255, 200, 200);
            buttonDelete.Location = new Point(875, 17);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(75, 30);
            buttonDelete.TabIndex = 5;
            buttonDelete.Text = "🗑 Удалить";
            buttonDelete.UseVisualStyleBackColor = false;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // panelFilter
            // 
            panelFilter.BorderStyle = BorderStyle.FixedSingle;
            panelFilter.Controls.Add(labelFilterMonth);
            panelFilter.Controls.Add(checkBoxFilterMonth);
            panelFilter.Controls.Add(dateTimePickerMonth);
            panelFilter.Controls.Add(buttonRefresh);
            panelFilter.Location = new Point(12, 12);
            panelFilter.Name = "panelFilter";
            panelFilter.Size = new Size(581, 55);
            panelFilter.TabIndex = 0;
            // 
            // labelFilterMonth
            // 
            labelFilterMonth.AutoSize = true;
            labelFilterMonth.Location = new Point(8, 18);
            labelFilterMonth.Name = "labelFilterMonth";
            labelFilterMonth.Size = new Size(139, 20);
            labelFilterMonth.TabIndex = 0;
            labelFilterMonth.Text = "Фильтр по месяцу:";
            // 
            // checkBoxFilterMonth
            // 
            checkBoxFilterMonth.AutoSize = true;
            checkBoxFilterMonth.Location = new Point(150, 18);
            checkBoxFilterMonth.Name = "checkBoxFilterMonth";
            checkBoxFilterMonth.Size = new Size(127, 24);
            checkBoxFilterMonth.TabIndex = 1;
            checkBoxFilterMonth.Text = "Только месяц";
            checkBoxFilterMonth.CheckedChanged += checkBoxFilterMonth_CheckedChanged;
            // 
            // dateTimePickerMonth
            // 
            dateTimePickerMonth.CustomFormat = "MM.yyyy";
            dateTimePickerMonth.Enabled = false;
            dateTimePickerMonth.Format = DateTimePickerFormat.Custom;
            dateTimePickerMonth.Location = new Point(275, 14);
            dateTimePickerMonth.Name = "dateTimePickerMonth";
            dateTimePickerMonth.ShowUpDown = true;
            dateTimePickerMonth.Size = new Size(120, 27);
            dateTimePickerMonth.TabIndex = 2;
            dateTimePickerMonth.ValueChanged += dateTimePickerMonth_ValueChanged;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new Point(410, 13);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(120, 30);
            buttonRefresh.TabIndex = 3;
            buttonRefresh.Text = "🔄 Обновить";
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // labelHint
            // 
            labelHint.AutoSize = true;
            labelHint.ForeColor = Color.DimGray;
            labelHint.Location = new Point(625, 22);
            labelHint.Name = "labelHint";
            labelHint.Size = new Size(0, 20);
            labelHint.TabIndex = 2;
            // 
            // button1
            // 
            button1.BackgroundImage = Properties.Resources.Без_названия1;
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.ImageAlign = ContentAlignment.TopLeft;
            button1.Location = new Point(913, 30);
            button1.Name = "button1";
            button1.Size = new Size(35, 29);
            button1.TabIndex = 3;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(608, 31);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(311, 27);
            textBox1.TabIndex = 5;
            // 
            // WorkScheduleForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 600);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(panelFilter);
            Controls.Add(dataGridViewSchedule);
            Controls.Add(panelEditor);
            Controls.Add(labelHint);
            MinimumSize = new Size(984, 640);
            Name = "WorkScheduleForm";
            Text = "График работы";
            ((System.ComponentModel.ISupportInitialize)dataGridViewSchedule).EndInit();
            panelEditor.ResumeLayout(false);
            panelEditor.PerformLayout();
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dataGridViewSchedule;
        private Panel panelEditor;
        private Panel panelFilter;
        private Label labelEmployee;
        private ComboBox comboBoxEmployee;
        private Label labelDate;
        private DateTimePicker dateTimePickerWork;
        private Label labelShift;
        private ComboBox comboBoxShift;
        private Button buttonAdd;
        private Button buttonDelete;
        private CheckBox checkBoxFilterMonth;
        private DateTimePicker dateTimePickerMonth;
        private Label labelFilterMonth;
        private Button buttonRefresh;
        private Label labelHint;
        private Button button1;
        private TextBox textBox1;
    }
}