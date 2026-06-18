using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Diagnostics;
using System.Globalization;
using Control = System.Windows.Forms.Control;
using Color = System.Drawing.Color;

namespace вход_курсач
{
    public partial class RaportShift : Form
    {
        private readonly int _currentUserId;
        private readonly DB_Quarterly _db;
        private ClassUser? _currentUser;
        private int _currentFilialId;
        private int _editingReportId = 0;
        private int _currentTeamNum = 0;
        private List<ShiftReportListItem> _currentShiftList = new();
        private string _lastSortedColumn = "";
        private bool _ascending = true;

        public RaportShift(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            _db = new DB_Quarterly(Program.ConnectionString);
            var user = _db.GetUserById(userId);
            if (user.Status == 3 || user.Status == 6)
            {
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;
                textBox7.Visible = true;
                textBox8.Visible = true;
                label4.Visible = true;
                label13.Visible = true;
                label11.Visible = true;
                label5.Visible = true;
                label3.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                cmbNextManager.Visible = true;
                listBox1.Visible = true;
                btnSave.Visible = true;
                btnChange.Visible = true;
                btnImport.Visible = true;
            }
            if (user.Status != 1)
                btnImport.Visible = true;
            Load += RaportShift_Load;
            Load += (s, e) => ApplyGazpromStyle();
            dataGridView1.ColumnHeaderMouseClick += DataGridView1_ColumnHeaderMouseClick;
        }

        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentShiftList == null || _currentShiftList.Count == 0)
                return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

            if (string.IsNullOrEmpty(columnName))
                return;

            if (_lastSortedColumn == columnName)
                _ascending = !_ascending;
            else
                _ascending = true;

            _lastSortedColumn = columnName;

            var prop = typeof(ShiftReportListItem).GetProperty(columnName);
            if (prop == null)
                return;

            _currentShiftList = _ascending
                ? _currentShiftList.OrderBy(x => prop.GetValue(x)).ToList()
                : _currentShiftList.OrderByDescending(x => prop.GetValue(x)).ToList();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _currentShiftList;
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
                else if (ctrl is NumericUpDown num)
                {
                    num.BackColor = Color.White;
                    num.ForeColor = Design.GazpromStyle.DarkGray;
                }
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

        private void RaportShift_Load(object? sender, EventArgs e)
        {
            _currentUser = _db.GetUserById(_currentUserId);
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не найден");
                Close();
                return;
            }

            _currentFilialId = _currentUser.Filial;
            _currentTeamNum = _currentUser.TeamNum;

            textBox1.Text = _db.GetNextReportNumber().ToString();
            textBox2.Text = DateTime.Today.ToString("dd.MM.yyyy");

            int hour = DateTime.Now.Hour;
            string shiftType = (hour >= 8 && hour < 20) ? "Дневная" : "Ночная";
            textBox8.Text = shiftType;

            textBox3.Text = $"{_currentUser.Surname} {_currentUser.Name[0]}." +
                           $"{(_currentUser.SecondName?.Length > 0 ? _currentUser.SecondName[0] + "." : "")}";

            var managers = _db.GetShiftManagersInFilial(_currentFilialId, _currentUserId);
            var list = new List<EmployeeItem> { new EmployeeItem { Id = 0, FullName = "— Не выбран —" } };

            var shiftManagersOnly = managers.Where(m =>
            {
                var user = _db.GetUserById(m.Id);
                return user != null && user.Status == 3;
            }).ToList();

            list.AddRange(shiftManagersOnly);
            cmbNextManager.DataSource = list;
            cmbNextManager.DisplayMember = "FullName";
            cmbNextManager.ValueMember = "Id";

            LoadTeamMembers();
            RefreshHistory();

            btnSave.Enabled = true;
            btnChange.Enabled = true;
            btnImport.Enabled = true;

