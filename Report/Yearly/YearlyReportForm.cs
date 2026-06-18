using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing.Drawing2D;
using Control = System.Windows.Forms.Control;

namespace вход_курсач
{
    public partial class YearlyReportForm : Form
    {
        private readonly int _userId;
        private readonly DB_Quarterly _db;
        private List<QuarterlyReportListItem> _reports;
        private string _lastSortedColumn = "";
        private bool _ascending = true;

        public YearlyReportForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _db = new DB_Quarterly(Program.ConnectionString);
            var user = _db.GetUserById(userId);
            if (user.Status == 3 && user.Status == 6)
            {
                label1.Visible = false;
                comboBox1.Visible = false;
                btnCreate.Visible = false;
            }
            SetupGrid();
            LoadYears();
            LoadReports();

            this.Load += (s, e) => ApplyGazpromStyle();
            dataGridView1.ColumnHeaderMouseClick += DataGridView1_ColumnHeaderMouseClick;
        }

        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_reports == null || _reports.Count == 0)
                return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

            if (string.IsNullOrEmpty(columnName))
                return;

            if (_lastSortedColumn == columnName)
                _ascending = !_ascending;
            else
                _ascending = true;

            _lastSortedColumn = columnName;

            var prop = typeof(QuarterlyReportListItem).GetProperty(columnName);
            if (prop == null)
                return;

            _reports = _ascending
                ? _reports.OrderBy(x => prop.GetValue(x)).ToList()
                : _reports.OrderByDescending(x => prop.GetValue(x)).ToList();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _reports;
        }

        private void ApplyGazpromStyle()
        {
            Design.GazpromStyle.StyleForm(this);
            foreach (Control ctrl in GetAllControls(this))
            {
                if (ctrl is Button btn) Design.GazpromStyle.StyleButton(btn);
                else if (ctrl is TextBox txt) Design.GazpromStyle.StyleTextBox(txt);
                else if (ctrl is Label lbl) Design.GazpromStyle.StyleLabel(lbl);
                else if (ctrl is ComboBox cmb) Design.GazpromStyle.StyleComboBox(cmb);
                else if (ctrl is DataGridView dgv) Design.GazpromStyle.StyleDataGridView(dgv);
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

        private void LoadYears()
        {
            comboBox1.Items.Clear();
            int currentYear = DateTime.Today.Year;
            for (int y = currentYear - 4; y <= currentYear; y++)
                comboBox1.Items.Add(y.ToString());

            comboBox1.SelectedItem = currentYear.ToString();
        }

        private void SetupGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Year",
                HeaderText = "Год",
                DataPropertyName = "Year"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FilialName",
                HeaderText = "Филиал",
                DataPropertyName = "FilialName"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CompiledBy",
                HeaderText = "Составил",
                DataPropertyName = "CompiledBy"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AddressedTo",
                HeaderText = "На имя",
                DataPropertyName = "AddressedTo"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Дата создания",
                DataPropertyName = "CreatedAt"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PlannedGas",
                HeaderText = "План (м³)",
                DataPropertyName = "PlannedGas",
                DefaultCellStyle = { Format = "N2" }
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActualGas",
                HeaderText = "Факт (м³)",
                DataPropertyName = "ActualGas",
                DefaultCellStyle = { Format = "N2" }
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Completion",
                HeaderText = "Выполнение (%)",
                DataPropertyName = "Completion",
                DefaultCellStyle = { Format = "N2" }
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Profit",
                HeaderText = "Прибыль",
                DataPropertyName = "Profit",
                DefaultCellStyle = { Format = "N2" }
            });

            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadReports()
        {
            try
            {
                _reports = _db.GetYearlyReportsByUser(_userId);

                if (_reports == null || _reports.Count == 0)
                {
                    dataGridView1.DataSource = null;
                    return;
                }

                dataGridView1.DataSource = _reports;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(comboBox1.Text, out int year))
            {
                MessageBox.Show("Введите корректный год.");
                return;
            }

            var result = _db.CreateYearlyReport(_userId, year);
            if (result.Success)
            {
                // ✅ НОВОЕ уведомление о создании годового отчёта
                string filialName = _db.GetFilialNameById(_db.GetFilialByUserId(_userId));
                NotificationService.ReportCreated("Годовой",
                    $"{year} г.\n" +
                    $"Филиал: {filialName}");

                MessageBox.Show($"Годовой отчёт за {year} создан.");
                LoadReports();
            }
            else
                MessageBox.Show(result.ErrorMessage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите отчёт.");
                return;
            }

            var report = _reports[dataGridView1.SelectedRows[0].Index];

            using var dlg = new SaveFileDialog
            {
                Filter = "Word документ|*.docx",
                FileName = $"Годовой_отчёт_{report.Year}.docx"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            string chartPath = GenerateYearlyChartImage(_reports, report.Year);

            ExportToWord(report, dlg.FileName, chartPath);
            MessageBox.Show("Файл сохранён.");
        }

        // ====================== ЭКСПОРТ ======================
        private void ExportToWord(QuarterlyReportListItem r, string path, string chartImagePath)
        {
            using var doc = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document);
            var main = doc.AddMainDocumentPart();
            main.Document = new Document(new Body());
            var body = main.Document.Body!;

            void AddLine(string text, bool bold = false, int size = 24)
            {
                var run = new Run();
                var props = new RunProperties();
                if (bold) props.Append(new Bold());
                props.Append(new FontSize { Val = size.ToString() });
                run.Append(props);
                run.Append(new Text(text) { Space = SpaceProcessingModeValues.Preserve });
                body.AppendChild(new Paragraph(run));
            }

            AddLine("ИТОГОВЫЙ ГОДОВОЙ ОТЧЁТ ПО ТРАНСПОРТИРОВКЕ ГАЗА", true, 28);
            AddLine("");
            AddLine($"Год: {r.Year}", true);
            AddLine($"Филиал: {r.FilialName}");
            AddLine($"Дата создания: {r.CreatedAt}");
            AddLine($"Составил: {r.CompiledBy}");
            AddLine("");

            AddLine("ЭКОНОМИЧЕСКИЕ ПОКАЗАТЕЛИ", true, 26);
            AddLine($"Общая выручка:   {r.Revenue:N2} руб.");
            AddLine($"Общие затраты:   {r.Cost:N2} руб.");
            AddLine($"Чистая прибыль:  {r.Profit:N2} руб.", true);
            AddLine("");

            AddLine("ТЕХНИЧЕСКИЕ ПОКАЗАТЕЛИ", true, 26);
            AddLine($"Запланировано:   {r.PlannedGas:N2} м³");
            AddLine($"Фактически:      {r.ActualGas:N2} м³");
            AddLine($"Выполнение плана: {r.Completion:N2}%");
            AddLine("");

            if (!string.IsNullOrEmpty(chartImagePath) && File.Exists(chartImagePath))
            {
                AddLine("ДИАГРАММА ПРИБЫЛИ ПО ГОДАМ", true, 26);
                AddLine("");
                InsertImageIntoWord(main, body, chartImagePath);
                AddLine("");
            }

            AddLine($"Подпись составителя: _______________________ / {r.CompiledBy} /");
        }

        // ====================== ГРАФИЧЕСКАЯ ДИАГРАММА ======================
        private string GenerateYearlyChartImage(List<QuarterlyReportListItem> reports, int currentYear)
        {
            var data = reports.Where(x => x.Year <= currentYear).OrderBy(x => x.Year).ToList();
            if (data.Count == 0) return null;

            int width = 720, height = 420;
            using var bmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(bmp);

            g.Clear(System.Drawing.Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float maxProfit = (float)data.Max(x => Math.Abs(x.Profit)) * 1.25f;
            int padding = 80;
            float chartH = height - padding * 2;
            int barWidth = 65;
            int gap = 45;

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                float barH = (float)(Math.Abs(item.Profit) / maxProfit * chartH);
                float x = padding + i * (barWidth + gap);
                float y = height - padding - barH;

                var brush = item.Profit >= 0 ? Brushes.SteelBlue : Brushes.Crimson;
                g.FillRectangle(brush, x, y, barWidth, barH);
                g.DrawRectangle(Pens.Black, x, y, barWidth, barH);

                g.DrawString(item.Profit.ToString("N0"),
                    new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold),
                    Brushes.Black, x + 5, y - 25);

                g.DrawString(item.Year.ToString(),
                    new System.Drawing.Font("Segoe UI", 10),
                    Brushes.Black, x + 15, height - padding + 20);
            }

            string filePath = Path.Combine(Path.GetTempPath(), $"year_chart_{DateTime.Now.Ticks}.png");
            bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            return filePath;
        }

        // ====================== ВСТАВКА ИЗОБРАЖЕНИЯ ======================
        private static void InsertImageIntoWord(MainDocumentPart mainPart, Body body, string imagePath)
        {
            var imagePart = mainPart.AddImagePart(ImagePartType.Png);
            using (var stream = new FileStream(imagePath, FileMode.Open))
                imagePart.FeedData(stream);

            string relId = mainPart.GetIdOfPart(imagePart);

            var drawing = new Drawing(
                new DocumentFormat.OpenXml.Drawing.Wordprocessing.Inline(
                    new DocumentFormat.OpenXml.Drawing.Wordprocessing.Extent { Cx = 6500000, Cy = 3200000 },
                    new DocumentFormat.OpenXml.Drawing.Wordprocessing.DocProperties { Id = 1, Name = "ProfitChart" },
                    new DocumentFormat.OpenXml.Drawing.Wordprocessing.NonVisualGraphicFrameDrawingProperties(),
                    new DocumentFormat.OpenXml.Drawing.Graphic(
                        new DocumentFormat.OpenXml.Drawing.GraphicData(
                            new DocumentFormat.OpenXml.Drawing.Pictures.Picture(
                                new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureProperties(
                                    new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualDrawingProperties { Id = 0, Name = "Chart.png" },
                                    new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureDrawingProperties()),
                                new DocumentFormat.OpenXml.Drawing.Pictures.BlipFill(
                                    new DocumentFormat.OpenXml.Drawing.Blip { Embed = relId },
                                    new DocumentFormat.OpenXml.Drawing.Stretch(new DocumentFormat.OpenXml.Drawing.FillRectangle())),
                                new DocumentFormat.OpenXml.Drawing.Pictures.ShapeProperties(
                                    new DocumentFormat.OpenXml.Drawing.Transform2D(
                                        new DocumentFormat.OpenXml.Drawing.Offset { X = 0, Y = 0 },
                                        new DocumentFormat.OpenXml.Drawing.Extents { Cx = 6500000, Cy = 3200000 }),
                                    new DocumentFormat.OpenXml.Drawing.PresetGeometry(new DocumentFormat.OpenXml.Drawing.AdjustValueList())
                                    { Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle })))
                        { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })));

            body.AppendChild(new Paragraph(new Run(drawing)));
        }
    }
}