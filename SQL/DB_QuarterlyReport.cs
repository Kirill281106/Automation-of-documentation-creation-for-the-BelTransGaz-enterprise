using Dapper;

namespace вход_курсач
{
    // Квартальные отчёты: создание, список, просмотр, данные для прогноза.
    internal class DB_QuarterlyReport : DB_Base
    {
        // Общая часть запросов: отчёт + филиал + кто составил + кому адресован
        private const string ReportSelectSql = @"
            SELECT
                qr.Id,
                qr.Quarter,
                qr.Year,
                f.Name AS FilialName,
                u.Name || ' ' || u.Surname AS CompiledBy,
                COALESCE(u2.Name || ' ' || u2.Surname, 'Генеральный директор') AS AddressedTo,
                to_char(qr.CreatedAt, 'YYYY-MM-DD') AS CreatedAt,
                qr.PlannedGas,
                qr.ActualGas,
                qr.Completion,
                qr.PurchasePrice,
                qr.SalePrice,
                qr.Revenue,
                qr.Cost,
                qr.Profit
            FROM QuarterlyReports qr
            INNER JOIN Filials f  ON f.Id  = qr.FilialId
            INNER JOIN Users   u  ON u.Id  = qr.CompiledById
            LEFT  JOIN Users   u2 ON u2.Id = qr.AddressedToId";

        // Создаёт квартальный отчёт филиала пользователя:
        // план и факт суммируются по сменным отчётам за квартал, по ценам считается прибыль.
        public OperationResult CreateQuarterlyReport(int currentUserId, int quarter, int year,
            double purchasePrice, double salePrice)
        {
            using var db = Open();

            var currentUser = FindUser(db, currentUserId);
            if (currentUser == null)
                return OperationResult.Fail("Пользователь не найден.");

            DateTime today = DateTime.Today;
            if (year > today.Year)
                return OperationResult.Fail("Нельзя создавать отчёт за будущий год.");

            int startMonth = (quarter - 1) * 3 + 1;
            int endMonth = quarter * 3;
            DateTime dateFrom = new DateTime(year, startMonth, 1);
            DateTime dateTo = new DateTime(year, endMonth, DateTime.DaysInMonth(year, endMonth));

            if (today <= dateTo)
                return OperationResult.Fail("Квартал ещё не завершён. Создание отчёта невозможно.");

            long exists = db.ExecuteScalar<long>(@"
                SELECT COUNT(*) FROM QuarterlyReports
                WHERE Quarter = @Quarter AND Year = @Year AND FilialId = @Filial",
                new { Quarter = quarter, Year = year, Filial = currentUser.Filial });
            if (exists > 0)
                return OperationResult.Fail("Отчёт за этот квартал уже существует.");

            var (planned, actual) = db.QueryFirst<(double, double)>(@"
                SELECT COALESCE(SUM(PlannedGas), 0), COALESCE(SUM(ActualGas), 0)
                FROM ShiftReports
                WHERE FilialId = @Filial AND ReportDate BETWEEN @From AND @To",
                new { Filial = currentUser.Filial, From = dateFrom, To = dateTo });

            double completion = planned > 0 ? Math.Round(actual / planned * 100, 2) : 0;
            double revenue = actual / 1000.0 * salePrice;
            double cost = actual / 1000.0 * purchasePrice;

            // Отчёт адресуется начальнику филиала
            int? managerId = db.ExecuteScalar<int?>(
                "SELECT ManagerId FROM Filials WHERE Id = @Id", new { Id = currentUser.Filial });

            db.Execute(@"
                INSERT INTO QuarterlyReports
                    (Quarter, Year, FilialId, CompiledById, AddressedToId, CreatedAt,
                     PlannedGas, ActualGas, Completion,
                     PurchasePrice, SalePrice, Revenue, Cost, Profit)
                VALUES
                    (@Quarter, @Year, @FilialId, @CompiledById, @AddressedToId, @CreatedAt,
                     @PlannedGas, @ActualGas, @Completion,
                     @PurchasePrice, @SalePrice, @Revenue, @Cost, @Profit)",
                new
                {
                    Quarter = quarter,
                    Year = year,
                    FilialId = currentUser.Filial,
                    CompiledById = currentUserId,
                    AddressedToId = managerId,
                    CreatedAt = today,
                    PlannedGas = planned,
                    ActualGas = actual,
                    Completion = completion,
                    PurchasePrice = purchasePrice,
                    SalePrice = salePrice,
                    Revenue = revenue,
                    Cost = cost,
                    Profit = revenue - cost
                });

            return OperationResult.Ok();
        }

        // Квартальные отчёты филиала пользователя
        public List<QuarterlyReportListItem> GetQuarterlyReports(int userId)
        {
            using var db = Open();
            string sql = $@"
                {ReportSelectSql}
                WHERE u.Filial = (SELECT Filial FROM Users WHERE Id = @UserId)
                ORDER BY qr.Id";

            return db.Query<QuarterlyReportListItem>(sql, new { UserId = userId }).ToList();
        }

        public QuarterlyReportListItem? GetQuarterlyReportById(int reportId)
        {
            using var db = Open();
            return db.QueryFirstOrDefault<QuarterlyReportListItem>(
                $"{ReportSelectSql} WHERE qr.Id = @Id", new { Id = reportId });
        }

        // История по кварталам для графика и прогноза
        public List<QuartelyPoint> GetHistoryPoints(int filialId)
        {
            using var db = Open();
            return db.Query<QuartelyPoint>(@"
                SELECT
                    Year,
                    Quarter,
                    ActualGas,
                    PlannedGas,
                    Completion,
                    (Year * 4 + Quarter) AS AbsoluteIndex
                FROM QuarterlyReports
                WHERE FilialId = @FilialId
                ORDER BY Year, Quarter",
                new { FilialId = filialId }).ToList();
        }
    }
}
