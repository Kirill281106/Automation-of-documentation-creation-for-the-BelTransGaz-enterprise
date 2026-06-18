namespace вход_курсач
{
    public partial class QuarterlyForecastForm : Form
    {
        private readonly QuarterlyForecastService _service;
        private int _filialId;

        private List<QuartelyPoint> _history;
        private QuartelyResult _forecast;
        public QuarterlyForecastForm(int userId)
        {
            InitializeComponent();

            _service = new QuarterlyForecastService(Program.ConnectionString);
            _filialId = _service.GetFilial(userId);

            LoadData();

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
        private void LoadData()
        {
            _history = _service.GetHistory(_filialId);
            _forecast = _service.Calculate(_filialId);

            if (_history == null || _history.Count < 3)
            {
                lblForecast.Text = "Недостаточно данных.";
                return;
            }

            if (_forecast != null)
            {
                lblForecast.Text = _forecast.ForecastText;
                lblTrend.Text = _forecast.TrendText;
                lblTrend.ForeColor = _forecast.IsGrowth ? Color.Green : Color.Red;
            }

            pbChart.Invalidate(); // заставить перерисовать график
        }
        private void pbChart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Получаем последние 3 точки истории для отрисовки
            var historyPoints = _history
                .Skip(Math.Max(0, _history.Count - 3))
                .ToList();

            if (historyPoints.Count < 3 || _forecast == null) return;

            int padding = 60;
            int barWidth = 70;
            int gap = 50;

            // Находим максимум для масштаба (из 3-х историй и 1 прогноза)
            float maxVal = (float)Math.Max(historyPoints.Max(p => p.ActualGas), _forecast.ForecastGas) * 1.3f;
            float chartHeight = pbChart.Height - padding * 2;

            for (int i = 0; i < 4; i++) // 0,1,2 - история, 3 - прогноз
            {
                double val = (i < 3) ? historyPoints[i].ActualGas : _forecast.ForecastGas;
                string label = (i < 3) ? historyPoints[i].Label : "ПРОГНОЗ";

                // Столбики истории - синие, прогноза - оранжевый
                Brush barBrush = (i < 3) ? Brushes.SteelBlue : Brushes.DarkOrange;

                float barHeight = (float)(val / maxVal * chartHeight);
                float x = padding + i * (barWidth + gap);
                float y = (pbChart.Height - padding) - barHeight;

                // Рисуем тень (для красоты)
                g.FillRectangle(Brushes.LightGray, x + 5, y + 5, barWidth, barHeight);
                // Рисуем основной столбик
                g.FillRectangle(barBrush, x, y, barWidth, barHeight);
                g.DrawRectangle(Pens.Black, x, y, barWidth, barHeight);

                // Подпись значения м3
                g.DrawString(val.ToString("N0"), this.Font, Brushes.Black, x, y - 20);

                // Подпись квартала
                g.DrawString(label, this.Font, Brushes.Black, x, pbChart.Height - padding + 10);

                // Проценты (над столбиком)
                if (i > 0)
                {
                    double pct = _forecast.Percentages[i];
                    string pctStr = (pct >= 0 ? "+" : "") + pct.ToString("F1") + "%";
                    Brush pctBrush = pct >= 0 ? Brushes.Green : Brushes.Red;
                    g.DrawString(pctStr, new Font(this.Font, FontStyle.Bold), pctBrush, x + 10, y - 40);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

        }
    }
}
