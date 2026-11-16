namespace вход_курсач
{
    public partial class NewUsers : Form
    {
        private readonly int CurrentUserId;
        private readonly DB_Requests _db = new DB_Requests();
        private readonly Design.ColumnSorter _sorter = new();
        private List<ClassUser> _currentList = new();

        public NewUsers(int userId)
        {
            InitializeComponent();
            CurrentUserId = userId;
            ListForGVB();
            this.Load += RoleLoad;
            this.Load += (s, e) => ApplyGazpromStyle();
            dataGridView1.ColumnHeaderMouseClick += DataGridView_ColumnHeaderMouseClick;
        }

        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentList == null || _currentList.Count == 0)
                return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

            if (string.IsNullOrEmpty(columnName))
                return;

            _currentList = _sorter.Sort(_currentList, columnName);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _currentList;
        }

        private void ApplyGazpromStyle()
        {
            Design.GazpromStyle.StyleForm(this);
            Design.GazpromStyle.StyleButton(AddBT);
            Design.GazpromStyle.StyleButton(DeleteRequests);
            Design.GazpromStyle.StyleComboBox(comboBoxRole);
            Design.GazpromStyle.StyleDataGridView(dataGridView1);
        }
        private void RoleLoad(object sender, EventArgs e)
        {
            try
            {
                comboBoxRole.DisplayMember = "RoleName";
                comboBoxRole.ValueMember = "Status";
                comboBoxRole.DataSource = new DB_Users().GetRoles();
                comboBoxRole.SelectedIndex = 0;
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

                _currentList = _db.ListApplication(CurrentUserId);

                if (_currentList == null || _currentList.Count == 0)
                {
                    MessageBox.Show("Нет заявок для вашего филиала");
                    return;
                }

                dataGridView1.DataSource = _currentList;

                ConfigureDataGridView();
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

            // Проверка: выбрана ли роль в комбобоксе
            if (comboBoxRole.SelectedValue == null)
            {
                MessageBox.Show("Поле не введено. Выберите статус.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ClassUser newUser = (ClassUser)dataGridView1.SelectedRows[0].DataBoundItem;
            _db.ApproveRequest(newUser.Id, Convert.ToInt32(comboBoxRole.SelectedValue));

            MessageBox.Show("Заявка одобрена успешно", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                if (_db.DeleteRequest(selectedUser.Id))
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