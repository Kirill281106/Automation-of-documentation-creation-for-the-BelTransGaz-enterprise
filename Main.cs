using System.Data;

namespace вход_курсач
{
    public partial class Main : Form
    {
        public int Status;
        public int UserId;

        public Main(int Status, int userId)
        {
            InitializeComponent();
            this.Status = Status;
            VisibleComponents();
            this.UserId = userId;
            this.Load += (s, e) => ApplyGazpromStyle();
            this.Load += (s, e) => Main_Load(s, e);
        }
        private void VisibleComponents()
        {
            if (this.Status == 1)
            {
                бэкапБдToolStripMenuItem.Visible = false;
                квартальныйToolStripMenuItem.Visible = false;
                годовойToolStripMenuItem.Visible = false;
                планПоставкиToolStripMenuItem.Visible = false;
                отчётОРаботеКомпанииToolStripMenuItem.Visible = false;
                пользователиToolStripMenuItem.Visible = false;
                пользователиToolStripMenuItem.Visible = false;
            }
            if (this.Status==2)
            {
                бэкапБдToolStripMenuItem.Visible = false;
                ОтчётToolStripMenuItem.Visible = false;
                квартальныйToolStripMenuItem.Visible = false;
                сменныйToolStripMenuItem.Visible = false;
                годовойToolStripMenuItem.Visible = false;
                планПоставкиToolStripMenuItem.Visible = false;
                отчётОРаботеКомпанииToolStripMenuItem.Visible = false;
            }
            if (this.Status == 3)
            {
                бэкапБдToolStripMenuItem.Visible = false;
                квартальныйToolStripMenuItem.Visible = false;
                годовойToolStripMenuItem.Visible = false;
                пользователиToolStripMenuItem.Visible = false;
                отчётОРаботеКомпанииToolStripMenuItem.Visible = false;
                пользователиToolStripMenuItem.Visible = false;
            }
            if (this.Status == 4)
            {
                бэкапБдToolStripMenuItem.Visible = false;
                отчётОРаботеКомпанииToolStripMenuItem.Visible = false;
                пользователиToolStripMenuItem.Visible = false;
            }
            if (this.Status == 5)
            {
                бэкапБдToolStripMenuItem.Visible = false;
                заявкиНаСозданиеНовогоПользователяToolStripMenuItem.Visible = false;
            }
            if (this.Status == 7)
            {
                ОтчётToolStripMenuItem.Visible = false;
                бэкапБдToolStripMenuItem.Visible = false;
                графикРаботыToolStripMenuItem.Visible = false;
                планПоставкиToolStripMenuItem.Visible = false;
                заявкиНаСозданиеНовогоПользователяToolStripMenuItem.Visible = false;
            }
        }
        private void ApplyGazpromStyle()
        {
            Design.GazpromStyle.StyleForm(this);
            StyleMenuStrip();

            // Стилизуем элементы плана
            if (btnSavePlanToDB != null) Design.GazpromStyle.StyleButton(btnSavePlanToDB);
            if (lblTotalPlan != null) Design.GazpromStyle.StyleLabel(lblTotalPlan);
            if (lblAveragePlan != null) Design.GazpromStyle.StyleLabel(lblAveragePlan);
            if (cmbPlanMonth != null) Design.GazpromStyle.StyleComboBox(cmbPlanMonth);
            if (cmbPlanYear != null) Design.GazpromStyle.StyleComboBox(cmbPlanYear);
            if (dgvDailyPlan != null) Design.GazpromStyle.StyleDataGridView(dgvDailyPlan);
        }

        private void StyleMenuStrip()
        {
            menuStrip1.BackColor = Design.GazpromStyle.PrimaryBlue;
            menuStrip1.ForeColor = Color.White;
            menuStrip1.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                StyleMenuItems(item);
            }
        }

