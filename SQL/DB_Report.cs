using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace вход_курсач
{

    internal class DB_Quarterly
    {
        private readonly string _connection;

        public DB_Quarterly(string connection)
        {
            _connection = connection;
        }

        // ─── Вспомогательный ──────────────────────────────────────────────────

        public ClassUser? GetUserById(int userId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                return db.QueryFirstOrDefault<ClassUser>(
                    "SELECT * FROM Users WHERE Id = @Id", new { Id = userId });
            }
        }

        // ─── Сменные отчёты ───────────────────────────────────────────────────

        public int GetNextReportNumber()
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                long max = db.ExecuteScalar<long>(
                    "SELECT COALESCE(MAX(ReportNumber),0) FROM ShiftReports");
                return (int)max + 1;
            }
        }
        public string GetFilialNameById(int id)
        {
            using var conn = new SqliteConnection(_connection);
            conn.Open();
            return conn.QueryFirstOrDefault<string>(
                "SELECT Name FROM Filials WHERE Id = @Id", new { Id = id }) ?? id.ToString();
        }
        public int SaveShiftReport(ShiftReportData r)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                if (r.Id == 0)
                {
                    string sql = @"
                INSERT INTO ShiftReports
                    (ReportNumber, ReportDate, FilialId, ShiftType, ShiftManagerId,
                     NextManagerId, PlannedGas, ActualGas, Completion, TransferNote)
                VALUES
                    (@ReportNumber, @ReportDate, @FilialId, @ShiftType, @ShiftManagerId,
                     @NextManagerId, @PlannedGas, @ActualGas, @Completion, @TransferNote);
                SELECT last_insert_rowid();";

                    return db.ExecuteScalar<int>(sql, r);
                }
                else
                {
                    string sql = @"
                UPDATE ShiftReports
                SET ReportNumber = @ReportNumber,
                    ReportDate = @ReportDate,
                    FilialId = @FilialId,
                    ShiftType = @ShiftType,
                    ShiftManagerId = @ShiftManagerId,
                    NextManagerId = @NextManagerId,
                    PlannedGas = @PlannedGas,
                    ActualGas = @ActualGas,
                    Completion = @Completion,
                    TransferNote = @TransferNote
                WHERE Id = @Id";

                    db.Execute(sql, r);
                    return r.Id;
                }
            }
        }

        public List<ShiftReportListItem> GetShiftReports(int filialId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string sql = @"
            SELECT
                sr.Id,
                sr.ReportNumber,
                sr.ReportDate,
                sr.ShiftType,
                sm.Surname || ' ' || sm.Name AS ManagerName,
                sr.PlannedGas,
                sr.ActualGas,
                sr.Completion
            FROM ShiftReports sr
            INNER JOIN Users sm ON sm.Id = sr.ShiftManagerId
            WHERE sr.FilialId = @FilialId
            ORDER BY sr.ReportNumber DESC";

                return db.Query<ShiftReportListItem>(sql, new { FilialId = filialId }).ToList();
            }
        }

        public ShiftReportData? GetShiftReportById(int id)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                return db.QueryFirstOrDefault<ShiftReportData>(
                    "SELECT * FROM ShiftReports WHERE Id = @Id", new { Id = id });
            }
        }

        public bool DeleteShiftReport(int id)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                return db.Execute("DELETE FROM ShiftReports WHERE Id = @Id", new { Id = id }) > 0;
            }
        }

        // Состав смены (люди с таким же TeamNum в филиале)
        public List<EmployeeItem> GetTeamMembers(int filialId, int teamNum, int excludeUserId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string sql = @"
            SELECT 
                u.Id,
                u.Surname || ' ' || u.Name ||
                CASE WHEN u.SecondName IS NOT NULL AND u.SecondName != ''
                     THEN ' ' || u.SecondName ELSE '' END AS FullName
            FROM Users u
            WHERE u.Filial = @Filial 
              AND u.TeamNum = @TeamNum
              AND u.IsActive = 1
              AND u.Id != @ExcludeUserId
            ORDER BY u.Surname, u.Name";

                return db.Query<EmployeeItem>(sql,
                    new { Filial = filialId, TeamNum = teamNum, ExcludeUserId = excludeUserId }).ToList();
            }
        }

        // Только начальники смены в филиале
        public List<EmployeeItem> GetShiftManagersInFilial(int filialId, int excludeUserId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string sql = @"
            SELECT u.Id,
                   u.Surname || ' ' || u.Name ||
                   CASE WHEN u.SecondName IS NOT NULL AND u.SecondName != ''
                        THEN ' ' || u.SecondName ELSE '' END AS FullName
            FROM Users u
            WHERE u.Filial = @Filial 
              AND u.IsActive = 1
              AND u.Status = 3
              AND u.Id <> @Exclude
            ORDER BY u.Surname";

                return db.Query<EmployeeItem>(sql,
                    new { Filial = filialId, Exclude = excludeUserId }).ToList();
            }
        }

        public double GetDailyPlanVolume(int filialId, DateTime date)
{
    using (IDbConnection db = new SqliteConnection(_connection))
    {
        double result = db.ExecuteScalar<double?>(
            @"SELECT PlannedVolume FROM WorkPlan 
              WHERE FilialId = @FilialId 
                AND PlanYear = @Year 
                AND PlanMonth = @Month 
                AND PlanDay = @Day",
            new { 
                FilialId = filialId, 
                Year = date.Year, 
                Month = date.Month, 
                Day = date.Day 
            }) ?? 0;
        
        return result;
    }
}

        // ─── Квартальные отчёты ───────────────────────────────────────────────
        public OperationResult CreateQuarterlyReport(
            int currentUserId,
            int quarter,
            int year,
            double purchasePrice,
            double salePrice)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                var cur = db.QueryFirstOrDefault<ClassUser>(
                    "SELECT * FROM Users WHERE Id = @Id",
                    new { Id = currentUserId });

                if (cur == null)
                    return OperationResult.Fail("Пользователь не найден.");

                DateTime today = DateTime.Today;

                if (year > today.Year)
                    return OperationResult.Fail("Нельзя создавать отчёт за будущий год.");

                int endMonth = quarter * 3;
                DateTime quarterEndDate = new DateTime(
                    year,
                    endMonth,
                    DateTime.DaysInMonth(year, endMonth));

                if (today <= quarterEndDate)
                    return OperationResult.Fail(
                        "Квартал ещё не завершён. Создание отчёта невозможно.");

                long exists = db.ExecuteScalar<long>(
                    @"SELECT COUNT(1)
              FROM QuarterlyReports
              WHERE Quarter = @Q
                AND Year = @Y
                AND FilialId = @F",
                    new { Q = quarter, Y = year, F = cur.Filial });

                if (exists > 0)
                    return OperationResult.Fail(
                        "Отчёт за этот квартал уже существует.");

                int startMonth = (quarter - 1) * 3 + 1;

                DateTime dateFrom = new DateTime(year, startMonth, 1);
                DateTime dateTo = new DateTime(
                    year,
                    endMonth,
                    DateTime.DaysInMonth(year, endMonth));

                var agg = db.QueryFirstOrDefault<GasAggregation>(
                    @"SELECT
                COALESCE(SUM(PlannedGas), 0) AS Planned,
                COALESCE(SUM(ActualGas),  0) AS Actual
              FROM ShiftReports
              WHERE FilialId = @Filial
                AND ReportDate BETWEEN @From AND @To",
                    new
                    {
                        Filial = cur.Filial,
                        From = dateFrom.ToString("yyyy-MM-dd"),
                        To = dateTo.ToString("yyyy-MM-dd")
                    });

                double planned = agg?.Planned ?? 0;
                double actual = agg?.Actual ?? 0;

                double completion = planned > 0
                    ? Math.Round(actual / planned * 100, 2)
                    : 0;

                double revenue = (actual / 1000.0) * salePrice;
                double cost = (actual / 1000.0) * purchasePrice;
                double profit = revenue - cost;

                int? managerId = db.ExecuteScalar<int?>(
                    "SELECT ManagerId FROM Filials WHERE Id = @Id",
                    new { Id = cur.Filial });

                db.Execute(@"
            INSERT INTO QuarterlyReports
                (Quarter, Year, FilialId,
                 CompiledById, AddressedToId, CreatedAt,
                 PlannedGas, ActualGas, Completion,
                 PurchasePrice, SalePrice,
                 Revenue, Cost, Profit)
            VALUES
                (@Quarter, @Year, @FilialId,
                 @CompiledById, @AddressedToId, @CreatedAt,
                 @PlannedGas, @ActualGas, @Completion,
                 @PurchasePrice, @SalePrice,
                 @Revenue, @Cost, @Profit)",
                    new
                    {
                        Quarter = quarter,
                        Year = year,
                        FilialId = cur.Filial,
                        CompiledById = currentUserId,
                        AddressedToId = (object?)managerId ?? DBNull.Value,
                        CreatedAt = today.ToString("yyyy-MM-dd"),
                        PlannedGas = planned,
                        ActualGas = actual,
                        Completion = completion,
                        PurchasePrice = purchasePrice,
                        SalePrice = salePrice,
                        Revenue = revenue,
                        Cost = cost,
                        Profit = profit
                    });

                return OperationResult.Ok();
            }
        }
        public List<QuarterlyReportListItem> GetQuarterlyReports(int userId)
		{
			var list = new List<QuarterlyReportListItem>();

			using var conn = new SqliteConnection(_connection);
			conn.Open();

			string sql = @"
        SELECT 
            qr.Id,
            qr.Quarter,
            qr.Year,
            f.Name AS FilialName,
            u.Name || ' ' || u.Surname AS CompiledBy,
            COALESCE(u2.Name || ' ' || u2.Surname, 'Генеральный директор') AS AddressedTo,
            qr.CreatedAt,
            qr.PlannedGas,
            qr.ActualGas,
            qr.Completion,
            qr.PurchasePrice,
            qr.SalePrice,
            qr.Revenue,
            qr.Cost,
            qr.Profit
        FROM QuarterlyReports qr
        JOIN Filials f ON qr.FilialId = f.Id
        JOIN Users u ON qr.CompiledById = u.Id
        LEFT JOIN Users u2 ON qr.AddressedToId = u2.Id
        WHERE u.Filial = (SELECT Filial FROM Users WHERE Id = @userId)";

			using var cmd = new SqliteCommand(sql, conn);
			cmd.Parameters.AddWithValue("@userId", userId);

			using var reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				list.Add(new QuarterlyReportListItem
				{
					Id = reader.GetInt32(0),
					Quarter = reader.GetInt32(1),
					Year = reader.GetInt32(2),
					FilialName = reader.GetString(3),
					CompiledBy = reader.GetString(4),
					AddressedTo = reader.GetString(5),
					CreatedAt = reader.GetString(6),
					PlannedGas = reader.GetDouble(7),
					ActualGas = reader.GetDouble(8),
					Completion = reader.GetDouble(9),
					PurchasePrice = reader.GetDouble(10),
					SalePrice = reader.GetDouble(11),
					Revenue = reader.GetDouble(12),
					Cost = reader.GetDouble(13),
					Profit = reader.GetDouble(14)
				});
			}

			return list;
		}
		public QuarterlyReportListItem GetQuarterlyReportById(int reportId)
		{
			using var conn = new SqliteConnection(_connection);
			conn.Open();

			string sql = @"
        SELECT 
            qr.Id,
            qr.Quarter,
            qr.Year,
            f.Name AS FilialName,
            u.Name || ' ' || u.Surname AS CompiledBy,
            COALESCE(u2.Name || ' ' || u2.Surname, 'Генеральный директор') AS AddressedTo,
            qr.CreatedAt,
            qr.PlannedGas,
            qr.ActualGas,
            qr.Completion,
            qr.PurchasePrice,
            qr.SalePrice,
            qr.Revenue,
            qr.Cost,
            qr.Profit
        FROM QuarterlyReports qr
        JOIN Filials f ON qr.FilialId = f.Id
        JOIN Users u ON qr.CompiledById = u.Id
        LEFT JOIN Users u2 ON qr.AddressedToId = u2.Id
        WHERE qr.Id = @id";

			using var cmd = new SqliteCommand(sql, conn);
			cmd.Parameters.AddWithValue("@id", reportId);

			using var reader = cmd.ExecuteReader();
			if (reader.Read())
			{
				return new QuarterlyReportListItem
				{
					Id = reader.GetInt32(0),
					Quarter = reader.GetInt32(1),
					Year = reader.GetInt32(2),
					FilialName = reader.GetString(3),
					CompiledBy = reader.GetString(4),
					AddressedTo = reader.GetString(5),
					CreatedAt = reader.GetString(6),
					PlannedGas = reader.GetDouble(7),
					ActualGas = reader.GetDouble(8),
					Completion = reader.GetDouble(9),
					PurchasePrice = reader.GetDouble(10),
					SalePrice = reader.GetDouble(11),
					Revenue = reader.GetDouble(12),
					Cost = reader.GetDouble(13),
					Profit = reader.GetDouble(14)
				};
			}

			return null;
		}
        // ─── Годовые отчёты ───────────────────────────────────────────────────

        public OperationResult CreateYearlyReport(int userId, int year)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                var cur = db.QueryFirstOrDefault<ClassUser>("SELECT * FROM Users WHERE Id = @Id", new { Id = userId });
                if (cur == null) return OperationResult.Fail("Пользователь не найден.");

                var quarters = db.Query<QuarterlyReportListItem>(
                    @"SELECT 
                        Id,
                        Quarter,
                        Year,
                        PlannedGas,
                        ActualGas,
                        Completion,
                        PurchasePrice,
                        SalePrice,
                        Revenue,
                        Cost,
                        Profit
                      FROM QuarterlyReports 
                      WHERE Year = @Y AND FilialId = @F",
                    new { Y = year, F = cur.Filial }).ToList();

                // Если кварталов нет — ничего не создаем
                if (quarters.Count == 0)
                    return OperationResult.Fail($"Нет квартальных отчетов за {year} год. Создание невозможно.");

                // 2. Суммируем всё, что есть (1, 2, 3 или 4 квартала)
                double totalPlanned = quarters.Sum(q => q.PlannedGas);
                double totalActual = quarters.Sum(q => q.ActualGas);
                double totalRevenue = quarters.Sum(q => q.Revenue);
                double totalCost = quarters.Sum(q => q.Cost);
                double totalProfit = quarters.Sum(q => q.Profit);

                // 3. СРЕДНИЕ ЦЕНЫ (взвешенное среднее по объёму прокачки)
                double avgPurchasePrice = quarters.Average(q => q.PurchasePrice);
                double avgSalePrice = quarters.Average(q => q.SalePrice);

                double totalCompletion = totalPlanned > 0
                    ? Math.Round(totalActual / totalPlanned * 100, 2) : 0;

                int? managerId = db.ExecuteScalar<int?>(
                    "SELECT ManagerId FROM Filials WHERE Id = @Id", new { Id = cur.Filial });

                // 4. Сохраняем (INSERT OR REPLACE обновит отчет, если вы добавите позже еще квартал)
                db.Execute(@"
            INSERT OR REPLACE INTO YearlyReports
                (Year, FilialId, CompiledById, AddressedToId, CreatedAt, 
                 PlannedGas, ActualGas, Completion, 
                 PurchasePrice, SalePrice, Revenue, Cost, Profit)
            VALUES
                (@Year, @FilialId, @CompiledById, @AddressedToId, @CreatedAt, 
                 @PlannedGas, @ActualGas, @Completion, 
                 @PurchasePrice, @SalePrice, @Revenue, @Cost, @Profit)",
                    new
                    {
                        Year = year,
                        FilialId = cur.Filial,
                        CompiledById = userId,
                        AddressedToId = (object?)managerId ?? DBNull.Value,
                        CreatedAt = DateTime.Today.ToString("yyyy-MM-dd"),
                        PlannedGas = totalPlanned,
                        ActualGas = totalActual,
                        Completion = totalCompletion,
                        PurchasePrice = avgPurchasePrice,
                        SalePrice = avgSalePrice,
                        Revenue = totalRevenue,
                        Cost = totalCost,
                        Profit = totalProfit
                    });

                return OperationResult.Ok();
            }
        }


        public List<QuarterlyReportListItem> GetYearlyReportsByUser(int currentUserId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                var cur = db.QueryFirstOrDefault<ClassUser>(
                    "SELECT * FROM Users WHERE Id = @Id", new { Id = currentUserId });
                if (cur == null) return new();

                string sql = @"
        SELECT
            yr.Id,
            0 AS Quarter,
            yr.Year,
            f.Name AS FilialName,
            (cb.Surname || ' ' || cb.Name) AS CompiledBy,
            COALESCE(ad.Surname || ' ' || ad.Name, '—') AS AddressedTo,
            yr.CreatedAt,
            yr.PlannedGas,
            yr.ActualGas,
            yr.Completion,
            yr.PurchasePrice,
            yr.SalePrice,
            yr.Revenue,
            yr.Cost,
            yr.Profit
        FROM YearlyReports yr
        INNER JOIN Filials f ON f.Id = yr.FilialId
        INNER JOIN Users cb ON cb.Id = yr.CompiledById
        LEFT JOIN Users ad ON ad.Id = yr.AddressedToId
        WHERE yr.FilialId = @Filial
        ORDER BY yr.Year DESC";

                return db.Query<QuarterlyReportListItem>(sql, new { Filial = cur.Filial }).ToList();
            }
        }
        public QuarterlyReportListItem? GetYearlyReportById(int id)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string sql = @"
        SELECT
            yr.Id,
            0 AS Quarter,
            yr.Year,
            f.Name AS FilialName,
            (cb.Surname || ' ' || cb.Name) AS CompiledBy,
            COALESCE(ad.Surname || ' ' || ad.Name, '—') AS AddressedTo,
            yr.CreatedAt,
            yr.PlannedGas,
            yr.ActualGas,
            yr.Completion,
            yr.PurchasePrice,
            yr.SalePrice,
            yr.Revenue,
            yr.Cost,
            yr.Profit
        FROM YearlyReports yr
        INNER JOIN Filials f ON f.Id = yr.FilialId
        INNER JOIN Users cb ON cb.Id = yr.CompiledById
        LEFT JOIN Users ad ON ad.Id = yr.AddressedToId
        WHERE yr.Id = @Id";

                return db.QueryFirstOrDefault<QuarterlyReportListItem>(sql, new { Id = id });
            }
        }
        public int GetFilialByUserId(int userId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                return db.ExecuteScalar<int>(
                    "SELECT Filial FROM Users WHERE Id = @Id", new { Id = userId });
            }
        }

        public List<QuartelyPoint> GetHistoryPoints(int filialId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string sql = @"
                SELECT
                    qr.Year,
                    qr.Quarter,
                    qr.ActualGas,
                    qr.PlannedGas,
                    qr.Completion,
                    (qr.Year * 4 + qr.Quarter) AS AbsoluteIndex
                FROM QuarterlyReports qr
                WHERE qr.FilialId = @FilialId
                ORDER BY qr.Year ASC, qr.Quarter ASC";

                return db.Query<QuartelyPoint>(sql, new { FilialId = filialId }).ToList();
            }
        }

        // Сводные данные по всем филиалам за год (для CompanyYearlyReportForm)
        public List<CompanyYearItem> GetCompanyYearlyData(int year)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string sql = @"
        SELECT
            f.Name AS FilialName,
            SUM(yr.PlannedGas) AS PlannedGas,
            SUM(yr.ActualGas)  AS ActualGas,
            CASE
                WHEN SUM(yr.PlannedGas) > 0
                THEN SUM(yr.ActualGas) * 100.0 / SUM(yr.PlannedGas)
                ELSE 0
            END AS Completion
        FROM YearlyReports yr
        INNER JOIN Filials f ON f.Id = yr.FilialId
        WHERE yr.Year = @Year
        GROUP BY f.Name
        ORDER BY f.Name";

                return db.Query<CompanyYearItem>(sql, new { Year = year }).ToList();
            }
        }

        // Все годовые отчёты компании за год с деталями составителя
        public List<QuarterlyReportListItem> GetAllYearlyReports(int year)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string sql = @"
        SELECT
            yr.Id,
            yr.Year,
            f.Name AS FilialName,
            (cb.Surname || ' ' || cb.Name) AS CompiledBy,
            COALESCE(ad.Surname || ' ' || ad.Name, '—') AS AddressedTo,
            yr.CreatedAt,
            yr.PlannedGas,
            yr.ActualGas,
            yr.Completion
        FROM YearlyReports yr
        INNER JOIN Filials f  ON f.Id  = yr.FilialId
        INNER JOIN Users   cb ON cb.Id = yr.CompiledById
        LEFT  JOIN Users   ad ON ad.Id = yr.AddressedToId
        WHERE yr.Year = @Year
        ORDER BY f.Name";

                return db.Query<QuarterlyReportListItem>(sql, new { Year = year }).ToList();
            }
        }

        // ─── Вспомогательный класс ────────────────────────────────────────────

        private class GasAggregation
        {
            public double Planned { get; set; }
            public double Actual { get; set; }
        }
    }
}
