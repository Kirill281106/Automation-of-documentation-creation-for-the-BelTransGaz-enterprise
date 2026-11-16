using Dapper;

namespace вход_курсач
{
    // Сменные отчёты: создание, изменение, список, удаление.
    internal class DB_ShiftReport : DB_Base
    {
        public int GetNextReportNumber()
        {
            using var db = Open();
            return db.ExecuteScalar<int>("SELECT COALESCE(MAX(ReportNumber), 0) + 1 FROM ShiftReports");
        }

        // Сохраняет отчёт: новый (Id = 0) добавляется, существующий изменяется. Возвращает Id отчёта.
        public int SaveShiftReport(ShiftReportData report)
        {
            using var db = Open();

            if (report.Id == 0)
            {
                return db.ExecuteScalar<int>(@"
                    INSERT INTO ShiftReports
                        (ReportNumber, ReportDate, FilialId, ShiftType, ShiftManagerId,
                         NextManagerId, PlannedGas, ActualGas, Completion, TransferNote)
                    VALUES
                        (@ReportNumber, CAST(@ReportDate AS DATE), @FilialId, @ShiftType, @ShiftManagerId,
                         @NextManagerId, @PlannedGas, @ActualGas, @Completion, @TransferNote)
                    RETURNING Id", report);
            }

            db.Execute(@"
                UPDATE ShiftReports
                SET ReportNumber   = @ReportNumber,
                    ReportDate     = CAST(@ReportDate AS DATE),
                    FilialId       = @FilialId,
                    ShiftType      = @ShiftType,
                    ShiftManagerId = @ShiftManagerId,
                    NextManagerId  = @NextManagerId,
                    PlannedGas     = @PlannedGas,
                    ActualGas      = @ActualGas,
                    Completion     = @Completion,
                    TransferNote   = @TransferNote
                WHERE Id = @Id", report);

            return report.Id;
        }

        // Все сменные отчёты филиала (для таблицы истории)
        public List<ShiftReportListItem> GetShiftReports(int filialId)
        {
            using var db = Open();
            return db.Query<ShiftReportListItem>(@"
                SELECT
                    sr.Id,
                    sr.ReportNumber,
                    to_char(sr.ReportDate, 'YYYY-MM-DD') AS ReportDate,
                    sr.ShiftType,
                    sm.Surname || ' ' || sm.Name AS ManagerName,
                    sr.PlannedGas,
                    sr.ActualGas,
                    sr.Completion
                FROM ShiftReports sr
                INNER JOIN Users sm ON sm.Id = sr.ShiftManagerId
                WHERE sr.FilialId = @FilialId
                ORDER BY sr.ReportNumber DESC",
                new { FilialId = filialId }).ToList();
        }

        public ShiftReportData? GetShiftReportById(int id)
        {
            using var db = Open();
            return db.QueryFirstOrDefault<ShiftReportData>(@"
                SELECT
                    Id, ReportNumber,
                    to_char(ReportDate, 'YYYY-MM-DD') AS ReportDate,
                    FilialId, ShiftType, ShiftManagerId, NextManagerId,
                    PlannedGas, ActualGas, Completion, TransferNote
                FROM ShiftReports
                WHERE Id = @Id",
                new { Id = id });
        }

        public bool DeleteShiftReport(int id)
        {
            using var db = Open();
            return db.Execute("DELETE FROM ShiftReports WHERE Id = @Id", new { Id = id }) > 0;
        }
    }
}
