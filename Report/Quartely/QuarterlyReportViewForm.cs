using Control = System.Windows.Forms.Control;

namespace вход_курсач
{
    public partial class QuarterlyReportViewForm : Form
    {
        private readonly int _reportId;
        private readonly DB_Quarterly _db;
        private QuarterlyReportListItem? _report;

        public QuarterlyReportViewForm(int reportId, string connection)
        {
            InitializeComponent();
            _reportId = reportId;
            _db = new DB_Quarterly(connection);

            // ИСПРАВЛЕНИЕ: загружаем после того как форма создана,
            // в событии Load, а не в конструкторе
            this.Load += QuarterlyReportViewForm_Load;
            this.Load += (s, e) => ApplyGazpromStyle();
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

        private void QuarterlyReportViewForm_Load(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            _report = _db.GetQuarterlyReportById(_reportId);

            if (_report == null)
            {
                MessageBox.Show("Отчёт не найден.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                // ИСПРАВЛЕНИЕ: используем BeginInvoke чтобы закрыть ПОСЛЕ
                // полной инициализации формы
                this.BeginInvoke(new Action(() => this.Close()));
                return;
            }

            listBoxReport.Items.Clear();
            listBoxReport.Items.Add("КВАРТАЛЬНЫЙ ОТЧЁТ");
            listBoxReport.Items.Add("");
            listBoxReport.Items.Add($"Квартал:              {_report.Quarter} кв. {_report.Year} г.");
            listBoxReport.Items.Add($"Филиал:               {_report.FilialName}");
            listBoxReport.Items.Add($"Дата составления:     {_report.CreatedAt}");
            listBoxReport.Items.Add($"Составил:             {_report.CompiledBy}");
            listBoxReport.Items.Add($"На имя:               {_report.AddressedTo}");
            listBoxReport.Items.Add("");
            listBoxReport.Items.Add($"Запланировано (м³):   {_report.PlannedGas:N2}");
            listBoxReport.Items.Add($"Прокачано (м³):       {_report.ActualGas:N2}");
            listBoxReport.Items.Add($"Выполнение:           {_report.Completion:N2}%");
        }

        private void btnExportWord_Click(object sender, EventArgs e)
        {
            if (_report == null) return;

            using var dlg = new SaveFileDialog
            {
                Filter = "Word документ|*.docx",
                FileName = $"Квартальный_отчёт_{_report.Quarter}кв{_report.Year}.docx"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            
            CreateQuarterlyReportForm.ExportToWord(
                _report,
                dlg.FileName,
                null,
                null,  
                null,
                _report.PurchasePrice,
                _report.SalePrice
            );

            MessageBox.Show("Файл сохранён.", "Экспорт",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}