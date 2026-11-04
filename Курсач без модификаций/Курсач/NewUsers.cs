using Dapper;

namespace вход_курсач
{
    public partial class NewUsers : Form
    {
        private int CurrentUserId;

        public NewUsers(int userId)
        {
            InitializeComponent();
            CurrentUserId = userId;
            ListForGVB();
            this.Load += RoleLoad;
        }
        private void RoleLoad(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(Program.ConnectionString))
                {
                    var list = conn.Query("Select Status,RoleName from Status_and_RoleName").ToList();

                    comboBoxRole.DisplayMember = "RoleName";
                    comboBoxRole.ValueMember = "Status";
                    comboBoxRole.DataSource = list;
                    comboBoxRole.SelectedIndex = -1;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка чтения должностей");
            }
        }
        private void ListForGVB()
        {
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                DB dB = new DB(Program.ConnectionString);

                // Получаем список заявок для филиала текущего пользователя
                List<ClassUser> list = dB.ListApplication(CurrentUserId);

                if (list == null || list.Count == 0)
                {
                    MessageBox.Show("Нет заявок для вашего филиала");
                    return;
                }
                else
                {
                    dataGridView1.DataSource = list;
                    // Настройка колонок
                    ConfigureDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void ConfigureDataGridView()
        {
            if (dataGridView1.Columns["Password"] != null)
                dataGridView1.Columns["Password"].Visible = false;
            if (dataGridView1.Columns["IsActive"] != null)
                dataGridView1.Columns["IsActive"].Visible = false;

            if (dataGridView1.Columns["RequestId"] != null)
                dataGridView1.Columns["RequestId"].HeaderText = "№ заявки";
            if (dataGridView1.Columns["Id"] != null)
                dataGridView1.Columns["Id"].HeaderText = "ID";
            if (dataGridView1.Columns["Name"] != null)
                dataGridView1.Columns["Name"].HeaderText = "Имя";
            if (dataGridView1.Columns["Surname"] != null)
                dataGridView1.Columns["Surname"].HeaderText = "Фамилия";
            if (dataGridView1.Columns["SecondName"] != null)
                dataGridView1.Columns["SecondName"].HeaderText = "Отчество";
            if (dataGridView1.Columns["Login"] != null)
                dataGridView1.Columns["Login"].HeaderText = "Логин";
            if (dataGridView1.Columns["FilialName"] != null)
                dataGridView1.Columns["FilialName"].HeaderText = "Филиал";
            if (dataGridView1.Columns["TeamNum"] != null)
                dataGridView1.Columns["TeamNum"].HeaderText = "Смена";
            if (dataGridView1.Columns["Status"] != null)
                dataGridView1.Columns["Status"].HeaderText = "Статус";
            if (dataGridView1.Columns["RequestType"] != null)
                dataGridView1.Columns["RequestType"].Visible = false;
        }

        private void AddBT_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ClassUser newUser = (ClassUser)dataGridView1.SelectedRows[0].DataBoundItem;
            DB dB = new DB(Program.ConnectionString);
            long newStatus = (long)comboBoxRole.SelectedValue;
            bool success = dB.ApproveRequest(newUser.Id, (int)newStatus, CurrentUserId);
            MessageBox.Show("Заявка одобрена успешно", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            ListForGVB();
        }

        private void DeleteRequests_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Удалить выбранную заявку?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClassUser selectedUser = (ClassUser)dataGridView1.SelectedRows[0].DataBoundItem;

                DB dB = new DB(Program.ConnectionString);
                bool success = dB.DeleteRequest(selectedUser.Id);

                if (success)
                {
                    MessageBox.Show("Заявка удалена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListForGVB();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}