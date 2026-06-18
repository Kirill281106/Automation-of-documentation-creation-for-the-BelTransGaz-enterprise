using Microsoft.Data.Sqlite;
using Dapper;

namespace вход_курсач
{
    public class WorkPlanItem
    {
        public int DayNumber { get; set; }
        public decimal PlannedVolume { get; set; }
    }

    public class WorkPlanDB
    {
        private readonly string _connectionString;

        public WorkPlanDB(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<WorkPlanItem> GetMonthPlan(int filialId, int month, int year)
        {
            var days = DateTime.DaysInMonth(year, month);
            var plan = new List<WorkPlanItem>();

            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                var dbPlans = conn.Query<dynamic>(
                    @"SELECT PlanDay, PlannedVolume 
                      FROM WorkPlan 
                      WHERE FilialId = @FilialId AND PlanMonth = @Month AND PlanYear = @Year
                      ORDER BY PlanDay",
                    new { FilialId = filialId, Month = month, Year = year }).ToList();

                for (int day = 1; day <= days; day++)
                {
                    var dbPlan = dbPlans.FirstOrDefault(p => p.PlanDay == day);

                    decimal volume = 0;
                    if (dbPlan?.PlannedVolume != null)
                    {
                        // Явное преобразование double в decimal
                        volume = Convert.ToDecimal(dbPlan.PlannedVolume);
                    }

                    plan.Add(new WorkPlanItem
                    {
                        DayNumber = day,
                        PlannedVolume = volume
                    });
                }
            }

            return plan;
        }

        public bool SaveDayPlan(int filialId, int month, int year, int day, decimal volume, int userId)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    conn.Open();

                    // Проверяем, есть ли уже план на этот день
                    var existing = conn.QueryFirstOrDefault<int?>(
                        @"SELECT Id FROM WorkPlan 
                          WHERE FilialId = @FilialId AND PlanMonth = @Month 
                          AND PlanYear = @Year AND PlanDay = @Day",
                        new { FilialId = filialId, Month = month, Year = year, Day = day });

                    if (existing.HasValue)
                    {
                        // Проверяем, заблокирован ли уже созданный план
                        var isLocked = conn.QueryFirstOrDefault<int>(
                            @"SELECT IsLocked FROM WorkPlan WHERE Id = @Id",
                            new { Id = existing.Value });

                        if (isLocked == 1)
                        {
                            return false; // План заблокирован, нельзя менять
                        }

                        // Обновляем существующий план
                        conn.Execute(
                            @"UPDATE WorkPlan 
                              SET PlannedVolume = @Volume
                              WHERE Id = @Id",
                            new { Id = existing.Value, Volume = volume });
                    }
                    else
                    {
                        // Создаём новый план
                        conn.Execute(
                            @"INSERT INTO WorkPlan (FilialId, PlanMonth, PlanYear, PlanDay, PlannedVolume, CreatedBy)
                              VALUES (@FilialId, @Month, @Year, @Day, @Volume, @UserId)",
                            new { FilialId = filialId, Month = month, Year = year, Day = day, Volume = volume, UserId = userId });
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool LockMonthPlan(int filialId, int month, int year)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    conn.Open();
                    conn.Execute(
                        @"UPDATE WorkPlan 
                          SET IsLocked = 1
                          WHERE FilialId = @FilialId AND PlanMonth = @Month AND PlanYear = @Year",
                        new { FilialId = filialId, Month = month, Year = year });
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public decimal GetTotalPlanVolume(int filialId, int month, int year)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                var total = conn.QueryFirstOrDefault<decimal?>(
                    @"SELECT SUM(PlannedVolume) FROM WorkPlan 
                      WHERE FilialId = @FilialId AND PlanMonth = @Month AND PlanYear = @Year",
                    new { FilialId = filialId, Month = month, Year = year });

                return total ?? 0m;
            }
        }

        public decimal GetAveragePlanVolume(int filialId, int month, int year)
        {
            var plan = GetMonthPlan(filialId, month, year);
            var daysWithPlan = plan.Count(p => p.PlannedVolume > 0);

            if (daysWithPlan == 0)
                return 0m;

            return plan.Sum(p => p.PlannedVolume) / daysWithPlan;
        }

        public bool CanEditPlan(int filialId, int month, int year, int day)
        {
            var today = DateTime.Today;

            // Нельзя менять прошлые планы (старше 1 дня)
            var planDate = new DateTime(year, month, day);
            if (planDate < today.AddDays(-1))
                return false;

            using (var conn = new SqliteConnection(_connectionString))
            {
                var isLocked = conn.QueryFirstOrDefault<int?>(
                    @"SELECT IsLocked FROM WorkPlan 
                      WHERE FilialId = @FilialId AND PlanMonth = @Month 
                      AND PlanYear = @Year AND PlanDay = @Day",
                    new { FilialId = filialId, Month = month, Year = year, Day = day });

                return isLocked != 1;
            }
        }

        public (int MaxMonth, int MaxYear) GetMaxPlanningDate()
        {
            var today = DateTime.Today;
            var maxDate = today.AddMonths(3);
            return (maxDate.Month, maxDate.Year);
        }

        public bool IsOverPlanningLimit(int month, int year)
        {
            var (maxMonth, maxYear) = GetMaxPlanningDate();

            // Если год больше, то превышен
            if (year > maxYear)
                return true;

            // Если год равен, сравниваем месяцы
            if (year == maxYear && month > maxMonth)
                return true;

            return false;
        }
    }
}
