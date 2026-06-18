namespace вход_курсач
{
    public class YearlyForecastResult
    {
        public int NextYear { get; set; }
        public double ForecastGas { get; set; }
        public double[] Percentages { get; set; }
        public bool IsGrowth { get; set; }

        public string TrendText => IsGrowth
            ? "▲ Наблюдается рост объёмов."
            : "▼ Наблюдается снижение объёмов.";

        public string ForecastText =>
            $"Прогноз на {NextYear} год: {ForecastGas:N2} м³";
    }

    public class YearlyForecastService
    {
        private readonly DB_Quarterly _db;

        public YearlyForecastService(string connection)
        {
            _db = new DB_Quarterly(connection);
        }

        public YearlyForecastResult Calculate(int userId)
        {
            var history = _db.GetYearlyReportsByUser(userId);

            if (history.Count < 3) return null;

            var last3 = history
                .OrderBy(r => r.Year)
                .Skip(Math.Max(0, history.Count - 3))
                .ToList();

            double avg = last3.Average(r => r.ActualGas);

            double[] pct = new double[4];
            pct[0] = 0;
            pct[1] = ((last3[1].ActualGas - last3[0].ActualGas) / last3[0].ActualGas) * 100;
            pct[2] = ((last3[2].ActualGas - last3[1].ActualGas) / last3[1].ActualGas) * 100;
            pct[3] = ((avg - last3[2].ActualGas) / last3[2].ActualGas) * 100;

            return new YearlyForecastResult
            {
                NextYear = last3.Last().Year + 1,
                ForecastGas = avg,
                Percentages = pct,
                IsGrowth = pct[3] >= 0
            };
        }
    }
}