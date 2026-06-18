using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Control = System.Windows.Forms.Control;

namespace вход_курсач
{
    public partial class CompanyYearlyReportForm : Form
    {
        private readonly DB_Quarterly _db;
        private List<CompanyYearItem> _data;
        private int _selectedYear;

        public CompanyYearlyReportForm()    
        {
            InitializeComponent();
            _db = new DB_Quarterly(Program.ConnectionString);

            LoadYears();

            this.Load += (s, e) => ApplyGazpromStyle();
        }

        private void ApplyGazpromStyle()
        {
            Design.GazpromStyle.StyleForm(this);
            foreach (Control ctrl in GetAllControls(this))
            {
                if (ctrl is Button btn)
                    Design.GazpromStyle.StyleButton(btn);
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

        private void LoadYears()
        {
            cmbYear.Items.Clear();
            int currentYear = DateTime.Today.Year;

            for (int y = currentYear - 1; y <= currentYear; y++)
                cmbYear.Items.Add(y.ToString());

            cmbYear.SelectedItem = currentYear.ToString();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            _selectedYear = int.Parse(cmbYear.SelectedItem.ToString());
            GenerateReport();
        }

        private void GenerateReport()
        {
            _data = _db.GetCompanyYearlyData(_selectedYear);

            if (_data == null || _data.Count == 0)
            {
                MessageBox.Show("Нет данных за выбранный год.");
                return;
            }

            double totalPlanned = _data.Sum(x => x.PlannedGas);
            double totalActual = _data.Sum(x => x.ActualGas);
            double totalCompletion = totalPlanned > 0
                ? totalActual / totalPlanned * 100
                : 0;

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Filial", "Филиал");
            dataGridView1.Columns.Add("Planned", "План (м³)");
            dataGridView1.Columns.Add("Actual", "Факт (м³)");
            dataGridView1.Columns.Add("Completion", "Выполнение (%)");
            dataGridView1.Columns.Add("Share", "Доля (%)");

            dataGridView1.Rows.Clear();

            foreach (var item in _data)
            {
                double share = totalActual > 0
                    ? item.ActualGas / totalActual * 100
                    : 0;

                dataGridView1.Rows.Add(
                    item.FilialName,
                    item.PlannedGas.ToString("N2"),
                    item.ActualGas.ToString("N2"),
                    item.Completion.ToString("N2") + "%",
                    share.ToString("N2") + "%"
                );
            }

            dataGridView1.Rows.Add(
                "ИТОГО ПО КОМПАНИИ",
                totalPlanned.ToString("N2"),
                totalActual.ToString("N2"),
                totalCompletion.ToString("N2") + "%",
                "100%"
            );
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_data == null || _data.Count == 0)
            {
                MessageBox.Show("Сначала сформируйте отчёт.");
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Filter = "Word документ|*.docx",
                FileName = $"Отчёт_по_компании_{_selectedYear}.docx"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            ExportToWord(dlg.FileName);
            MessageBox.Show("Файл сохранён.");
        }

        private void ExportToWord(string path)
        {
            using var doc = WordprocessingDocument.Create(
                path,
                WordprocessingDocumentType.Document);

            var main = doc.AddMainDocumentPart();
            main.Document = new Document(new Body());
            var body = main.Document.Body!;

            void AddLine(string text, bool bold = false)
            {
                var run = new Run(new Text(text)
                {
                    Space = SpaceProcessingModeValues.Preserve
                });

                if (bold)
                    run.PrependChild(new RunProperties(new Bold()));

                body.AppendChild(new Paragraph(run));
            }

            double totalPlanned = _data.Sum(x => x.PlannedGas);
            double totalActual = _data.Sum(x => x.ActualGas);
            double totalCompletion = totalPlanned > 0
                ? totalActual / totalPlanned * 100
                : 0;

            AddLine("ГОДОВОЙ ОТЧЁТ ПО КОМПАНИИ", true);
            AddLine("");
            AddLine($"Год: {_selectedYear}");
            AddLine("");
            AddLine($"Общий план: {totalPlanned:N2} м³", true);
            AddLine($"Общий факт: {totalActual:N2} м³", true);
            AddLine($"Общее выполнение: {totalCompletion:N2}%", true);
            AddLine("");

            AddLine("РАСПРЕДЕЛЕНИЕ ПО ФИЛИАЛАМ", true);
            AddLine("");

            foreach (var item in _data)
            {
                double share = totalActual > 0
                    ? item.ActualGas / totalActual * 100
                    : 0;

                AddLine(
                    $"{item.FilialName}: " +
                    $"Факт {item.ActualGas:N2} м³ | " +
                    $"Доля {share:N2}%");
            }

            AddLine("");
            AddLine("Генеральный директор: _______________________");
        }
    }

        public class CompanyYearItem
    {
        public string FilialName { get; set; }
        public double PlannedGas { get; set; }
        public double ActualGas { get; set; }
        public double Completion { get; set; }
    }
}