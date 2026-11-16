namespace вход_курсач
{
    public partial class UserManagementForm : Form
    {
        private readonly int _currentUserId;
        private readonly int _currentUserStatus;
        private readonly DB_Users _db = new DB_Users();
        private readonly Design.ColumnSorter _sorter = new();
        private int? _selectedUserId;
        private int _currentUserFilial;

        // Список пользователей видят отдел кадров, администратор и генеральный директор;
        // остальным в этом окне показывается их личный график работы.
        private bool CanSeeUsers =>
            _currentUserStatus == Roles.Hr ||
            _currentUserStatus == Roles.Admin ||
            _currentUserStatus == Roles.Director;

        public UserManagementForm(int currentUserId, int currentUserStatus)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
            _currentUserStatus = currentUserStatus;

            this.Load += UserManagementForm_Load;
            this.Load += (s, e) => Design.GazpromStyle.StyleAll(this);
            dataGridViewUsers.ColumnHeaderMouseClick += DataGridViewUsers_ColumnHeaderMouseClick;
            dataGridViewUsers.SelectionChanged += DataGridViewUsers_SelectionChanged;
        }

        private void DataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count == 0)
            {
                _selectedUserId = null;
                return;
            }

            var row = dataGridViewUsers.SelectedRows[0];

            if (row.DataBoundItem is UserManagementItem item)
            {
                _selectedUserId = item.Id;

                textBoxLogin.Text = item.Login;
                textBoxPassword.Text = item.Password;
                textBoxName.Text = item.Name;
                textBoxSurname.Text = item.Surname;
                textBoxSecondName.Text = item.SecondName;
                numericTeam.Value = item.TeamNum;

                if (comboBoxRole.Items.Count > 0)
                    comboBoxRole.SelectedValue = item.Status;
            }
            else
            {
                _selectedUserId = null;
            }
        }

        private void DataGridViewUsers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridViewUsers.DataSource == null)
                return;

            string columnName = dataGridViewUsers.Columns[e.ColumnIndex].DataPropertyName;

            if (string.IsNullOrEmpty(columnName))
                return;

            // В таблице либо список пользователей, либо личный график работы
            if (dataGridViewUsers.DataSource is List<UserManagementItem> users)
                dataGridViewUsers.DataSource = _sorter.Sort(users, columnName);
            else if (dataGridViewUsers.DataSource is List<WorkScheduleItem> schedule)
                dataGridViewUsers.DataSource = _sorter.Sort(schedule, columnName);
        }

        private void UserManagementForm_Load(object? sender, EventArgs e)
        {
            LoadFilials();

            if (CanSeeUsers)
            {
                LoadRoles();
                RefreshUsers();

                if (_currentUserStatus == Roles.Hr)
                    labelHint.Text = "Отдел кадров: работа только со своим филиалом.";
                else if (_currentUserStatus == Roles.Admin)
                    labelHint.Text = "Администратор: полный доступ.";
                else
                    labelHint.Text = "Генеральный директор: видны все филиалы.";
            }
            else
            {
                RefreshWorkSchedule();
                labelHint.Text = "Вы видите только свой график работы.";
            }
        }

        private void LoadFilials()
        {
            try
            {
                _currentUserFilial = _db.GetUserFilial(_currentUserId);
            }
            catch { }
        }

        private void LoadRoles()
        {
            try
            {
                // Отдел кадров не может назначать начальника филиала и выше
                int maxStatus = _currentUserStatus == Roles.Hr ? Roles.Accountant : Roles.Director;

                var roles = _db.GetRoles()
                    .Where(r => r.Status > Roles.Request && r.Status <= maxStatus)
                    .ToList();

                comboBoxRole.DisplayMember = "RoleName";
                comboBoxRole.ValueMember = "Status";
                comboBoxRole.DataSource = roles;
                comboBoxRole.SelectedIndex = -1;
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
                dataGridViewUsers.DataSource = _db.GetUsersForManagement(_currentUserId);
                ConfigureUserGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
            }
        }

        private void RefreshWorkSchedule()
        {
            try
            {
                var schedule = new DB_WorkSchedule().GetUserWorkSchedule(_currentUserId);
                dataGridViewUsers.DataSource = schedule;
                ConfigureWorkScheduleGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки графика: {ex.Message}");
            }
        }

        private void ConfigureUserGrid()
        {
            var filialColumn = dataGridViewUsers.Columns["Filial"];
            if (filialColumn != null) filialColumn.Visible = false;

            var statusColumn = dataGridViewUsers.Columns["Status"];
            if (statusColumn != null) statusColumn.Visible = false;

            var isActiveColumn = dataGridViewUsers.Columns["IsActive"];
            if (isActiveColumn != null) isActiveColumn.Visible = false;

            RenameColumn("Id", "ID");
            RenameColumn("Login", "Логин");
            RenameColumn("Name", "Имя");
            RenameColumn("Surname", "Фамилия");
            RenameColumn("SecondName", "Отчество");
            RenameColumn("FilialName", "Филиал");
            RenameColumn("TeamNum", "Смена");
            RenameColumn("RoleName", "Роль");
        }

        private void ConfigureWorkScheduleGrid()
        {
            dataGridViewUsers.Columns.Clear();
            dataGridViewUsers.Columns.Add("WorkDate", "Дата");
            dataGridViewUsers.Columns.Add("ShiftType", "Смена");
            dataGridViewUsers.Columns.Add("CreatedAt", "Дата создания");
            dataGridViewUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void RenameColumn(string columnName, string headerText)
        {
            if (dataGridViewUsers.Columns[columnName] != null)
            {
                dataGridViewUsers.Columns[columnName].HeaderText = headerText;
            }
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