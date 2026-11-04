using Dapper;
using Microsoft.Data.Sqlite;

namespace вход_курсач
{
    public partial class UserManagementForm : Form
    {
        private const int HrStatus = 2;
        private const int ManagerStatus = 5;
        private const int AdminStatus = 6;

        private readonly int _currentUserId;
        private readonly int _currentUserStatus;
        private readonly DB _db;
        private int? _selectedUserId;
        private int _currentUserFilial;

        public UserManagementForm(int currentUserId, int currentUserStatus)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
            _currentUserStatus = currentUserStatus;
            _db = new DB(Program.ConnectionString);
            Load += UserManagementForm_Load;
        }

        private void UserManagementForm_Load(object? sender, EventArgs e)
        {
            LoadFilials();
            LoadRoles();
            RefreshUsers();
            ApplyPermissionHints();
        }

        private void LoadFilials()
        {
            try
            {
                using (var conn = new SqliteConnection(Program.ConnectionString))
                {
                    _currentUserFilial = conn.QueryFirstOrDefault<int>(
                        "SELECT Filial FROM Users WHERE Id = @Id",
                        new { Id = _currentUserId });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки филиала: {ex.Message}");
            }
        }

        private void LoadRoles()
        {
            try
            {
                using (var conn = new SqliteConnection(Program.ConnectionString))
                {
                    string sql = _currentUserStatus == AdminStatus
                        ? "SELECT Status, RoleName FROM Status_and_RoleName WHERE Status > 0 ORDER BY Status"
                        : "SELECT Status, RoleName FROM Status_and_RoleName WHERE Status > 0 AND Status < 5 ORDER BY Status";

                    var roles = conn.Query(sql).ToList();
                    comboBoxRole.DisplayMember = "RoleName";
                    comboBoxRole.ValueMember = "Status";
                    comboBoxRole.DataSource = roles;
                    comboBoxRole.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }

        private void RefreshUsers()
        {
            try
            {
                dataGridViewUsers.DataSource = null;
                dataGridViewUsers.Columns.Clear();
                dataGridViewUsers.DataSource = _db.GetUsersForManagement(_currentUserId);
                ConfigureGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
            }
        }

        private void ConfigureGrid()
        {
            var filialColumn = dataGridViewUsers.Columns["Filial"];
            if (filialColumn != null)
                filialColumn.Visible = false;
            var statusColumn = dataGridViewUsers.Columns["Status"];
            if (statusColumn != null)
                statusColumn.Visible = false;
            var isActiveColumn = dataGridViewUsers.Columns["IsActive"];
            if (isActiveColumn != null)
                isActiveColumn.Visible = false;

            RenameColumn("Id", "ID");
            RenameColumn("Login", "Логин");
            RenameColumn("Name", "Имя");
            RenameColumn("Surname", "Фамилия");
            RenameColumn("SecondName", "Отчество");
            RenameColumn("FilialName", "Филиал");
            RenameColumn("TeamNum", "Смена");
            RenameColumn("RoleName", "Роль");
            RenameColumn("ProtectionNote", "Примечание");

            dataGridViewUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void RenameColumn(string columnName, string headerText)
        {
            if (dataGridViewUsers.Columns[columnName] != null)
            {
                dataGridViewUsers.Columns[columnName].HeaderText = headerText;
            }
        }

        private void ApplyPermissionHints()
        {
            if (_currentUserStatus == HrStatus)
            {
                labelHint.Text = "Отдел кадров: можно работать только со своим филиалом. Удаление начальников/замов и администраторов запрещено.";
            }
            else
            {
                labelHint.Text = "Администратор: можно добавлять, изменять и удалять пользователей, кроме 8 системных администраторов.";
            }
        }

        private void dataGridViewUsers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count == 0)
            {
                return;
            }

            if (dataGridViewUsers.SelectedRows[0].DataBoundItem is not UserManagementItem selected)
            {
                return;
            }

            _selectedUserId = selected.Id;
            textBoxLogin.Text = selected.Login;
            textBoxPassword.Text = string.Empty;
            textBoxName.Text = selected.Name;
            textBoxSurname.Text = selected.Surname;
            textBoxSecondName.Text = selected.SecondName;
            numericTeam.Value = selected.TeamNum >= numericTeam.Minimum && selected.TeamNum <= numericTeam.Maximum
                ? selected.TeamNum
                : numericTeam.Minimum;
            comboBoxRole.SelectedValue = selected.Status;
        }

        private void buttonAdd_Click(object? sender, EventArgs e)
        {
            var formData = ReadFormData();
            if (!formData.Success)
            {
                MessageBox.Show(formData.ErrorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _db.AddUser(
                _currentUserId,
                textBoxLogin.Text,
                textBoxPassword.Text,
                textBoxName.Text,
                textBoxSurname.Text,
                textBoxSecondName.Text,
                _currentUserFilial,
                formData.TeamNum,
                formData.Status);

            ShowResult(result, "Пользователь добавлен.");
        }

        private void buttonUpdate_Click(object? sender, EventArgs e)
        {
            if (!_selectedUserId.HasValue)
            {
                MessageBox.Show("Выберите пользователя из таблицы.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var formData = ReadFormData();
            if (!formData.Success)
            {
                MessageBox.Show(formData.ErrorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _db.UpdateUser(
                _currentUserId,
                _selectedUserId.Value,
                textBoxLogin.Text,
                textBoxPassword.Text,
                textBoxName.Text,
                textBoxSurname.Text,
                textBoxSecondName.Text,
                _currentUserFilial,
                formData.TeamNum,
                formData.Status);

            ShowResult(result, "Данные пользователя обновлены.");
        }

        private void buttonDelete_Click(object? sender, EventArgs e)
        {
            if (!_selectedUserId.HasValue)
            {
                MessageBox.Show("Выберите пользователя из таблицы.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult resultDialog = MessageBox.Show(
                "Удалить выбранного пользователя?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultDialog != DialogResult.Yes)
            {
                return;
            }

            var result = _db.DeleteUser(_currentUserId, _selectedUserId.Value);
            ShowResult(result, "Пользователь удалён.");
        }

        private void ShowResult(OperationResult result, string successMessage)
        {
            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(successMessage, "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshUsers();
            ClearEditor();
        }

        private UserFormData ReadFormData()
        {
            if (comboBoxRole.SelectedValue == null)
            {
                return UserFormData.Fail("Выберите роль.");
            }

            return UserFormData.Ok(
                (int)numericTeam.Value,
                Convert.ToInt32(comboBoxRole.SelectedValue));
        }

        private void ClearEditor()
        {
            _selectedUserId = null;
            textBoxLogin.Clear();
            textBoxPassword.Clear();
            textBoxName.Clear();
            textBoxSurname.Clear();
            textBoxSecondName.Clear();
            numericTeam.Value = 0;
            comboBoxRole.SelectedIndex = -1;
            dataGridViewUsers.ClearSelection();
        }

        private sealed class UserFormData
        {
            public bool Success { get; private set; }
            public string ErrorMessage { get; private set; } = string.Empty;
            public int TeamNum { get; private set; }
            public int Status { get; private set; }

            public static UserFormData Ok(int teamNum, int status)
            {
                return new UserFormData
                {
                    Success = true,
                    TeamNum = teamNum,
                    Status = status
                };
            }

            public static UserFormData Fail(string message)
            {
                return new UserFormData
                {
                    Success = false,
                    ErrorMessage = message
                };
            }
        }
    }
}