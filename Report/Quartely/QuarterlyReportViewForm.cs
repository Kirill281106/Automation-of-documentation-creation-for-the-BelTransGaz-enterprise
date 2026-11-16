namespace вход_курсач
{
    public partial class QuarterlyReportViewForm : Form
    {
        private readonly int _reportId;
        private readonly DB_QuarterlyReport _db = new DB_QuarterlyReport();
        private QuarterlyReportListItem? _report;

        public QuarterlyReportViewForm(int reportId)
        {
            InitializeComponent();
            _reportId = reportId;

            this.Load += QuarterlyReportViewForm_Load;
            this.Load += (s, e) => Design.GazpromStyle.StyleAll(this);
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