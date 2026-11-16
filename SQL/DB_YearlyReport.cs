using Dapper;

namespace вход_курсач
{
    // Годовые отчёты: создание, список, просмотр, сводка по компании.
    internal class DB_YearlyReport : DB_Base
    {
        // Общая часть запросов: отчёт + филиал + кто составил + кому адресован.
        // Годовой отчёт показывается той же моделью, что и квартальный, поэтому Quarter = 0.
        private const string ReportSelectSql = @"
            SELECT
                yr.Id,
                0 AS Quarter,
                yr.Year,
                f.Name AS FilialName,
                cb.Surname || ' ' || cb.Name AS CompiledBy,
                COALESCE(ad.Surname || ' ' || ad.Name, '—') AS AddressedTo,
                to_char(yr.CreatedAt, 'YYYY-MM-DD') AS CreatedAt,
                yr.PlannedGas,
                yr.ActualGas,
                yr.Completion,
                yr.PurchasePrice,
                yr.SalePrice,
                yr.Revenue,
                yr.Cost,
                yr.Profit
            FROM YearlyReports yr
            INNER JOIN Filials f  ON f.Id  = yr.FilialId
            INNER JOIN Users   cb ON cb.Id = yr.CompiledById
            LEFT  JOIN Users   ad ON ad.Id = yr.AddressedToId";

        // Создаёт годовой отчёт филиала пользователя из квартальных отчётов за этот год.
        // Если отчёт за год уже есть, он пересчитывается (например, когда добавился ещё один квартал).
        public OperationResult CreateYearlyReport(int userId, int year)
        {
            using var db = Open();

            var currentUser = FindUser(db, userId);
            if (currentUser == null)
                return OperationResult.Fail("Пользователь не найден.");

            var quarters = db.Query<QuarterlyReportListItem>(@"
                SELECT PlannedGas, ActualGas, PurchasePrice, SalePrice, Revenue, Cost, Profit
                FROM QuarterlyReports
                WHERE Year = @Year AND FilialId = @Filial",
                new { Year = year, Filial = currentUser.Filial }).ToList();

            if (quarters.Count == 0)
                return OperationResult.Fail($"Нет квартальных отчетов за {year} год. Создание невозможно.");

            // Объёмы и деньги суммируются, цены берутся средние по кварталам
            double totalPlanned = quarters.Sum(q => q.PlannedGas);
            double totalActual = quarters.Sum(q => q.ActualGas);
            double completion = totalPlanned > 0 ? Math.Round(totalActual / totalPlanned * 100, 2) : 0;

            // Отчёт адресуется начальнику филиала
            int? managerId = db.ExecuteScalar<int?>(
                "SELECT ManagerId FROM Filials WHERE Id = @Id", new { Id = currentUser.Filial });

            db.Execute(@"
                INSERT INTO YearlyReports
                    (Year, FilialId, CompiledById, AddressedToId, CreatedAt,
                     PlannedGas, ActualGas, Completion,
                     PurchasePrice, SalePrice, Revenue, Cost, Profit)
                VALUES
                    (@Year, @FilialId, @CompiledById, @AddressedToId, @CreatedAt,
                     @PlannedGas, @ActualGas, @Completion,
                     @PurchasePrice, @SalePrice, @Revenue, @Cost, @Profit)
                ON CONFLICT (Year, FilialId) DO UPDATE SET
                    CompiledById  = EXCLUDED.CompiledById,
                    AddressedToId = EXCLUDED.AddressedToId,
                    CreatedAt     = EXCLUDED.CreatedAt,
                    PlannedGas    = EXCLUDED.PlannedGas,
                    ActualGas     = EXCLUDED.ActualGas,
                    Completion    = EXCLUDED.Completion,
                    PurchasePrice = EXCLUDED.PurchasePrice,
                    SalePrice     = EXCLUDED.SalePrice,
                    Revenue       = EXCLUDED.Revenue,
                    Cost          = EXCLUDED.Cost,
                    Profit        = EXCLUDED.Profit",
                new
                {
                    Year = year,
                    FilialId = currentUser.Filial,
                    CompiledById = userId,
                    AddressedToId = managerId,
                    CreatedAt = DateTime.Today,
                    PlannedGas = totalPlanned,
                    ActualGas = totalActual,
                    Completion = completion,
                    PurchasePrice = quarters.Average(q => q.PurchasePrice),
                    SalePrice = quarters.Average(q => q.SalePrice),
                    Revenue = quarters.Sum(q => q.Revenue),
                    Cost = quarters.Sum(q => q.Cost),
                    Profit = quarters.Sum(q => q.Profit)
                });

            return OperationResult.Ok();
        }

        // Годовые отчёты филиала пользователя, новые сверху
        public List<QuarterlyReportListItem> GetYearlyReportsByUser(int currentUserId)
        {
            using var db = Open();
            string sql = $@"
                {ReportSelectSql}
                WHERE yr.FilialId = (SELECT Filial FROM Users WHERE Id = @UserId)
                ORDER BY yr.Year DESC";

            return db.Query<QuarterlyReportListItem>(sql, new { UserId = currentUserId }).ToList();
        }

        public QuarterlyReportListItem? GetYearlyReportById(int id)
        {
            using var db = Open();
            return db.QueryFirstOrDefault<QuarterlyReportListItem>(
                $"{ReportSelectSql} WHERE yr.Id = @Id", new { Id = id });
        }

        // Сводка по всем филиалам за год (окно «О работе компании»)
        public List<CompanyYearItem> GetCompanyYearlyData(int year)
        {
            using var db = Open();
            return db.Query<CompanyYearItem>(@"
                SELECT
                    f.Name AS FilialName,
                    SUM(yr.PlannedGas) AS PlannedGas,
                    SUM(yr.ActualGas)  AS ActualGas,
                    CASE WHEN SUM(yr.PlannedGas) > 0
                         THEN SUM(yr.ActualGas) * 100.0 / SUM(yr.PlannedGas)
                         ELSE 0
                    END AS Completion
                FROM YearlyReports yr
                INNER JOIN Filials f ON f.Id = yr.FilialId
                WHERE yr.Year = @Year
                GROUP BY f.Name
                ORDER BY f.Name",
                new { Year = year }).ToList();
        }
    }
}
