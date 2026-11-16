using Dapper;

namespace вход_курсач
{
    // График работы: просмотр, добавление и удаление записей.
    internal class DB_WorkSchedule : DB_Base
    {
        // Общая часть запросов графика: запись графика + сотрудник + его филиал
        private const string ScheduleSelectSql = @"
            SELECT
                ws.Id,
                ws.UserId,
                (u.Surname || ' ' || substr(u.Name, 1, 1) || '.' ||
                 CASE WHEN u.SecondName IS NOT NULL AND u.SecondName <> ''
                      THEN substr(u.SecondName, 1, 1) || '.' ELSE '' END) AS UserFullName,
                u.TeamNum,
                to_char(ws.WorkDate, 'YYYY-MM-DD') AS WorkDate,
                ws.ShiftType,
                f.Id   AS FilialId,
                f.Name AS FilialName
            FROM WorkSchedule ws
            INNER JOIN Users   u ON u.Id = ws.UserId
            INNER JOIN Filials f ON f.Id = u.Filial";

        // График филиала текущего пользователя. filterMonth — месяц в виде "2026-03" или null (все месяцы).
        // Отдел кадров, начальник смены и администратор видят весь филиал, остальные — только свои смены.
        public List<WorkScheduleItem> GetSchedule(int currentUserId, string? filterMonth = null)
        {
            using var db = Open();
            var currentUser = FindUser(db, currentUserId);
            if (currentUser == null)
                return new List<WorkScheduleItem>();

            bool canViewAll = currentUser.Status == Roles.Hr ||
                              currentUser.Status == Roles.ShiftManager ||
                              currentUser.Status == Roles.Admin;

            string monthFilter = string.IsNullOrWhiteSpace(filterMonth)
                ? ""
                : "AND to_char(ws.WorkDate, 'YYYY-MM') = @Month";
            string userFilter = canViewAll ? "" : "AND ws.UserId = @CurrentUserId";

            string sql = $@"
                {ScheduleSelectSql}
                WHERE u.Filial = @Filial AND u.IsActive = 1
                {monthFilter}
                {userFilter}
                ORDER BY ws.WorkDate, ws.ShiftType, u.Surname";

            return db.Query<WorkScheduleItem>(sql,
                new { Filial = currentUser.Filial, Month = filterMonth, CurrentUserId = currentUserId }).ToList();
        }

        // Личный график одного сотрудника
        public List<WorkScheduleItem> GetUserWorkSchedule(int userId)
        {
            using var db = Open();
            string sql = $@"
                {ScheduleSelectSql}
                WHERE ws.UserId = @UserId AND u.IsActive = 1
                ORDER BY ws.WorkDate";

            return db.Query<WorkScheduleItem>(sql, new { UserId = userId }).ToList();
        }

        public OperationResult AddScheduleEntry(int currentUserId, int targetUserId,
            DateTime workDate, string shiftType)
        {
            using var db = Open();

            var currentUser = FindUser(db, currentUserId);
            if (currentUser == null || currentUser.Status < Roles.Manager)
                return OperationResult.Fail("Составлять график могут только начальник филиала, его заместители и администратор.");

            var target = FindUser(db, targetUserId);
            if (target == null || target.Filial != currentUser.Filial)
                return OperationResult.Fail("Сотрудник не найден или принадлежит другому филиалу.");

            if (shiftType != "Дневная" && shiftType != "Ночная")
                return OperationResult.Fail("Тип смены должен быть «Дневная» или «Ночная».");

            long exists = db.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM WorkSchedule WHERE UserId = @UserId AND WorkDate = @Date",
                new { UserId = targetUserId, Date = workDate.Date });
            if (exists > 0)
                return OperationResult.Fail("На эту дату у сотрудника уже есть запись в графике.");

            db.Execute(@"
                INSERT INTO WorkSchedule (UserId, WorkDate, ShiftType, CreatedBy)
                VALUES (@UserId, @WorkDate, @ShiftType, @CreatedBy)",
                new
                {
                    UserId = targetUserId,
                    WorkDate = workDate.Date,
                    ShiftType = shiftType,
                    CreatedBy = currentUserId
                });

            return OperationResult.Ok();
        }

        public OperationResult DeleteScheduleEntry(int currentUserId, int scheduleId)
        {
            using var db = Open();

            var currentUser = FindUser(db, currentUserId);
            if (currentUser == null || currentUser.Status < Roles.Manager)
                return OperationResult.Fail("Недостаточно прав для удаления записей графика.");

            int rows = db.Execute("DELETE FROM WorkSchedule WHERE Id = @Id", new { Id = scheduleId });
            return rows > 0 ? OperationResult.Ok() : OperationResult.Fail("Запись не найдена.");
        }
    }
}
