using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace вход_курсач
{
    public partial class Filials : Form
    {
        private readonly DB_Filials _db = new DB_Filials();

        public Filials(int UserId)
        {
            InitializeComponent();

            if (DesignMode) return;

            // Добавлять, изменять и удалять филиалы могут отдел кадров, начальник филиала и администратор
            var user = new DB_Users().GetUserById(UserId);
            bool canEdit = user != null &&
                (user.Status == Roles.Hr || user.Status == Roles.Manager || user.Status == Roles.Admin);

            lblName.Visible = canEdit;
            lblAddress.Visible = canEdit;
            lblContact.Visible = canEdit;
            txtName.Visible = canEdit;
            txtAddress.Visible = canEdit;
            txtContact.Visible = canEdit;
            Добавить.Visible = canEdit;
            Изменить.Visible = canEdit;
            Удалить.Visible = canEdit;

            RefreshGrid();

            this.Load += (s, e) => Design.GazpromStyle.StyleAll(this);
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
            // UseShellExecute нужен, чтобы ссылка открылась в браузере по умолчанию
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
                "https://belarus-tr.gazprom.ru/about/") { UseShellExecute = true });
        }
    }
}