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
            checkBoxFilterMonth = new CheckBox();
            dateTimePickerMonth = new DateTimePicker();
            labelFilterMonth = new Label();
            buttonRefresh = new Button();
            labelHint = new Label();

            ((System.ComponentModel.ISupportInitialize)dataGridViewSchedule).BeginInit();
            panelEditor.SuspendLayout();
            panelFilter.SuspendLayout();
            SuspendLayout();

            // ── dataGridViewSchedule ──────────────────────────────────────
            dataGridViewSchedule.AllowUserToAddRows = false;
            dataGridViewSchedule.AllowUserToDeleteRows = false;
            dataGridViewSchedule.ReadOnly = true;
            dataGridViewSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSchedule.MultiSelect = false;
            dataGridViewSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewSchedule.Location = new Point(12, 80);
            dataGridViewSchedule.Name = "dataGridViewSchedule";
            dataGridViewSchedule.Size = new Size(960, 420);
            dataGridViewSchedule.TabIndex = 0;
            dataGridViewSchedule.SelectionChanged += dataGridViewSchedule_SelectionChanged;

            // ── panelFilter ───────────────────────────────────────────────
            panelFilter.BorderStyle = BorderStyle.FixedSingle;
            panelFilter.Location = new Point(12, 12);
            panelFilter.Size = new Size(600, 55);
            panelFilter.Name = "panelFilter";

            // labelFilterMonth
            labelFilterMonth.AutoSize = true;
            labelFilterMonth.Location = new Point(8, 18);
            labelFilterMonth.Text = "Фильтр по месяцу:";

            // checkBoxFilterMonth
            checkBoxFilterMonth.AutoSize = true;
            checkBoxFilterMonth.Location = new Point(150, 18);
            checkBoxFilterMonth.Text = "Только месяц";
            checkBoxFilterMonth.CheckedChanged += checkBoxFilterMonth_CheckedChanged;

            // dateTimePickerMonth
            dateTimePickerMonth.Format = DateTimePickerFormat.Custom;
            dateTimePickerMonth.CustomFormat = "MM.yyyy";
            dateTimePickerMonth.ShowUpDown = true;
            dateTimePickerMonth.Enabled = false;
            dateTimePickerMonth.Location = new Point(275, 14);
            dateTimePickerMonth.Size = new Size(120, 28);
            dateTimePickerMonth.ValueChanged += dateTimePickerMonth_ValueChanged;

            // buttonRefresh
            buttonRefresh.Text = "🔄 Обновить";
            buttonRefresh.Location = new Point(410, 13);
            buttonRefresh.Size = new Size(120, 30);
            buttonRefresh.Click += buttonRefresh_Click;

            panelFilter.Controls.AddRange(new Control[]
            {
                labelFilterMonth, checkBoxFilterMonth,
                dateTimePickerMonth, buttonRefresh
            });

            // ── panelEditor ───────────────────────────────────────────────
            panelEditor.BorderStyle = BorderStyle.FixedSingle;
            panelEditor.Location = new Point(12, 515);
            panelEditor.Size = new Size(960, 70);
            panelEditor.Name = "panelEditor";

            // labelEmployee
            labelEmployee.AutoSize = true;
            labelEmployee.Location = new Point(8, 22);
            labelEmployee.Text = "Сотрудник:";

            // comboBoxEmployee
            comboBoxEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEmployee.FormattingEnabled = true;
            comboBoxEmployee.Location = new Point(90, 18);
            comboBoxEmployee.Size = new Size(300, 28);
            comboBoxEmployee.TabIndex = 1;

            // labelDate
            labelDate.AutoSize = true;
            labelDate.Location = new Point(405, 22);
            labelDate.Text = "Дата:";

            // dateTimePickerWork
            dateTimePickerWork.Format = DateTimePickerFormat.Short;
            dateTimePickerWork.Location = new Point(450, 18);
            dateTimePickerWork.Size = new Size(130, 28);
            dateTimePickerWork.TabIndex = 2;

            // labelShift
            labelShift.AutoSize = true;
            labelShift.Location = new Point(595, 22);
            labelShift.Text = "Смена:";

            // comboBoxShift
            comboBoxShift.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxShift.FormattingEnabled = true;
            comboBoxShift.Location = new Point(648, 18);
            comboBoxShift.Size = new Size(120, 28);
            comboBoxShift.TabIndex = 3;

            // buttonAdd
            buttonAdd.Text = "➕ Добавить";
            buttonAdd.Location = new Point(785, 17);
            buttonAdd.Size = new Size(80, 30);
            buttonAdd.TabIndex = 4;
            buttonAdd.Click += buttonAdd_Click;

            // buttonDelete
            buttonDelete.Text = "🗑 Удалить";
            buttonDelete.Location = new Point(875, 17);
            buttonDelete.Size = new Size(75, 30);
            buttonDelete.TabIndex = 5;
            buttonDelete.BackColor = Color.FromArgb(255, 200, 200);
            buttonDelete.Click += buttonDelete_Click;

            panelEditor.Controls.AddRange(new Control[]
            {
                labelEmployee, comboBoxEmployee,
                labelDate, dateTimePickerWork,
                labelShift, comboBoxShift,
                buttonAdd, buttonDelete
            });

            // ── labelHint ─────────────────────────────────────────────────
            labelHint.AutoSize = true;
            labelHint.Location = new Point(625, 22);
            labelHint.Size = new Size(350, 40);
            labelHint.ForeColor = Color.DimGray;
            labelHint.Text = "";

            // ── WorkScheduleForm ──────────────────────────────────────────
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 600);
            Text = "График работы";
            MinimumSize = new Size(984, 640);

            Controls.AddRange(new Control[]
            {
                panelFilter,
                dataGridViewSchedule,
                panelEditor,
                labelHint
            });

            ((System.ComponentModel.ISupportInitialize)dataGridViewSchedule).EndInit();
            panelEditor.ResumeLayout(false);
            panelFilter.ResumeLayout(false);
            ResumeLayout(false);
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
    }
}