using Dapper;

namespace вход_курсач
{
    // План на один день месяца
    public class WorkPlanItem
    {
        public int DayNumber { get; set; }
        public decimal PlannedVolume { get; set; }
    }

    // Планирование: план поставки газа по дням месяца.
    internal class DB_WorkPlan : DB_Base
    {
        // План на каждый день месяца. Для дней, на которые плана ещё нет, объём равен 0.
        public List<WorkPlanItem> GetMonthPlan(int filialId, int month, int year)
        {
            using var db = Open();
            var saved = db.Query<WorkPlanItem>(@"
                SELECT PlanDay AS DayNumber, PlannedVolume
                FROM WorkPlan
                WHERE FilialId = @FilialId AND PlanMonth = @Month AND PlanYear = @Year",
                new { FilialId = filialId, Month = month, Year = year })
                .ToDictionary(p => p.DayNumber, p => p.PlannedVolume);

            var plan = new List<WorkPlanItem>();
            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                plan.Add(new WorkPlanItem
                {
                    DayNumber = day,
                    PlannedVolume = saved.GetValueOrDefault(day)
                });
            }

            return plan;
        }

        // План филиала на конкретный день (0, если плана нет). Нужен сменному отчёту.
        public double GetDailyPlanVolume(int filialId, DateTime date)
        {
            using var db = Open();
            return db.ExecuteScalar<double?>(@"
                SELECT PlannedVolume FROM WorkPlan
                WHERE FilialId = @FilialId AND PlanYear = @Year AND PlanMonth = @Month AND PlanDay = @Day",
                new { FilialId = filialId, date.Year, date.Month, date.Day }) ?? 0;
        }

        // Сохраняет план на день: если записи нет — добавляет, если есть — меняет объём.
        // Заблокированный план (IsLocked = 1) не меняется, тогда возвращается false.
        public bool SaveDayPlan(int filialId, int month, int year, int day, decimal volume, int userId)
        {
            try
            {
                using var db = Open();
                int rows = db.Execute(@"
                    INSERT INTO WorkPlan (FilialId, PlanMonth, PlanYear, PlanDay, PlannedVolume, CreatedBy)
                    VALUES (@FilialId, @Month, @Year, @Day, @Volume, @UserId)
                    ON CONFLICT (FilialId, PlanYear, PlanMonth, PlanDay)
                    DO UPDATE SET PlannedVolume = EXCLUDED.PlannedVolume
                    WHERE WorkPlan.IsLocked = 0",
                    new { FilialId = filialId, Month = month, Year = year, Day = day, Volume = volume, UserId = userId });

                return rows > 0;
            }
            catch
            {
                return false;
            }
        }

        // План можно менять, если день не старше вчерашнего и запись не заблокирована
        public bool CanEditPlan(int filialId, int month, int year, int day)
        {
            if (new DateTime(year, month, day) < DateTime.Today.AddDays(-1))
                return false;

            using var db = Open();
            int? isLocked = db.QueryFirstOrDefault<int?>(@"
                SELECT IsLocked FROM WorkPlan
                WHERE FilialId = @FilialId AND PlanMonth = @Month AND PlanYear = @Year AND PlanDay = @Day",
                new { FilialId = filialId, Month = month, Year = year, Day = day });

            return isLocked != 1;
        }

        // Планировать можно не дальше чем на 3 месяца вперёд
        public bool IsOverPlanningLimit(int month, int year)
        {
            DateTime maxDate = DateTime.Today.AddMonths(3);
            return year > maxDate.Year || (year == maxDate.Year && month > maxDate.Month);
        }
    }
}