            textBox6.TextChanged += (s, ev) => CalculateCompletion();
            LoadPlanToPlannedField();
        }

        private void LoadTeamMembers()
        {
            var teamMembers = _db.GetTeamMembers(_currentFilialId, _currentTeamNum, _currentUserId);
            listBox1.DataSource = teamMembers;
            listBox1.DisplayMember = "FullName";
            listBox1.ValueMember = "Id";
        }

        private void RefreshHistory()
        {
            _currentShiftList = _db.GetShiftReports(_currentFilialId);
            dataGridView1.DataSource = _currentShiftList;

            if (dataGridView1.Columns["Id"] != null)
                dataGridView1.Columns["Id"].Visible = false;

            if (dataGridView1.Columns["ReportNumber"] != null)
                dataGridView1.Columns["ReportNumber"].HeaderText = "№ отчёта";

            if (dataGridView1.Columns["ReportDate"] != null)
                dataGridView1.Columns["ReportDate"].HeaderText = "Дата";

            if (dataGridView1.Columns["ShiftType"] != null)
                dataGridView1.Columns["ShiftType"].HeaderText = "Смена";

            if (dataGridView1.Columns["ManagerName"] != null)
                dataGridView1.Columns["ManagerName"].HeaderText = "Начальник смены";

            if (dataGridView1.Columns["PlannedGas"] != null)
            {
                dataGridView1.Columns["PlannedGas"].HeaderText = "План (тыс.м³)";
                dataGridView1.Columns["PlannedGas"].DefaultCellStyle.Format = "N2";
            }

            if (dataGridView1.Columns["ActualGas"] != null)
            {
                dataGridView1.Columns["ActualGas"].HeaderText = "Факт (тыс.м³)";
                dataGridView1.Columns["ActualGas"].DefaultCellStyle.Format = "N2";
            }

            if (dataGridView1.Columns["Completion"] != null)
            {
                dataGridView1.Columns["Completion"].HeaderText = "% выполнения";
                dataGridView1.Columns["Completion"].DefaultCellStyle.Format = "N1";
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadPlanToPlannedField()
        {
            try
            {
                DateTime today = DateTime.Today;
                double dailyPlan = _db.GetDailyPlanVolume(_currentFilialId, today);

                if (dailyPlan > 0)
                {
                    double shiftPlan = dailyPlan / 2;
                    textBox4.Text = shiftPlan.ToString("F3", CultureInfo.InvariantCulture);
                }
                else
                {
                    textBox4.Text = "";
                }
            }
            catch (Exception ex)
            {
                textBox4.Text = "";
                Console.WriteLine($"Ошибка загрузки плана: {ex.Message}");
            }

            CalculateCompletion();
        }

        private void CalculateCompletion()
        {
            try
            {
                if (double.TryParse(textBox4.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double plan) && plan > 0)
                {
                    if (double.TryParse(textBox6.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double actual))
                    {
                        textBox7.Text = (actual / plan * 100).ToString("F1", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        textBox7.Text = "0.0";
                    }
                }
                else
                {
                    textBox7.Text = "";
                }
            }
            catch
            {
                textBox7.Text = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Поле 'Запланировано' не заполнено!\nВведите значение вручную.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!double.TryParse(textBox4.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double plan) || plan <= 0)
                {
                    MessageBox.Show($"Введите корректное плановое значение (>0)\nТекущее значение: '{textBox4.Text}'",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!double.TryParse(textBox6.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double actual))
                {
                    MessageBox.Show($"Введите корректное фактическое значение\nТекущее значение: '{textBox6.Text}'",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int? nextManagerId = null;
                if (cmbNextManager.SelectedItem is EmployeeItem selected && selected.Id > 0)
                {
                    var selectedUser = _db.GetUserById(selected.Id);
                    if (selectedUser != null && selectedUser.Status == 3)
                    {
                        nextManagerId = selected.Id;
                    }
                    else
                    {
                        MessageBox.Show("Принимать смену может только начальник смены!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                var report = new ShiftReportData
                {
                    Id = _editingReportId,
                    ReportNumber = int.Parse(textBox1.Text),
                    ReportDate = DateTime.Today.ToString("yyyy-MM-dd"),
                    FilialId = _currentFilialId,
                    ShiftType = textBox8.Text,
                    ShiftManagerId = _currentUserId,
                    NextManagerId = nextManagerId,
                    PlannedGas = plan,
                    ActualGas = actual,
                    Completion = plan > 0 ? actual / plan * 100 : 0,
                    TransferNote = textBox5.Text
                };

                _db.SaveShiftReport(report);

                NotificationService.NewShiftReport(
                    reportDate: report.ReportDate,
                    filialName: _db.GetFilialNameById(_currentUser.Filial),
                    shiftType: report.ShiftType,
                    managerName: $"{_currentUser.Surname} {_currentUser.Name}"
                );

                NotificationService.ReportCreated("Сменный",
                    $"Филиал: {_db.GetFilialNameById(_currentUser.Filial)}\n" +
                    $"Смена: {report.ShiftType}\n" +
                    $"Начальник: {_currentUser.Surname} {_currentUser.Name}\n" +
                    $"№ отчёта: {report.ReportNumber}");

                string message = _editingReportId == 0 ? "Отчёт добавлен!" : "Отчёт изменён!";
                MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
                RefreshHistory();
                LoadPlanToPlannedField();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите отчёт в таблице", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = (ShiftReportListItem)dataGridView1.CurrentRow.DataBoundItem;
            var report = _db.GetShiftReportById(selected.Id);

            if (report == null)
            {
                MessageBox.Show("Отчёт не найден", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _editingReportId = report.Id;
            textBox1.Text = report.ReportNumber.ToString();
            textBox2.Text = DateTime.Parse(report.ReportDate).ToString("dd.MM.yyyy");
            textBox8.Text = report.ShiftType == "Дневная" ? "Дневная" : "Ночная";
            textBox4.Text = report.PlannedGas.ToString("F3", CultureInfo.InvariantCulture);
            textBox6.Text = report.ActualGas.ToString("F3", CultureInfo.InvariantCulture);
            textBox7.Text = report.Completion.ToString("F1", CultureInfo.InvariantCulture);
            textBox5.Text = report.TransferNote;

            if (report.NextManagerId.HasValue)
            {
                for (int i = 0; i < cmbNextManager.Items.Count; i++)
                {
                    var item = (EmployeeItem)cmbNextManager.Items[i];
                    if (item.Id == report.NextManagerId.Value)
                    {
                        cmbNextManager.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cmbNextManager.SelectedIndex = 0;
            }

            btnSave.Text = "Изменить";
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите отчёт в таблице для экспорта в Word", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = (ShiftReportListItem)dataGridView1.CurrentRow.DataBoundItem;
            var report = _db.GetShiftReportById(selected.Id);

            if (report == null)
            {
                MessageBox.Show("Отчёт не найден", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ExportToWord(report);
        }

        private void ExportToWord(ShiftReportData report)
        {
            try
            {
                var shiftManager = _db.GetUserById(report.ShiftManagerId);
                string managerName = shiftManager != null
                    ? $"{shiftManager.Surname} {shiftManager.Name[0]}.{(!string.IsNullOrEmpty(shiftManager.SecondName) ? shiftManager.SecondName[0] + "." : "")}"
                    : "Неизвестно";

                string nextManagerName = "—";
                if (report.NextManagerId.HasValue)
                {
                    var nextManager = _db.GetUserById(report.NextManagerId.Value);
                    if (nextManager != null)
                    {
                        nextManagerName = $"{nextManager.Surname} {nextManager.Name[0]}.{(!string.IsNullOrEmpty(nextManager.SecondName) ? nextManager.SecondName[0] + "." : "")}";
                    }
                }

                string dateFormatted = DateTime.TryParse(report.ReportDate, out var dt)
                    ? dt.ToString("dd.MM.yyyy")
                    : report.ReportDate;

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"Сменный_отчёт_{report.ReportNumber}.docx";
                string filePath = Path.Combine(desktopPath, fileName);

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    Paragraph titlePara = new Paragraph(
                        new Run(new Text($"СМЕННЫЙ ОТЧЁТ №{report.ReportNumber}"))
                        {
                            RunProperties = new RunProperties(
                                new Bold(),
                                new FontSize { Val = "36" })
                        })
                    {
                        ParagraphProperties = new ParagraphProperties(
                            new Justification() { Val = JustificationValues.Center },
                            new SpacingBetweenLines() { After = "240" })
                    };
                    body.AppendChild(titlePara);

                    body.AppendChild(new Paragraph(new Run(new Text(""))));

                    AddWordParagraph(body, "Дата", dateFormatted);
                    AddWordParagraph(body, "Начальник смены", managerName);
                    AddWordParagraph(body, "Принял смену", nextManagerName);

                    body.AppendChild(new Paragraph(new Run(new Text(""))));

                    Paragraph indicatorsTitle = new Paragraph(
                        new Run(new Text("ПРОИЗВОДСТВЕННЫЕ ПОКАЗАТЕЛИ"))
                        {
                            RunProperties = new RunProperties(
                                new Bold(),
                                new FontSize { Val = "28" })
                        })
                    {
                        ParagraphProperties = new ParagraphProperties(
                            new Justification() { Val = JustificationValues.Center },
                            new SpacingBetweenLines() { After = "200" })
                    };
                    body.AppendChild(indicatorsTitle);

                    AddWordParagraph(body, "Запланировано газа к перекачке", $"{report.PlannedGas.ToString("F3", CultureInfo.InvariantCulture)} тыс. м³");
                    AddWordParagraph(body, "Реально перекачано", $"{report.ActualGas.ToString("F3", CultureInfo.InvariantCulture)} тыс. м³");
                    AddWordParagraph(body, "Процент выполнения", $"{report.Completion.ToString("F1", CultureInfo.InvariantCulture)} %");

                    body.AppendChild(new Paragraph(new Run(new Text(""))));

                    Paragraph transferTitle = new Paragraph(
                        new Run(new Text("ПЕРЕДАЧА СМЕНЫ"))
                        {
                            RunProperties = new RunProperties(
                                new Bold(),
                                new FontSize { Val = "28" })
                        })
                    {
                        ParagraphProperties = new ParagraphProperties(
                            new Justification() { Val = JustificationValues.Center },
                            new SpacingBetweenLines() { After = "200" })
                    };
                    body.AppendChild(transferTitle);

                    AddWordParagraph(body, "Замечания и передача", string.IsNullOrEmpty(report.TransferNote) ? "—" : report.TransferNote);

                    body.AppendChild(new Paragraph(new Run(new Text(""))));
                    body.AppendChild(new Paragraph(new Run(new Text(""))));

                    Paragraph signature1 = new Paragraph(
                        new Run(new Text("Начальник смены: _________________ /"))
                        {
                            RunProperties = new RunProperties(new Bold())
                        },
                        new Run(new Text(managerName)),
                        new Run(new Text("/"))
                        {
                            RunProperties = new RunProperties(new Bold())
                        });
                    body.AppendChild(signature1);

                    Paragraph signature2 = new Paragraph(
                        new Run(new Text("Смену принял: _________________ /"))
                        {
                            RunProperties = new RunProperties(new Bold())
                        },
                        new Run(new Text(nextManagerName)),
                        new Run(new Text("/"))
                        {
                            RunProperties = new RunProperties(new Bold())
                        });
                    body.AppendChild(signature2);

                    Paragraph datePara = new Paragraph(
                        new Run(new Text($"\nДата составления: {DateTime.Today:dd.MM.yyyy}"))
                        {
                            RunProperties = new RunProperties(new Italic())
                        });
                    body.AppendChild(datePara);
                }

                MessageBox.Show($"Документ сохранён на рабочем столе:\n{fileName}", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании документа:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddWordParagraph(Body body, string label, string value)
        {
            Paragraph para = new Paragraph(
                new Run(new Text(label + ": "))
                {
                    RunProperties = new RunProperties(new Bold())
                },
                new Run(new Text(value)));

            para.ParagraphProperties = new ParagraphProperties(
                new SpacingBetweenLines() { After = "100" });

            body.AppendChild(para);
        }

        private void ClearForm()
        {
            _editingReportId = 0;
            textBox1.Text = _db.GetNextReportNumber().ToString();
            textBox2.Text = DateTime.Today.ToString("dd.MM.yyyy");
            int hour = DateTime.Now.Hour;
            string shiftType = (hour >= 8 && hour < 20) ? "Дневная" : "Ночная";
            textBox8.Text = shiftType;
            textBox4.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox5.Clear();
            cmbNextManager.SelectedIndex = 0;
            btnSave.Text = "Сохранить";
        }
    }
}