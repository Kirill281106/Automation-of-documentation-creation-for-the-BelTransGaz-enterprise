using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Dapper;
using Control = System.Windows.Forms.Control;
using Color = System.Drawing.Color;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Font = System.Drawing.Font;
using Path = System.IO.Path;

namespace вход_курсач
{
    public partial class CreateQuarterlyReportForm : Form
    {
        private readonly int _userId;
        private readonly DB_Quarterly _db;
        private List<QuarterlyReportListItem> _reports;
        private string _lastSortedColumn = "";
        private bool _ascending = true;


        public CreateQuarterlyReportForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _db = new DB_Quarterly(Program.ConnectionString);
            var user = _db.GetUserById(userId);
            if (user.Status != 4 && user.Status != 6)
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                l1.Visible = false;
                cmbQuarter.Visible = false;
                cmbYear.Visible = false;
                txtPurchasePrice.Visible = false;
                txtSalePrice.Visible = false;
                btnCreate.Visible = false;
            }
            SetupDataGridView();
            LoadAvailableQuarters();
            LoadReports();

            this.Load += (s, e) => ApplyGazpromStyle();
            dataGridView1.ColumnHeaderMouseClick += DataGridView1_ColumnHeaderMouseClick;
        }

        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_reports == null || _reports.Count == 0)
                return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

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

            RedrawGrid();
        }

        private void RedrawGrid()
        {
            dataGridView1.Rows.Clear();

            foreach (var report in _reports)
            {
                string profitText = (report.PurchasePrice > 0)
                    ? report.Profit.ToString("N2")
                    : "❌ нет цен";

                int idx = dataGridView1.Rows.Add(
                    report.Quarter,
                    report.Year,
                    report.FilialName,
                    report.CompiledBy,
                    report.AddressedTo,
                    report.CreatedAt,
                    report.PlannedGas.ToString("N2"),
                    report.ActualGas.ToString("N2"),
                    report.Completion.ToString("N2") + "%",
                    profitText
                );

                if (report.PurchasePrice > 0)
                {
                    if (report.Profit < 0)
                        dataGridView1.Rows[idx].Cells["Profit"].Style.ForeColor = Color.Red;
                    else if (report.Profit > 0)
                        dataGridView1.Rows[idx].Cells["Profit"].Style.ForeColor = Color.Green;
                }
            }
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

        private void SetupDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Quarter", "Квартал");
            dataGridView1.Columns.Add("Year", "Год");
            dataGridView1.Columns.Add("FilialName", "Филиал");
            dataGridView1.Columns.Add("CompiledBy", "Составил");
            dataGridView1.Columns.Add("AddressedTo", "На имя");
            dataGridView1.Columns.Add("CreatedAt", "Дата создания");
            dataGridView1.Columns.Add("PlannedGas", "Планируемый газ (м³)");
            dataGridView1.Columns.Add("ActualGas", "Прокачано (м³)");
            dataGridView1.Columns.Add("Completion", "Выполнение (%)");
            dataGridView1.Columns.Add("Profit", "Прибыль (руб.)");
        }

        private void LoadAvailableQuarters()
        {
            cmbQuarter.Items.Clear();
            cmbYear.Items.Clear();

            for (int q = 1; q <= 4; q++)
                cmbQuarter.Items.Add(q.ToString());

            int currentYear = DateTime.Today.Year;

            for (int y = currentYear - 2; y <= currentYear; y++)
                cmbYear.Items.Add(y.ToString());

            cmbQuarter.SelectedIndex = 0;
            cmbYear.SelectedItem = currentYear.ToString();
        }

        private void LoadReports()
        {
            try
            {
                _reports = _db.GetQuarterlyReports(_userId);
                dataGridView1.Rows.Clear();

                foreach (var report in _reports)
                {
                    string profitText = (report.PurchasePrice > 0) ? report.Profit.ToString("N2") : "❌ нет цен";

                    dataGridView1.Rows.Add(
                        report.Quarter,
                        report.Year,
                        report.FilialName,
                        report.CompiledBy,
                        report.AddressedTo,
                        report.CreatedAt,
                        report.PlannedGas.ToString("N2"),
                        report.ActualGas.ToString("N2"),
                        report.Completion.ToString("N2") + "%",
                        profitText
                    );

                    var lastRow = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
                    if (report.PurchasePrice > 0)
                    {
                        if (report.Profit < 0) lastRow.Cells["Profit"].Style.ForeColor = Color.Red;
                        else if (report.Profit > 0) lastRow.Cells["Profit"].Style.ForeColor = Color.Green;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnCreate_Click_1(object sender, EventArgs e)
        {
            if (_db == null)
            {
                MessageBox.Show("Ошибка: Объект базы данных не инициализирован (null).", "Ошибка");
                return;
            }

            if (txtPurchasePrice == null || txtSalePrice == null)
            {
                MessageBox.Show("Ошибка: Текстовые поля txtPurchasePrice или txtSalePrice не найдены на форме!", "Ошибка");
                return;
            }

            if (cmbQuarter.SelectedItem == null || cmbYear.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите квартал и год!", "Внимание");
                return;
            }

            if (!double.TryParse(txtPurchasePrice.Text, out double purchasePrice) ||
                !double.TryParse(txtSalePrice.Text, out double salePrice))
            {
                MessageBox.Show("Введите корректные числовые значения цен!", "Ошибка ввода");
                return;
            }

            try
            {
                int quarter = int.Parse(cmbQuarter.SelectedItem.ToString());
                int year = int.Parse(cmbYear.SelectedItem.ToString());

                var result = _db.CreateQuarterlyReport(_userId, quarter, year, purchasePrice, salePrice);

                if (result != null && result.Success)
                {
                    string filialName = _db.GetFilialNameById(_db.GetFilialByUserId(_userId));
                    NotificationService.ReportCreated("Квартальный",
                        $"{quarter} кв. {year} г.\n" +
                        $"Филиал: {filialName}\n" +
                        $"Цена закупки: {purchasePrice:N2} руб.\n" +
                        $"Цена продажи: {salePrice:N2} руб.");

                    MessageBox.Show($"Квартальный отчёт за {quarter} кв. {year} г. успешно создан!", "Успех");
                    LoadReports();
                }
                else
                {
                    string error = result?.ErrorMessage ?? "Неизвестная ошибка БД";
                    MessageBox.Show(error, "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void ExportBTN_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите отчёт для экспорта.");
                return;
            }

            int index = dataGridView1.SelectedRows[0].Index;
            var reportFromList = _reports[index];

            var report = _db.GetQuarterlyReportById(reportFromList.Id);

            if (report == null) return;

            double pPrice = report.PurchasePrice;
            double sPrice = report.SalePrice;

            if (pPrice <= 0 || sPrice <= 0)
            {
                MessageBox.Show("В данном отчете не сохранены цены. Экспорт невозможен.");
                return;
            }

            var service = new QuarterlyForecastService(Program.ConnectionString);
            int filialId = service.GetFilial(_userId);
            var forecast = service.Calculate(filialId);
            var history = service.GetHistory(filialId);

            string chartPath = null;
            if (history != null && history.Count >= 2 && forecast != null)
                chartPath = GenerateChartImage(history, forecast);

            using var dlg = new SaveFileDialog
            {
                Filter = "Word документ|*.docx",
                FileName = $"Квартальный_отчёт_{report.Quarter}кв{report.Year}.docx"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            ExportToWord(reportFromList, dlg.FileName, forecast, history, chartPath, pPrice, sPrice);

            MessageBox.Show("Файл сохранён.", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateReportsDisplay()
        {
            if (_reports != null)
                LoadReports();
        }

        private void txtPurchasePrice_TextChanged(object sender, EventArgs e)
        {
            UpdateReportsDisplay();
        }

        private void txtSalePrice_TextChanged(object sender, EventArgs e)
        {
            UpdateReportsDisplay();
        }

        public static void ExportToWord(
            QuarterlyReportListItem r,
            string path,
            QuartelyResult forecast,
            List<QuartelyPoint> history,
            string chartImagePath,
            double purchasePrice,
            double salePrice)
        {
            using var doc = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document);
            var main = doc.AddMainDocumentPart();
            main.Document = new Document(new Body());
            var body = main.Document.Body!;

            void AddLine(string text, bool bold = false, int size = 24)
            {
                var run = new Run();
                var props = new RunProperties();
                if (bold) props.AppendChild(new Bold());
                props.AppendChild(new FontSize { Val = size.ToString() });
                run.AppendChild(props);
                run.AppendChild(new Text(text) { Space = SpaceProcessingModeValues.Preserve });
                body.AppendChild(new Paragraph(run));
            }

            AddLine("КВАРТАЛЬНЫЙ ОТЧЁТ", true, 28);
            AddLine("");
            AddLine($"Квартал: {r.Quarter} кв. {r.Year} г.", true);
            AddLine($"Филиал: {r.FilialName}");
            AddLine($"Дата составления: {r.CreatedAt}");
            AddLine($"Составил: {r.CompiledBy}");
            AddLine($"На имя: {r.AddressedTo}");
            AddLine("");

            AddLine($"Запланировано (м³): {r.PlannedGas:N2}");
            AddLine($"Прокачано (м³):     {r.ActualGas:N2}");
            AddLine($"Выполнение:         {r.Completion:N2}%");
            AddLine("");

            double revenue = (r.ActualGas / 1000.0) * salePrice;
            double cost = (r.ActualGas / 1000.0) * purchasePrice;
            double profit = revenue - cost;

            AddLine("ФИНАНСОВЫЕ ПОКАЗАТЕЛИ", true, 26);
            AddLine("");
            AddLine($"Закупочная цена: {purchasePrice:N2} руб. за 1000 м³");
            AddLine($"Продажная цена:  {salePrice:N2} руб. за 1000 м³");
            AddLine("");
            AddLine($"Выручка:        {revenue:N2} руб.");
            AddLine($"Себестоимость:  {cost:N2} руб.");
            AddLine($"Прибыль:        {profit:N2} руб.", true);
            AddLine("");

            if (history != null && history.Count > 0)
            {
                AddLine("СРАВНЕНИЕ С ПРЕДЫДУЩИМИ КВАРТАЛАМИ", true, 26);
                AddLine("");

                var last3 = history.Skip(Math.Max(0, history.Count - 3)).ToList();

                foreach (var p in last3)
                {
                    AddLine($"{p.Quarter} кв. {p.Year}: " +
                        $"Факт {p.ActualGas:N2} м³ | " +
                        $"План {p.PlannedGas:N2} м³ | " +
                        $"Выполнение {p.Completion:N2}%");
                }
                AddLine("");
            }

            if (chartImagePath != null && File.Exists(chartImagePath))
            {
                AddLine("ДИАГРАММА ПРОКАЧКИ ГАЗА", true, 26);
                AddLine("");
                InsertImageIntoWord(main, body, chartImagePath);
                AddLine("");
            }

            if (forecast != null)
            {
                AddLine("АНАЛИТИКА И ПРОГНОЗ", true, 26);
                AddLine("");
                AddLine(forecast.ForecastText);
                AddLine(forecast.TrendText);

                double pct = forecast.Percentages[3];
                AddLine($"Ожидаемое изменение: {(pct >= 0 ? "+" : "")}{pct:F2}%");

                AddLine("");
                string conclusion = forecast.IsGrowth
                    ? "Наблюдается положительная динамика роста объёмов прокачки."
                    : "Наблюдается снижение объёмов прокачки газа.";
                AddLine("Заключение: " + conclusion, true);
                AddLine("");
            }

            AddLine($"Подпись: _______________________  {r.CompiledBy}");
        }

        private string GenerateChartImage(List<QuartelyPoint> history, QuartelyResult forecast)
        {
            int width = 600;
            int height = 350;
            int padding = 60;
            int barWidth = 60;
            int gap = 40;

            using var bmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(bmp);

            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var last3 = history.Skip(Math.Max(0, history.Count - 3)).ToList();

            if (last3.Count == 0) return null;

            float maxVal = (float)Math.Max(last3.Max(p => p.ActualGas), forecast?.ForecastGas ?? 0) * 1.3f;
            float chartH = height - padding * 2;

            g.DrawLine(Pens.Black, padding, padding, padding, height - padding);
            g.DrawLine(Pens.Black, padding, height - padding, width - padding, height - padding);

            int count = last3.Count + (forecast != null ? 1 : 0);

            for (int i = 0; i < count; i++)
            {
                double val = i < last3.Count ? last3[i].ActualGas : forecast.ForecastGas;
                string label = i < last3.Count ? $"{last3[i].Quarter}кв {last3[i].Year}" : "Прогноз";
                var brush = i < last3.Count ? System.Drawing.Brushes.SteelBlue : System.Drawing.Brushes.DarkOrange;

                float barH = (float)(val / maxVal * chartH);
                float x = padding + i * (barWidth + gap);
                float y = (height - padding) - barH;

                g.FillRectangle(System.Drawing.Brushes.LightGray, x + 4, y + 4, barWidth, barH);
                g.FillRectangle(brush, x, y, barWidth, barH);
                g.DrawRectangle(Pens.Black, x, y, barWidth, barH);

                g.DrawString(val.ToString("N0"), new Font("Segoe UI", 8), System.Drawing.Brushes.Black, x, y - 20);
                g.DrawString(label, new Font("Segoe UI", 8), System.Drawing.Brushes.Black, x - 5, height - padding + 8);

                if (i > 0 && forecast?.Percentages != null && i < forecast.Percentages.Length)
                {
                    double pct = forecast.Percentages[i];
                    string pctStr = (pct >= 0 ? "+" : "") + pct.ToString("F1") + "%";
                    var pctBrush = pct >= 0 ? System.Drawing.Brushes.Green : System.Drawing.Brushes.Red;
                    g.DrawString(pctStr, new Font("Segoe UI", 8, FontStyle.Bold), pctBrush, x + 5, y - 38);
                }
            }

            g.FillRectangle(System.Drawing.Brushes.SteelBlue, padding, 10, 20, 12);
            g.DrawString("История", new Font("Segoe UI", 8), System.Drawing.Brushes.Black, padding + 25, 10);

            g.FillRectangle(System.Drawing.Brushes.DarkOrange, padding + 100, 10, 20, 12);
            g.DrawString("Прогноз", new Font("Segoe UI", 8), System.Drawing.Brushes.Black, padding + 125, 10);

            string tmpPath = Path.Combine(Path.GetTempPath(), "quarterly_chart.png");
            bmp.Save(tmpPath, ImageFormat.Png);
            return tmpPath;
        }

        private static void InsertImageIntoWord(MainDocumentPart mainPart, Body body, string imagePath)
        {
            var imagePart = mainPart.AddImagePart(ImagePartType.Png);
            using (var stream = new FileStream(imagePath, FileMode.Open))
                imagePart.FeedData(stream);

            string relationshipId = mainPart.GetIdOfPart(imagePart);

            long cx = 5500000L;
            long cy = 3200000L;

            var drawing = new Drawing(
                new DocumentFormat.OpenXml.Drawing.Wordprocessing.Inline(
                    new DocumentFormat.OpenXml.Drawing.Wordprocessing.Extent { Cx = cx, Cy = cy },
                    new DocumentFormat.OpenXml.Drawing.Wordprocessing.DocProperties { Id = 1, Name = "Chart" },
                    new DocumentFormat.OpenXml.Drawing.Wordprocessing.NonVisualGraphicFrameDrawingProperties(),
                    new DocumentFormat.OpenXml.Drawing.Graphic(
                        new DocumentFormat.OpenXml.Drawing.GraphicData(
                            new DocumentFormat.OpenXml.Drawing.Pictures.Picture(
                                new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureProperties(
                                    new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualDrawingProperties { Id = 0, Name = "Chart.png" },
                                    new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureDrawingProperties()),
                                new DocumentFormat.OpenXml.Drawing.Pictures.BlipFill(
                                    new DocumentFormat.OpenXml.Drawing.Blip { Embed = relationshipId },
                                    new DocumentFormat.OpenXml.Drawing.Stretch(new DocumentFormat.OpenXml.Drawing.FillRectangle())),
                                new DocumentFormat.OpenXml.Drawing.Pictures.ShapeProperties(
                                    new DocumentFormat.OpenXml.Drawing.Transform2D(
                                        new DocumentFormat.OpenXml.Drawing.Offset { X = 0, Y = 0 },
                                        new DocumentFormat.OpenXml.Drawing.Extents { Cx = cx, Cy = cy }),
                                    new DocumentFormat.OpenXml.Drawing.PresetGeometry(new DocumentFormat.OpenXml.Drawing.AdjustValueList())
                                    { Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle })))
                        { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }))
                {
                    DistanceFromTop = 0,
                    DistanceFromBottom = 0,
                    DistanceFromLeft = 0,
                    DistanceFromRight = 0
                });

            body.AppendChild(new Paragraph(new Run(drawing)));
        }

        private void Open_Analis_Click(object sender, EventArgs e)
        {
            QuarterlyForecastForm form = new QuarterlyForecastForm(_userId);
            form.ShowDialog(this);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите отчёт для просмотра.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int selectedIndex = dataGridView1.SelectedRows[0].Index;
            if (selectedIndex >= 0 && selectedIndex < _reports.Count)
            {
                var selectedReport = _reports[selectedIndex];

                using (var viewForm = new QuarterlyReportViewForm(
                    selectedReport.Id,
                    Program.ConnectionString))
                {
                    viewForm.ShowDialog(this);
                }
            }
        }
    }
}