using Dapper;
using Npgsql;

namespace вход_курсач
{
    // Создание базы данных и её структуры (таблиц).
    // Заполнение начальными данными — в классе DB_Fill.
    internal class DB_Create : DB_Base
    {
        // Создаёт саму базу данных на сервере PostgreSQL, если её ещё нет
        public void CreateDatabase()
        {
            var settings = new NpgsqlConnectionStringBuilder(Program.ConnectionString);
            string databaseName = settings.Database!;

            // Создать базу можно только подключившись к другой, служебной базе postgres
            settings.Database = "postgres";
            using var server = new NpgsqlConnection(settings.ConnectionString);
            server.Open();

            bool exists = server.ExecuteScalar<bool>(
                "SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = @Name)",
                new { Name = databaseName });

            if (!exists)
                server.Execute($"CREATE DATABASE \"{databaseName.Replace("\"", "\"\"")}\"");
        }

        public bool TablesExist()
        {
            using var db = Open();
            return db.ExecuteScalar<bool>("SELECT to_regclass('public.users') IS NOT NULL");
        }

        // Создаёт все таблицы. Выполняется в одной транзакции:
        // либо создаётся вся структура, либо ничего.
        public void CreateTables()
        {
            using var db = Open();
            using var transaction = db.BeginTransaction();

            // Справочник ролей (статусов) пользователей
            db.Execute(@"
                CREATE TABLE Status_and_RoleName (
                    Status   INTEGER PRIMARY KEY,
                    RoleName TEXT    NOT NULL
                )");

            // Филиалы
            db.Execute(@"
                CREATE TABLE Filials (
                    Id        SERIAL  PRIMARY KEY,
                    Name      TEXT    NOT NULL UNIQUE,
                    Address   TEXT    NOT NULL,
                    Contact   TEXT    NOT NULL,
                    ManagerId INTEGER
                )");

            // Пользователи
            db.Execute(@"
                CREATE TABLE Users (
                    Id         SERIAL    PRIMARY KEY,
                    Name       TEXT      NOT NULL,
                    Surname    TEXT      NOT NULL,
                    SecondName TEXT,
                    Filial     INTEGER   NOT NULL REFERENCES Filials(Id),
                    TeamNum    INTEGER,
                    Status     INTEGER   NOT NULL REFERENCES Status_and_RoleName(Status),
                    Login      TEXT      NOT NULL UNIQUE,
                    Password   TEXT      NOT NULL,
                    IsActive   INTEGER   DEFAULT 0,
                    CreatedAt  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                )");

            // Начальник филиала — это пользователь. Связь добавляется отдельно,
            // потому что таблица Users создаётся после таблицы Filials.
            db.Execute(@"
                ALTER TABLE Filials
                ADD CONSTRAINT fk_filials_manager FOREIGN KEY (ManagerId) REFERENCES Users(Id)");

            // Заявки на регистрацию
            db.Execute(@"
                CREATE TABLE Requests (
                    Id            SERIAL    PRIMARY KEY,
                    UserId        INTEGER   NOT NULL REFERENCES Users(Id),
                    RequestStatus INTEGER   DEFAULT 0,
                    CreatedAt     TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                )");

            // График работы
            db.Execute(@"
                CREATE TABLE WorkSchedule (
                    Id        SERIAL    PRIMARY KEY,
                    UserId    INTEGER   NOT NULL REFERENCES Users(Id),
                    WorkDate  DATE      NOT NULL,
                    ShiftType TEXT      NOT NULL,
                    CreatedBy INTEGER   NOT NULL REFERENCES Users(Id),
                    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                )");

            // Сменные отчёты
            db.Execute(@"
                CREATE TABLE ShiftReports (
                    Id             SERIAL  PRIMARY KEY,
                    ReportNumber   INTEGER NOT NULL,
                    ReportDate     DATE    NOT NULL,
                    FilialId       INTEGER NOT NULL REFERENCES Filials(Id),
                    ShiftType      TEXT    NOT NULL,
                    ShiftManagerId INTEGER NOT NULL REFERENCES Users(Id),
                    NextManagerId  INTEGER REFERENCES Users(Id),
                    PlannedGas     DOUBLE PRECISION NOT NULL DEFAULT 0,
                    ActualGas      DOUBLE PRECISION NOT NULL DEFAULT 0,
                    Completion     DOUBLE PRECISION NOT NULL DEFAULT 0,
                    TransferNote   TEXT    NOT NULL DEFAULT ''
                )");

            // План поставки газа по дням
            db.Execute(@"
                CREATE TABLE WorkPlan (
                    Id            SERIAL    PRIMARY KEY,
                    FilialId      INTEGER   NOT NULL REFERENCES Filials(Id),
                    PlanMonth     INTEGER   NOT NULL,
                    PlanYear      INTEGER   NOT NULL,
                    PlanDay       INTEGER   NOT NULL,
                    PlannedVolume DOUBLE PRECISION NOT NULL,
                    CreatedBy     INTEGER   NOT NULL REFERENCES Users(Id),
                    CreatedAt     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    IsLocked      INTEGER   DEFAULT 0,
                    UNIQUE (FilialId, PlanYear, PlanMonth, PlanDay)
                )");

            // Квартальные отчёты
            db.Execute(@"
                CREATE TABLE QuarterlyReports (
                    Id            SERIAL  PRIMARY KEY,
                    Quarter       INTEGER NOT NULL,
                    Year          INTEGER NOT NULL,
                    FilialId      INTEGER NOT NULL REFERENCES Filials(Id),
                    CompiledById  INTEGER NOT NULL REFERENCES Users(Id),
                    AddressedToId INTEGER REFERENCES Users(Id),
                    CreatedAt     DATE    NOT NULL,
                    PlannedGas    DOUBLE PRECISION NOT NULL DEFAULT 0,
                    ActualGas     DOUBLE PRECISION NOT NULL DEFAULT 0,
                    Completion    DOUBLE PRECISION NOT NULL DEFAULT 0,
                    PurchasePrice DOUBLE PRECISION NOT NULL DEFAULT 0,
                    SalePrice     DOUBLE PRECISION NOT NULL DEFAULT 0,
                    Revenue       DOUBLE PRECISION NOT NULL DEFAULT 0,
                    Cost          DOUBLE PRECISION NOT NULL DEFAULT 0,
                    Profit        DOUBLE PRECISION NOT NULL DEFAULT 0,
                    UNIQUE (Quarter, Year, FilialId)
                )");

            // Годовые отчёты
            db.Execute(@"
                CREATE TABLE YearlyReports (
                    Id            SERIAL  PRIMARY KEY,
                    Year          INTEGER NOT NULL,
                    FilialId      INTEGER NOT NULL REFERENCES Filials(Id),
                    CompiledById  INTEGER NOT NULL REFERENCES Users(Id),
                    AddressedToId INTEGER REFERENCES Users(Id),
                    CreatedAt     DATE    NOT NULL,
                    PlannedGas    DOUBLE PRECISION NOT NULL DEFAULT 0,
                    ActualGas     DOUBLE PRECISION NOT NULL DEFAULT 0,
                    Completion    DOUBLE PRECISION NOT NULL DEFAULT 0,
                    PurchasePrice DOUBLE PRECISION NOT NULL DEFAULT 0,
                    SalePrice     DOUBLE PRECISION NOT NULL DEFAULT 0,
                    Revenue       DOUBLE PRECISION NOT NULL DEFAULT 0,
                    Cost          DOUBLE PRECISION NOT NULL DEFAULT 0,
                    Profit        DOUBLE PRECISION NOT NULL DEFAULT 0,
                    UNIQUE (Year, FilialId)
                )");

            transaction.Commit();
        }
    }
}
