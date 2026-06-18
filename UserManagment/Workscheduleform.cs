namespace вход_курсач
{
    public partial class WorkScheduleForm : Form
    {
        private const int ManagerStatus = 5;

        private readonly int _currentUserId;
        private readonly int _currentUserStatus;
        private readonly DB _db;
        private int? _selectedScheduleId;
        private List<WorkScheduleItem> _allSchedule = new();

        public WorkScheduleForm(int currentUserId, int currentUserStatus)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
            _currentUserStatus = currentUserStatus;
            _db = new DB(Program.ConnectionString);
            Load += WorkScheduleForm_Load;
            Load += (s, e) => ApplyGazpromStyle();
        }

        private void ApplyGazpromStyle()
        {
            Design.GazpromStyle.StyleForm(this);
            foreach (Control ctrl in GetAllControls(this))
            {
                if (ctrl is Button btn)
                    Design.GazpromStyle.StyleButton(btn);
                else if (ctrl is TextBox txt)
                    Design.GazpromStyle.StyleTextBox(txt);
                else if (ctrl is Label lbl)
                    Design.GazpromStyle.StyleLabel(lbl);
                else if (ctrl is ComboBox cmb)
                    Design.GazpromStyle.StyleComboBox(cmb);
                else if (ctrl is DataGridView dgv)
                    Design.GazpromStyle.StyleDataGridView(dgv);
                else if (ctrl is GroupBox gb)
                    Design.GazpromStyle.StyleGroupBox(gb);
                else if (ctrl is Panel pnl && !(pnl is HeaderPanel))
                    Design.GazpromStyle.StylePanel(pnl);
            }
        }

        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                yield return ctrl;
                foreach (var child in GetAllControls(ctrl))
                    yield return child;
            }
        }

        private class HeaderPanel : Panel { }

        // ── загрузка формы ────────────────────────────────────────────────
        private void WorkScheduleForm_Load(object? sender, EventArgs e)
        {
            // Выпадающий список смен
            comboBoxShift.Items.Clear();
            comboBoxShift.Items.Add("Дневная");
            comboBoxShift.Items.Add("Ночная");
            comboBoxShift.SelectedIndex = 0;

            // Фильтр по месяцу — текущий месяц по умолчанию
            dateTimePickerMonth.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            // Дата назначения — сегодня
            dateTimePickerWork.Value = DateTime.Today;

            // Панель редактирования видна только начальникам и выше
            bool canEdit = _currentUserStatus >= ManagerStatus;
            panelEditor.Visible = canEdit;

            if (canEdit)
                LoadEmployees();

            RefreshGrid();
        }

        // ── загрузка сотрудников ──────────────────────────────────────────
        private void LoadEmployees()
        {
            try
            {
                var employees = _db.GetActiveEmployeesForSchedule(_currentUserId);
                comboBoxEmployee.DisplayMember = "FullName";
                comboBoxEmployee.ValueMember = "Id";

                // Добавляем вычисляемое FullName прямо здесь, чтобы не менять ClassUser
                var items = employees.Select(u => new
                {
                    u.Id,
                    FullName = $"{u.Surname} {u.Name}" +
                               (string.IsNullOrWhiteSpace(u.SecondName) ? "" : $" {u.SecondName}") +
                               $" (смена {u.TeamNum})"
                }).ToList();

                comboBoxEmployee.DataSource = items;
                comboBoxEmployee.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки сотрудников: {ex.Message}");
            }
        }

        // ── обновление таблицы ────────────────────────────────────────────
        private void RefreshGrid()
        {
            try
            {
                string? month = checkBoxFilterMonth.Checked
                    ? dateTimePickerMonth.Value.ToString("yyyy-MM")
                    : null;

                dataGridViewSchedule.DataSource = null;
                dataGridViewSchedule.Columns.Clear();
                _allSchedule = _db.GetSchedule(_currentUserId, month);
                dataGridViewSchedule.DataSource = _allSchedule;
                ConfigureGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки графика: {ex.Message}");
            }
        }

        private void ConfigureGrid()
        {
            // Скрываем технические поля
            HideColumn("Id");          // храним для удаления
            HideColumn("UserId");
            HideColumn("FilialId");

            RenameColumn("UserFullName", "Сотрудник");
            RenameColumn("TeamNum", "Смена №");
            RenameColumn("WorkDate", "Дата");
            RenameColumn("ShiftType", "Тип смены");
            RenameColumn("FilialName", "Филиал");

            // Цвет строк по типу смены
            dataGridViewSchedule.RowPrePaint -= OnRowPrePaint;
            dataGridViewSchedule.RowPrePaint += OnRowPrePaint;

            dataGridViewSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void OnRowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (dataGridViewSchedule.Rows[e.RowIndex].DataBoundItem is WorkScheduleItem item)
            {
                dataGridViewSchedule.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                    item.ShiftType == "Ночная"
                        ? Color.FromArgb(220, 230, 255)   // голубоватый — ночная
                        : Color.FromArgb(255, 250, 220);  // желтоватый  — дневная
            }
        }

        private void HideColumn(string name)
        {
            if (dataGridViewSchedule.Columns[name] != null)
                dataGridViewSchedule.Columns[name].Visible = false;
        }

        private void RenameColumn(string name, string header)
        {
            if (dataGridViewSchedule.Columns[name] != null)
                dataGridViewSchedule.Columns[name].HeaderText = header;
        }

        // ── выбор строки ──────────────────────────────────────────────────
        private void dataGridViewSchedule_SelectionChanged(object? sender, EventArgs e)
        {
            if (dataGridViewSchedule.SelectedRows.Count == 0) return;
            if (dataGridViewSchedule.SelectedRows[0].DataBoundItem is not WorkScheduleItem item) return;

            _selectedScheduleId = item.Id;
        }

        // ── кнопка «Добавить» ─────────────────────────────────────────────
        private void buttonAdd_Click(object? sender, EventArgs e)
        {
            if (comboBoxEmployee.SelectedValue == null)
            {
                MessageBox.Show("Выберите сотрудника.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int targetUserId = Convert.ToInt32(comboBoxEmployee.SelectedValue);
            DateTime workDate = dateTimePickerWork.Value.Date;
            string shiftType = comboBoxShift.SelectedItem?.ToString() ?? "Дневная";

            var result = _db.AddScheduleEntry(_currentUserId, targetUserId, workDate, shiftType);
            ShowResult(result, "Запись добавлена в график.");
        }

        // ── кнопка «Удалить» ──────────────────────────────────────────────
        private void buttonDelete_Click(object? sender, EventArgs e)
        {
            if (!_selectedScheduleId.HasValue)
            {
                MessageBox.Show("Выберите запись в таблице.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show("Удалить выбранную запись из графика?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            var result = _db.DeleteScheduleEntry(_currentUserId, _selectedScheduleId.Value);
            ShowResult(result, "Запись удалена.");
        }

        // ── кнопка «Обновить» / фильтр ────────────────────────────────────
        private void buttonRefresh_Click(object? sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void checkBoxFilterMonth_CheckedChanged(object? sender, EventArgs e)
        {
            dateTimePickerMonth.Enabled = checkBoxFilterMonth.Checked;
            RefreshGrid();
        }

        private void dateTimePickerMonth_ValueChanged(object? sender, EventArgs e)
        {
            if (checkBoxFilterMonth.Checked)
                RefreshGrid();
        }

        // ── вспомогательное ───────────────────────────────────────────────
        private void ShowResult(OperationResult result, string successMessage)
        {
            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(successMessage, "Готово",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            _selectedScheduleId = null;
            RefreshGrid();
        }

        private void button1_Click(object? sender, EventArgs e)
        {

            PerformSearch();
        }
        private void textBox1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
                e.SuppressKeyPress = true;
            }
        }

        private void PerformSearch()
        {
            if (_allSchedule == null || _allSchedule.Count == 0)
                return;

            string searchText = textBox1.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                dataGridViewSchedule.DataSource = _allSchedule;
                return;
            }

            var filtered = _allSchedule
                .Where(x => x.UserFullName != null &&
                            x.UserFullName.ToLower().Contains(searchText))
                .ToList();

            dataGridViewSchedule.DataSource = filtered;
        }
    }
}
