using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace вход_курсач
{
    public partial class Filials : Form
    {
        private readonly DB _db = new DB(Program.ConnectionString);
        private readonly DB_Quarterly _db_q;   // добавляем инициализацию

        public Filials(int UserId)
        {
            InitializeComponent();

            if (DesignMode) return;

            _db_q = new DB_Quarterly(Program.ConnectionString);   // ← ИСПРАВЛЕНИЕ

            var user = _db_q.GetUserById(UserId);
            if (user != null && (user.Status == 2 || user.Status == 6 || user.Status == 5))
            {
                lblName.Visible = true;
                lblAddress.Visible = true;
                lblContact.Visible = true;
                txtName.Visible = true;
                txtAddress.Visible = true;
                txtContact.Visible = true;
                Добавить.Visible = true;
                Изменить.Visible = true;
                Удалить.Visible = true;
            }
            else
            {
                // Скрываем всё для обычных пользователей
                lblName.Visible = false;
                lblAddress.Visible = false;
                lblContact.Visible = false;
                txtName.Visible = false;
                txtAddress.Visible = false;
                txtContact.Visible = false;
                Добавить.Visible = false;
                Изменить.Visible = false;
                Удалить.Visible = false;
            }

            LoadEmployeesComboBox();
            RefreshGrid();

            this.Load += (s, e) => ApplyGazpromStyle();
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

        // Загрузка ComboBox сотрудников 
        private void LoadEmployeesComboBox()
        {
            var employees = _db.GetActiveEmployees();
            var noManager = new EmployeeItem { Id = 0, FullName = "— Не назначен —" };
            var list = new List<EmployeeItem> { noManager };
            list.AddRange(employees);

            // Если ComboBox для выбора начальника остался — заполняем его
            // (если ты его удалил — этот метод можно удалить)
        }

        // Обновление таблицы 
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
            {
                dataGridView1.Columns["ManagerFullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["ManagerFullName"].HeaderText = "Начальник филиала";
            }

            dataGridView1.AutoResizeColumns();
            ClearFields();
        }

        // Очистка полей ввода
        private void ClearFields()
        {
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtContact.Text = string.Empty;
        }

        // Клик по строке — заполняем поля для редактирования
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
        }

        // Добавить
        private void Добавить_Click_1(object sender, EventArgs e)
        {
            var result = _db.AddFilial(txtName.Text, txtAddress.Text, txtContact.Text);

            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Филиал успешно добавлен.\nНачальник не назначен (можно назначить позже).",
                "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            RefreshGrid();
            ClearFields();
        }

        // Изменить
        private void Изменить_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку для изменения.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dataGridView1.CurrentRow.DataBoundItem as FilialDisplayItem;
            if (row == null) return;

            var edit = _db.GetFilialByName(row.FilialName);
            if (edit == null)
            {
                MessageBox.Show("Филиал не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = _db.UpdateFilial(edit.Id, txtName.Text, txtAddress.Text, txtContact.Text);
            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Филиал успешно изменён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshGrid();
        }

        // Удалить
        private void Удалить_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dataGridView1.CurrentRow.DataBoundItem as FilialDisplayItem;
            if (row == null) return;

            if (MessageBox.Show($"Удалить филиал «{row.FilialName}»?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var edit = _db.GetFilialByName(row.FilialName);
            if (edit == null)
            {
                MessageBox.Show("Филиал не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = _db.DeleteFilial(edit.Id);
            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Филиал успешно удалён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshGrid();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://belarus-tr.gazprom.ru/about/");
        }
    }
}