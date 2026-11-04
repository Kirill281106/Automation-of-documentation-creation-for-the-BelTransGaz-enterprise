using Dapper;

namespace вход_курсач
{
    public partial class Filials : Form
    {
        private readonly DB _db = new DB(Program.ConnectionString);

        public Filials()
        {
            InitializeComponent();

            // Не обращаемся к БД во время работы дизайнера Visual Studio
            if (DesignMode) return;

            LoadEmployeesComboBox();
            RefreshGrid();
        }

        // ─── Загрузка ComboBox сотрудников ───────────────────────────────────
        private void LoadEmployeesComboBox()
        {
            var employees = _db.GetActiveEmployees();
            var noManager = new EmployeeItem { Id = 0, FullName = "— Не назначен —" };
            var list = new List<EmployeeItem> { noManager };
            list.AddRange(employees);

            cmbManager.DataSource = list;
            cmbManager.DisplayMember = "FullName";
            cmbManager.ValueMember = "Id";
            cmbManager.SelectedIndex = 0;
        }

        // ─── Обновление таблицы ──────────────────────────────────────────────
        private void RefreshGrid()
        {
            var data = _db.GetFilialsForDisplay();
            dataGridView1.DataSource = data;

            if (dataGridView1.Columns["FilialName"] != null)
                dataGridView1.Columns["FilialName"].HeaderText = "Филиал";
            if (dataGridView1.Columns["Address"] != null)
                dataGridView1.Columns["Address"].HeaderText = "Адрес";
            if (dataGridView1.Columns["Contact"] != null)
                dataGridView1.Columns["Contact"].HeaderText = "Контакты";
            if (dataGridView1.Columns["ManagerFullName"] != null)
                dataGridView1.Columns["ManagerFullName"].HeaderText = "Начальник филиала";

            dataGridView1.AutoResizeColumns();
            ClearFields();
        }

        // ─── Очистка полей ввода ─────────────────────────────────────────────
        private void ClearFields()
        {
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtContact.Text = string.Empty;
            if (cmbManager.Items.Count > 0)
                cmbManager.SelectedIndex = 0;
        }

        // ─── Клик по строке — заполняем поля ─────────────────────────────────
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (DesignMode || dataGridView1.CurrentRow == null) return;

            var row = dataGridView1.CurrentRow.DataBoundItem as FilialDisplayItem;
            if (row == null) return;

            var edit = _db.GetFilialByName(row.FilialName);
            if (edit == null) return;

            txtName.Text = edit.Name;
            txtAddress.Text = edit.Address;
            txtContact.Text = edit.Contact;

            if (edit.ManagerId.HasValue)
            {
                var found = (cmbManager.DataSource as List<EmployeeItem>)
                    ?.FirstOrDefault(e => e.Id == edit.ManagerId.Value);
                if (found != null)
                    cmbManager.SelectedItem = found;
            }
            else
            {
                cmbManager.SelectedIndex = 0;
            }
        }


        // ─── Изменить ────────────────────────────────────────────────────────
        private void Изменить_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку для изменения.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dataGridView1.CurrentRow.DataBoundItem as FilialDisplayItem;
            if (row == null) return;

            var edit = _db.GetFilialByName(row.FilialName);
            if (edit == null)
            {
                MessageBox.Show("Филиал не найден.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int? managerId = GetSelectedManagerId();

            var result = _db.UpdateFilial(
                edit.Id, txtName.Text, txtAddress.Text, txtContact.Text, managerId);

            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Филиал успешно изменён.", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadEmployeesComboBox();
            RefreshGrid();
        }

        // ─── Удалить ─────────────────────────────────────────────────────────
        private void Удалить_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку для удаления.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dataGridView1.CurrentRow.DataBoundItem as FilialDisplayItem;
            if (row == null) return;

            var confirm = MessageBox.Show(
                $"Удалить филиал «{row.FilialName}»?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            var edit = _db.GetFilialByName(row.FilialName);
            if (edit == null)
            {
                MessageBox.Show("Филиал не найден.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = _db.DeleteFilial(edit.Id);
            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Филиал удалён.", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshGrid();
        }

        // ─── Helper ──────────────────────────────────────────────────────────
        private int? GetSelectedManagerId()
        {
            var emp = cmbManager.SelectedItem as EmployeeItem;
            return (emp == null || emp.Id == 0) ? (int?)null : emp.Id;
        }

        private void Добавить_Click_1(object sender, EventArgs e)
        {
            int? managerId = GetSelectedManagerId();

            var result = _db.AddFilial(
                txtName.Text, txtAddress.Text, txtContact.Text, managerId);

            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Филиал успешно добавлен.", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadEmployeesComboBox();
            RefreshGrid();
        }
    }
}
