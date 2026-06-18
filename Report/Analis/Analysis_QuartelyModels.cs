namespace вход_курсач
{
    public class QuartelyPoint
    {
        public int Year { get; set; }
        public int Quarter { get; set; }
        public double ActualGas { get; set; }
        public double PlannedGas { get; set; }
        public double Completion { get; set; }
        public int AbsoluteIndex { get; set; }

        public string Label => $"{Quarter}кв\n{Year}";
    }
    public class QuartelyResult
    {
        public int NextQuarter { get; set; }
        public int NextYear { get; set; }

        public double ForecastGas { get; set; }
        public double[] Percentages { get; set; }

        public double Slope { get; set; }
        public bool IsGrowth => Slope > 0;

        public string TrendText => IsGrowth
            ? $"▲ Тренд роста: +{Slope:N2} м³ за квартал"
            : $"▼ Тренд падения: {Slope:N2} м³ за квартал";

        public string ForecastText =>
            $"Прогноз на {NextQuarter} кв. {NextYear}: " +
            $"{ForecastGas:N2} м³";
    }
    public class QuarterlyForecastService
    {
        private readonly DB_Quarterly _db;

        public QuarterlyForecastService(string connection)
        {
            _db = new DB_Quarterly(connection);
        }

        public int GetFilial(int userId) =>
            _db.GetFilialByUserId(userId);

        public List<QuartelyPoint> GetHistory(int filialId) =>
            _db.GetHistoryPoints(filialId);

        public QuartelyResult Calculate(int filialId)
        {
            var allPoints = _db.GetHistoryPoints(filialId);
            if (allPoints.Count < 3) return null;

            var last3 = allPoints.Skip(Math.Max(0, allPoints.Count - 3)).ToList();

            double avgForecast = last3.Average(p => p.ActualGas);

            double[] percents = new double[4];
            percents[0] = 0;
            percents[1] = ((last3[1].ActualGas - last3[0].ActualGas) / last3[0].ActualGas) * 100;
            percents[2] = ((last3[2].ActualGas - last3[1].ActualGas) / last3[1].ActualGas) * 100;
            percents[3] = ((avgForecast - last3[2].ActualGas) / last3[2].ActualGas) * 100;

            var last = last3.Last();
            return new QuartelyResult
            {
                NextQuarter = last.Quarter == 4 ? 1 : last.Quarter + 1,
                NextYear = last.Quarter == 4 ? last.Year + 1 : last.Year,
                ForecastGas = avgForecast,
                Slope = avgForecast - last.ActualGas,
                Percentages = percents
            };
        }
    }
}