        private void StyleMenuItems(ToolStripMenuItem item)
        {
            item.BackColor = Design.GazpromStyle.PrimaryBlue;
            item.ForeColor = Color.White;
            item.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            foreach (ToolStripMenuItem subItem in item.DropDownItems.OfType<ToolStripMenuItem>())
            {
                subItem.BackColor = Design.GazpromStyle.LightGray;
                subItem.ForeColor = Design.GazpromStyle.DarkGray;
                StyleMenuItems(subItem);
            }
        }

        private void заявкиНаСозданиеНовогоПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewUsers newUsers = new NewUsers(this.UserId);
            this.Hide();
            newUsers.ShowDialog();
            this.Show();
        }

        private void учётныеЗаписиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserManagementForm form = new UserManagementForm(this.UserId, this.Status);
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void графикРаботыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkScheduleForm workScheduleForm = new WorkScheduleForm(this.UserId, this.Status);
            this.Hide();
            workScheduleForm.ShowDialog();
            this.Show();
        }

        private void информацияОПредприToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filials f = new Filials(this.UserId);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void сменныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaportShift raport = new RaportShift(this.UserId);
            this.Hide();
            raport.ShowDialog();
            this.Show();
        }

        private void квартальныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateQuarterlyReportForm createQuarterlyReportForm = new CreateQuarterlyReportForm(this.UserId);
            this.Hide();
            createQuarterlyReportForm.ShowDialog();
            this.Show();
        }

        private void годовойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YearlyReportForm form = new YearlyReportForm(this.UserId);
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void отчётОРаботеКомпанииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompanyYearlyReportForm form = new CompanyYearlyReportForm();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void планПоставкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cmbPlanMonth.Visible = true;
            cmbPlanYear.Visible = true;
            lblTotalPlan.Visible = true;
            lblAveragePlan.Visible = true;
            btnSavePlanToDB.Visible = true;
            dgvDailyPlan.Visible = true;
        }

        private void бэкапБдToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sourceDb = Program.ConnectionString.Replace("Data Source=", "").Trim();

            if (!System.IO.File.Exists(sourceDb))
            {
                MessageBox.Show("Файл базы данных не найден!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Title = "Создать резервную копию БД",
                Filter = "Файлы SQLite (*.db)|*.db",
                FileName = $"TPPO_backup_{DateTime.Now:yyyyMMdd_HHmm}.db"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.Copy(sourceDb, dlg.FileName, true);
                    MessageBox.Show("Резервная копия успешно создана!\nПуть: " + dlg.FileName,
                        "Бэкап", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при копировании: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private WorkPlanDB _workPlanDB;
        private int _currentFilialId;
        private DataTable _currentPlanTable = null;
        private bool _isViewMode = false;

        private void Main_Load(object sender, EventArgs e)
        {
            _workPlanDB = new WorkPlanDB(Program.ConnectionString);

            // Получаем филиал текущего пользователя
            using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new Microsoft.Data.Sqlite.SqliteCommand(
                    "SELECT Filial FROM Users WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", this.UserId);
                    var result = cmd.ExecuteScalar();
                    _currentFilialId = result != null ? Convert.ToInt32(result) : 0;
                }
            }

            LoadPlanMonths();
            LoadPlanYears();

            // Подписываемся на события
            cmbPlanMonth.SelectedIndexChanged += (s, ev) => RefreshPlanDisplay();
            cmbPlanYear.SelectedIndexChanged += (s, ev) => RefreshPlanDisplay();

            // Загружаем текущий план
            RefreshPlanDisplay();
        }

        // Заполнение месяцев
        private void LoadPlanMonths()
        {
            cmbPlanMonth.Items.Clear();
            string[] months = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                        "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

            for (int i = 0; i < 12; i++)
            {
                cmbPlanMonth.Items.Add(new { Text = months[i], Value = i + 1 });
            }

            cmbPlanMonth.DisplayMember = "Text";
            cmbPlanMonth.ValueMember = "Value";
            cmbPlanMonth.SelectedIndex = DateTime.Today.Month - 1;
        }

        // Заполнение годов
        private void LoadPlanYears()
        {
            cmbPlanYear.Items.Clear();
            int current = DateTime.Today.Year;
            cmbPlanYear.Items.Add(current);
            cmbPlanYear.Items.Add(current + 1);
            cmbPlanYear.SelectedItem = current;
        }

        // Обновление отображения плана
        private void RefreshPlanDisplay()
        {
            if (cmbPlanMonth.SelectedItem != null && cmbPlanYear.SelectedItem != null)
            {
                int month = ((dynamic)cmbPlanMonth.SelectedItem).Value;
                int year = (int)cmbPlanYear.SelectedItem;
                LoadPlanFromDatabase(year, month);
            }
        }

        private void LoadPlanFromDatabase(int year, int month)
        {
            // Определяем режим - можно ли редактировать
            var today = DateTime.Today;
            var selectedDate = new DateTime(year, month, 1);
            _isViewMode = selectedDate < new DateTime(today.Year, today.Month, 1);

            // Если это прошлый месяц или раньше - только просмотр
            if (_isViewMode)
            {
                dgvDailyPlan.ReadOnly = true;
                btnSavePlanToDB.Enabled = false;
                btnSavePlanToDB.BackColor = Color.Gray;
            }
            else
            {
                dgvDailyPlan.ReadOnly = false;
                btnSavePlanToDB.Enabled = true;
                btnSavePlanToDB.BackColor = Design.GazpromStyle.PrimaryBlue;
            }

            _currentPlanTable = new DataTable();
            _currentPlanTable.Columns.Add("День", typeof(int));
            _currentPlanTable.Columns.Add("План (м³)", typeof(decimal));
            _currentPlanTable.Columns.Add("Статус", typeof(string));

            var plan = _workPlanDB.GetMonthPlan(_currentFilialId, month, year);
            var today_date = DateTime.Today;

            foreach (var item in plan)
            {
                var planDate = new DateTime(year, month, item.DayNumber);
                string status = "";

                if (planDate < today_date)
                    status = "📅 Прошлый период";
                else if (planDate == today_date)
                    status = "🟢 Сегодня";
                else if (_workPlanDB.IsOverPlanningLimit(month, year) && planDate > today_date.AddMonths(3))
                    status = "🔒 Слишком далеко";
                else
                    status = "✏️ Можно редактировать";

                _currentPlanTable.Rows.Add(item.DayNumber, item.PlannedVolume, status);
            }

            dgvDailyPlan.DataSource = _currentPlanTable;
            SetupPlanGrid();
            CalculatePlanTotals();
        }

        private void SetupPlanGrid()
        {
            if (dgvDailyPlan.Columns.Count == 0) return;

            dgvDailyPlan.Columns[0].HeaderText = "День";
            dgvDailyPlan.Columns[0].Width = 60;
            dgvDailyPlan.Columns[0].ReadOnly = true;

            dgvDailyPlan.Columns[1].HeaderText = "План (м³)";
            dgvDailyPlan.Columns[1].DefaultCellStyle.Format = "N0";
            dgvDailyPlan.Columns[1].Width = 120;

            // В режиме просмотра - только чтение
            dgvDailyPlan.Columns[1].ReadOnly = _isViewMode;

            dgvDailyPlan.Columns[2].HeaderText = "Статус";
            dgvDailyPlan.Columns[2].Width = 150;
            dgvDailyPlan.Columns[2].ReadOnly = true;

            // Отписываемся от старого события, чтобы не было дублей
            dgvDailyPlan.RowPrePaint -= DgvDailyPlan_RowPrePaint;
            dgvDailyPlan.CellEndEdit -= DgvDailyPlan_CellEndEdit;

            // Подписываемся заново
            dgvDailyPlan.RowPrePaint += DgvDailyPlan_RowPrePaint;
            dgvDailyPlan.CellEndEdit += DgvDailyPlan_CellEndEdit;
        }

        private void DgvDailyPlan_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDailyPlan.Rows[e.RowIndex].Cells[2].Value == null) return;

            string status = dgvDailyPlan.Rows[e.RowIndex].Cells[2].Value.ToString();

            if (status.Contains("Прошлый"))
            {
                dgvDailyPlan.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                dgvDailyPlan.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
            }
            else if (status.Contains("Сегодня"))
            {
                dgvDailyPlan.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                dgvDailyPlan.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else if (status.Contains("Слишком далеко"))
            {
                dgvDailyPlan.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                dgvDailyPlan.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkGoldenrod;
                dgvDailyPlan.Rows[e.RowIndex].Cells[1].ReadOnly = true;
            }
            else if (status.Contains("Можно редактировать"))
            {
                dgvDailyPlan.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                dgvDailyPlan.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void DgvDailyPlan_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                CalculatePlanTotals();
            }
        }

        private void CalculatePlanTotals()
        {
            if (_currentPlanTable == null) return;

            decimal total = 0;
            int daysWithPlan = 0;

            foreach (DataRow row in _currentPlanTable.Rows)
            {
                var value = Convert.ToDecimal(row[1]);
                total += value;
                if (value > 0) daysWithPlan++;
            }

            decimal avg = daysWithPlan > 0 ? total / daysWithPlan : 0;

            lblTotalPlan.Text = $"Всего запланировано: {total:N0} м³";
            lblAveragePlan.Text = $"Средний план в день: {avg:N0} м³";
        }

        private void btnSavePlanToDB_Click(object sender, EventArgs e)
        {
            SavePlanToDatabase();
        }

        private void SavePlanToDatabase()
        {
            if (_isViewMode)
            {
                MessageBox.Show("Невозможно сохранить изменения в прошлых периодах. Переключитесь на будущий месяц.",
                    "Режим просмотра", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_currentPlanTable == null || _currentPlanTable.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для сохранения", "Ошибка");
                return;
            }

            if (cmbPlanMonth.SelectedItem == null || cmbPlanYear.SelectedItem == null)
            {
                MessageBox.Show("Выберите месяц и год", "Внимание");
                return;
            }

            int month = ((dynamic)cmbPlanMonth.SelectedItem).Value;
            int year = (int)cmbPlanYear.SelectedItem;

            // Проверяем лимит планирования
            if (_workPlanDB.IsOverPlanningLimit(month, year))
            {
                MessageBox.Show("Нельзя планировать на такую дату. Максимум планирования - 3 месяца вперёд.", "Ошибка");
                return;
            }

            int savedCount = 0;
            int skipCount = 0;

            foreach (DataRow row in _currentPlanTable.Rows)
            {
                int day = Convert.ToInt32(row[0]);
                decimal volume = Convert.ToDecimal(row[1]);
                string status = row[2].ToString();

                if (status.Contains("Прошлый"))
                {
                    skipCount++;
                    continue;
                }

                if (status.Contains("максимум на 3 месяца вперёд"))
                {
                    skipCount++;
                    continue;
                }

                if (!_workPlanDB.CanEditPlan(_currentFilialId, month, year, day))
                {
                    skipCount++;
                    continue;
                }

                if (_workPlanDB.SaveDayPlan(_currentFilialId, month, year, day, volume, this.UserId))
                    savedCount++;
            }

            if (skipCount > 0)
            {
                MessageBox.Show($"Сохранено: {savedCount} дней\nПропущено (прошлые даты): {skipCount} дней\n\nТекущий месяц и будущие периоды можно редактировать.",
                    "Сохранение плана");
            }
            else
            {
                MessageBox.Show($"План успешно сохранён! Сохранено {savedCount} дней.", "Успех");
            }

            // Обновляем отображение
            RefreshPlanDisplay();
        }
    }
}