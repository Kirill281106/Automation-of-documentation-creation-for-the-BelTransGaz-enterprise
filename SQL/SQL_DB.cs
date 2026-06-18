using Microsoft.Data.Sqlite;

namespace вход_курсач
{
    internal class SQL_DB
    {
        public void SQLite_DB()
        {
            using (var conn = new SqliteConnection(Program.ConnectionString))
            {
                conn.Open();
                ExecuteCommand(conn, "PRAGMA foreign_keys = OFF;");

                ExecuteCommand(conn, "DROP TABLE IF EXISTS QuarterlyReports;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS ShiftReports;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS WorkPlan;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS WorkSchedule;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS Requests;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS Status_and_RoleName;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS Users;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS Filials;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS YearlyReports;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS TelegramUsers;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS BotNotifications;");
                ExecuteCommand(conn, "DROP TABLE IF EXISTS ApprovalNotified;");

                ExecuteCommand(conn, "PRAGMA foreign_keys = ON;");
                try
                {
                    ExecuteCommand(conn, @"CREATE TABLE Filials (
                        Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name      TEXT    NOT NULL UNIQUE,
                        Address   TEXT    NOT NULL,
                        Contact   TEXT    NOT NULL,
                        ManagerId INTEGER, 
                        FOREIGN KEY (ManagerId) REFERENCES Users(Id)
                    );");

                    ExecuteCommand(conn, @"CREATE TABLE Users (
                        Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name       TEXT    NOT NULL,
                        Surname    TEXT    NOT NULL,
                        SecondName TEXT,
                        Filial     INTEGER NOT NULL,
                        TeamNum    INTEGER,
                        Status     INTEGER NOT NULL,
                        Login      TEXT    UNIQUE NOT NULL,
                        Password   TEXT    NOT NULL,
                        IsActive   INTEGER DEFAULT 0,
                        CreatedAt  DATETIME DEFAULT CURRENT_TIMESTAMP
                    );");

                    ExecuteCommand(conn, @"INSERT OR IGNORE INTO Users
                        (Id, Name, Surname, SecondName, Filial, TeamNum, Status, Login, Password, IsActive) VALUES
                        (1,  'Копец',       'Виталий',  'Анатольевич',  1, 0, 5, 'kopets',      'kopets',      1),
                        (2,  'Мазурчик',    'Денис',    'Анатольевич',  2, 0, 5, 'mazurchik',   'mazurchik',   1),
                        (3,  'Ермачёнок',   'Юрий',     'Владимирович', 3, 0, 5, 'ermachenok',  'ermachenok',  1),
                        (4,  'Лапин',       'Сергей',   'Михайлович',   4, 0, 5, 'lapin',       'lapin',       1),
                        (5,  'Кольков',     'Валерий',  'Валерьевич',   5, 0, 5, 'kolkov',      'kolkov',      1),
                        (6,  'Беляков',     'Владимир', 'Евгеньевич',   6, 0, 5, 'belyakov',    'belyakov',    1),
                        (7,  'Хлопонин',    'Юрий',     'Викторович',   7, 0, 5, 'hloponin',    'hloponin',    1),
                        (8,  'Можиловский', 'Сергей',   'Сергеевич',    8, 0, 5, 'mozhilovsky', 'mozhilovsky', 1),
                        (9,  'Ген',         'Ген',      'Ген',          8, 0, 7, 'Ген',         'Ген',         1),
                        (10, 'Минск',       'Минск',    'Минск',        1, 0, 6, 'Минск',       'Минск',       1),
                        (11, 'Орша',        'Орша',     'Орша',         2, 0, 6, 'Орша',        'Орша',        1),
                        (12, 'Крупки',      'Крупки',   'Крупки',       3, 0, 6, 'Крупки',      'Крупки',      1),
                        (13, 'Несвиж',      'Несвиж',   'Несвиж',       4, 0, 6, 'Несвиж',      'Несвиж',      1),
                        (14, 'Кобрин',      'Кобрин',   'Кобрин',       5, 0, 6, 'Кобрин',      'Кобрин',      1),
                        (15, 'Слоним',      'Слоним',   'Слоним',       6, 0, 6, 'Слоним',      'Слоним',      1),
                        (16, 'Гомель',      'Гомель',   'Гомель',       7, 0, 6, 'Гомель',      'Гомель',      1),
                        (17, 'Осиповичи',   'Осиповичи','Осиповичи',    8, 0, 6, 'Осиповичи',   'Осиповичи',   1),
                        (18, 'Александр',   'Петров',   'Иванович',     1, 1, 3, 'petrov_shift',    'pass1', 1),
                        (19, 'Дмитрий',     'Сидоров',  'Петрович',     1, 2, 3, 'sidorov_shift',   'pass2', 1),
                        (20, 'Игорь',       'Козлов',   'Сергеевич',    1, 1, 3, 'kozlov_shift',    'pass3', 1),
                        (21, 'Николай',     'Новиков',  'Андреевич',    1, 2, 3, 'novikov_shift',   'pass4', 1),
                        (22, 'Василий',     'Морозов',  'Дмитриевич',   1, 1, 3, 'morozov_shift',   'pass5', 1),
                        (23, 'Андрей',      'Волков',   'Николаевич',   1, 2, 3, 'volkov_shift',    'pass6', 1),
                        (24, 'Сергей',      'Лебедев',  'Викторович',   1, 1, 3, 'lebedev_shift',   'pass7', 1),
                        (25, 'Павел',       'Соколов',  'Алексеевич',   1, 2, 3, 'sokolov_shift',   'pass8', 1),
                        (26, 'Роман',       'Попов',    'Юрьевич',      1, 1, 3, 'popov_shift',     'pass9', 1),
                        (27, 'Максим',      'Зайцев',   'Романович',    1, 2, 3, 'zaitsev_shift',   'pass10', 1),
                        (28, '1',           '1',        '1',            1, 1, 4, '1',               '1',      1),
                        (29, '2',           '2',        '2',            1, 1, 2, '2',               '2',      1);");

                    ExecuteCommand(conn, @"INSERT OR IGNORE INTO Users
                        (Name, Surname, SecondName, Filial, TeamNum, Status, Login, Password, IsActive) VALUES
                        ('Елена', 'Бухгалтерова', 'Владимировна', 1, 0, 4, 'buh_minsk', 'buh123', 1),
                        ('Анна', 'Петрова', 'Ивановна', 1, 0, 2, 'hr_minsk', 'hr123', 1);");

                    ExecuteCommand(conn, @"INSERT OR IGNORE INTO Users
                        (Name, Surname, SecondName, Filial, TeamNum, Status, Login, Password, IsActive)
                        VALUES
                        ('Иван', 'Иванов', 'Иванович', 1, 1, 1, 'ivanov1', '123', 1),
                        ('Пётр', 'Петров', 'Петрович', 1, 2, 1, 'petrov1', '123', 1),
                        ('Сергей', 'Сергеев', 'Сергеевич', 1, 1, 1, 'sergeev1', '123', 1),
                        ('Андрей', 'Андреев', 'Андреевич', 1, 2, 1, 'andreev1', '123', 1);");

                    ExecuteCommand(conn, @"INSERT OR IGNORE INTO Filials (Id, Name, Address, Contact, ManagerId) VALUES
                        (1, 'Минское УМГ',      'ул. Некрасова, д. 9, Минск',               '+375 17-500-34-62',  1),
                        (2, 'Оршанское УМГ',    'ул. Могилевская, д. 22, г. Орша',           '+375 216-29-02-62',  2),
                        (3, 'Крупское УМГ',     'ул. Знаменская, д. 6, Крупки',              '+375 1796-4-25-71',  3),
                        (4, 'Несвижское УМГ',   'ул. Вокзальная, д. 3, Несвиж',              '+375 1770-6-22-62',  4),
                        (5, 'Кобринское УМГ',   'ул. Интернациональная, д. 78, Кобрин',       '+375 1642-2-10-72',  5),
                        (6, 'Слонимское УМГ',   'ул. Промышленная, д. 13, Слоним',           '+375 1562-7-22-62',  6),
                        (7, 'Гомельское УМГ',   'ул. Советская, д. 109, Гомель',             '+375 232-49-02-62',  7),
                        (8, 'Осиповичское УМГ', 'ул. Сумченко, д. 2, Осиповичи',             '+375 2235-55-04-2',  8);");

                    ExecuteCommand(conn, @"CREATE TABLE Status_and_RoleName (
                        Status   INTEGER PRIMARY KEY,
                        RoleName TEXT    NOT NULL
                    );");

                    ExecuteCommand(conn, @"INSERT OR IGNORE INTO Status_and_RoleName (Status, RoleName) VALUES
                        (0, 'Заявка (не подтверждён)'),
                        (1, 'Работник'),
                        (2, 'Отдел кадров'),
                        (3, 'Начальник смены'),
                        (4, 'Бухгалтер'),
                        (5, 'Нач. филиала / зам'),
                        (6, 'Администратор'),
                        (7, 'Ген. директор');");

                    ExecuteCommand(conn, @"CREATE TABLE Requests (
                        Id            INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId        INTEGER NOT NULL,
                        RequestStatus INTEGER DEFAULT 0,
                        CreatedAt     DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
                    );");

                    ExecuteCommand(conn, @"CREATE TABLE WorkSchedule (
                        Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId    INTEGER NOT NULL,
                        WorkDate  TEXT    NOT NULL,
                        ShiftType TEXT    NOT NULL,
                        CreatedBy INTEGER NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (UserId)    REFERENCES Users(Id),
                        FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
                    );");

                    ExecuteCommand(conn, @"CREATE TABLE ShiftReports (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ReportNumber INTEGER NOT NULL,
                        ReportDate TEXT NOT NULL,
                        FilialId INTEGER NOT NULL,
                        ShiftType TEXT NOT NULL,
                        ShiftManagerId INTEGER NOT NULL,
                        NextManagerId INTEGER,
                        PlannedGas REAL NOT NULL DEFAULT 0,
                        ActualGas REAL NOT NULL DEFAULT 0,
                        Completion REAL NOT NULL DEFAULT 0,
                        TransferNote TEXT NOT NULL DEFAULT '',
                        FOREIGN KEY (FilialId) REFERENCES Filials(Id),
                        FOREIGN KEY (ShiftManagerId) REFERENCES Users(Id),
                        FOREIGN KEY (NextManagerId) REFERENCES Users(Id)
                    );");

                    ExecuteCommand(conn, @"INSERT INTO ShiftReports
                        (Id, ReportNumber, ReportDate, FilialId, ShiftType, ShiftManagerId, NextManagerId, PlannedGas, ActualGas, Completion, TransferNote) VALUES
                        (1,  1,  '2026-01-01', 1, 'Дневная', 18, 19, 50000.0, 49500.0, 99.0,  'Замечаний нет'),
                        (2,  2,  '2026-01-01', 1, 'Ночная',  19, 20, 48000.0, 47200.0, 98.33, 'Плановое обслуживание'),
                        (3,  3,  '2026-01-02', 1, 'Дневная', 20, 21, 51000.0, 50800.0, 99.61, 'Замечаний нет'),
                        (4,  4,  '2026-01-02', 1, 'Ночная',  21, 22, 49000.0, 48100.0, 98.16, 'Замечаний нет'),
                        (5,  5,  '2026-01-03', 1, 'Дневная', 22, 23, 52000.0, 51500.0, 99.04, 'Замечаний нет'),
                        (6,  6,  '2026-01-03', 1, 'Ночная',  23, 24, 50500.0, 49900.0, 98.81, 'Замечаний нет'),
                        (7,  7,  '2026-01-04', 1, 'Дневная', 24, 25, 53000.0, 52400.0, 98.87, 'Замечаний нет'),
                        (8,  8,  '2026-01-04', 1, 'Ночная',  25, 26, 51500.0, 50800.0, 98.64, 'Замечаний нет'),
                        (9,  9,  '2026-01-05', 1, 'Дневная', 26, 27, 54000.0, 53200.0, 98.52, 'Замечаний нет'),
                        (10, 10, '2026-01-05', 1, 'Ночная',  27, 18, 52000.0, 51600.0, 99.23, 'Замечаний нет');");

                    ExecuteCommand(conn, @"INSERT INTO ShiftReports
                        (ReportNumber, ReportDate, FilialId, ShiftType, ShiftManagerId, NextManagerId, PlannedGas, ActualGas, Completion, TransferNote)
                    VALUES
                        (101, '2025-01-15', 1, 'Дневная', 18, 19, 55000, 54200, 98.55, 'Начало года'),
                        (102, '2025-01-15', 1, 'Ночная', 19, 20, 53000, 52500, 99.06, 'Без замечаний'),
                        (103, '2025-02-01', 1, 'Дневная', 20, 21, 56000, 55100, 98.39, 'План выполнен'),
                        (104, '2025-02-15', 1, 'Ночная', 21, 22, 54000, 53800, 99.63, 'Отлично'),
                        (105, '2025-03-01', 1, 'Дневная', 22, 23, 58000, 57500, 99.14, 'Замечаний нет'),
                        (106, '2025-03-15', 1, 'Ночная', 23, 24, 56000, 55500, 99.11, 'Штатно'),
                        (107, '2025-04-01', 1, 'Дневная', 24, 25, 59000, 58200, 98.64, '2 квартал'),
                        (108, '2025-04-15', 1, 'Ночная', 25, 26, 57000, 56800, 99.65, 'Без отклонений'),
                        (109, '2025-05-01', 1, 'Дневная', 26, 27, 60000, 59300, 98.83, 'Работа стабильная'),
                        (110, '2025-05-15', 1, 'Ночная', 27, 18, 58000, 57700, 99.48, 'Хорошо'),
                        (111, '2025-06-01', 1, 'Дневная', 18, 19, 61000, 60500, 99.18, '3 квартал'),
                        (112, '2025-06-15', 1, 'Ночная', 19, 20, 59000, 58800, 99.66, 'Отлично'),
                        (113, '2025-07-01', 1, 'Дневная', 20, 21, 62000, 61200, 98.71, 'Почти план'),
                        (114, '2025-07-15', 1, 'Ночная', 21, 22, 60000, 59800, 99.67, 'Хорошо'),
                        (115, '2025-08-01', 1, 'Дневная', 22, 23, 63000, 62500, 99.21, 'Замечаний нет'),
                        (116, '2025-08-15', 1, 'Ночная', 23, 24, 61000, 60800, 99.67, 'Отлично'),
                        (117, '2025-09-01', 1, 'Дневная', 24, 25, 64000, 63500, 99.22, '4 квартал'),
                        (118, '2025-09-15', 1, 'Ночная', 25, 26, 62000, 61800, 99.68, 'Хорошо'),
                        (119, '2025-10-01', 1, 'Дневная', 26, 27, 65000, 64200, 98.77, 'Работа в штатном режиме'),
                        (120, '2025-10-15', 1, 'Ночная', 27, 18, 63000, 62800, 99.68, 'Без замечаний'),
                        (121, '2025-11-01', 1, 'Дневная', 18, 19, 66000, 65500, 99.24, 'План выполнен'),
                        (122, '2025-11-15', 1, 'Ночная', 19, 20, 64000, 63800, 99.69, 'Отлично'),
                        (123, '2025-12-01', 1, 'Дневная', 20, 21, 67000, 66500, 99.25, 'Конец года'),
                        (124, '2025-12-15', 1, 'Ночная', 21, 22, 65000, 64800, 99.69, 'Смена передана успешно');");

                    ExecuteCommand(conn, @"CREATE TABLE WorkPlan (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FilialId INTEGER NOT NULL,
                        PlanMonth INTEGER NOT NULL,
                        PlanYear INTEGER NOT NULL,
                        PlanDay INTEGER NOT NULL,
                        PlannedVolume REAL NOT NULL,
                        CreatedBy INTEGER NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        IsLocked INTEGER DEFAULT 0,
                        UNIQUE(FilialId, PlanYear, PlanMonth, PlanDay),
                        FOREIGN KEY (FilialId) REFERENCES Filials(Id),
                        FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
                    );");

                    ExecuteCommand(conn, @"CREATE TABLE QuarterlyReports (
                        Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                        Quarter         INTEGER NOT NULL,
                        Year            INTEGER NOT NULL,
                        FilialId        INTEGER NOT NULL,
                        CompiledById    INTEGER NOT NULL,
                        AddressedToId   INTEGER,
                        CreatedAt       TEXT    NOT NULL,
                        PlannedGas      REAL    NOT NULL DEFAULT 0,
                        ActualGas       REAL    NOT NULL DEFAULT 0,
                        Completion      REAL    NOT NULL DEFAULT 0,
                        PurchasePrice   REAL    NOT NULL DEFAULT 0,
                        SalePrice       REAL    NOT NULL DEFAULT 0,
                        Revenue         REAL    NOT NULL DEFAULT 0,
                        Cost            REAL    NOT NULL DEFAULT 0,
                        Profit          REAL    NOT NULL DEFAULT 0,
                        UNIQUE(Quarter, Year, FilialId),
                        FOREIGN KEY (FilialId)      REFERENCES Filials(Id),
                        FOREIGN KEY (CompiledById)  REFERENCES Users(Id),
                        FOREIGN KEY (AddressedToId) REFERENCES Users(Id)
                    );");

                    ExecuteCommand(conn, @"INSERT INTO QuarterlyReports 
                        (Quarter, Year, FilialId, CompiledById, AddressedToId, CreatedAt,
                         PlannedGas, ActualGas, Completion,
                         PurchasePrice, SalePrice, Revenue, Cost, Profit)
                        VALUES
                        (1, 2025, 1, 28, 1, '2025-04-05',
                         167000, 165000, 98.80,
                         1200, 1550, 255750, 198000, 57750),

                        (2, 2025, 1, 28, 1, '2025-07-05',
                         177000, 175000, 98.87,
                         1230, 1580, 276500, 215250, 61250),

                        (3, 2025, 1, 28, 1, '2025-10-05',
                         187000, 185000, 98.93,
                         1250, 1600, 296000, 231250, 64750),

                        (4, 2025, 1, 28, 1, '2026-01-05',
                         197000, 195000, 98.98,
                         1280, 1630, 317850, 249600, 68250);");

                    ExecuteCommand(conn, @"CREATE TABLE YearlyReports (
                        Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                        Year            INTEGER NOT NULL,
                        FilialId        INTEGER NOT NULL,
                        CompiledById    INTEGER NOT NULL,
                        AddressedToId   INTEGER,
                        CreatedAt       TEXT    NOT NULL,
                        PlannedGas      REAL    NOT NULL DEFAULT 0,
                        ActualGas       REAL    NOT NULL DEFAULT 0,
                        Completion      REAL    NOT NULL DEFAULT 0,
                        PurchasePrice   REAL    NOT NULL DEFAULT 0,
                        SalePrice       REAL    NOT NULL DEFAULT 0,
                        Revenue         REAL    NOT NULL DEFAULT 0,
                        Cost            REAL    NOT NULL DEFAULT 0,
                        Profit          REAL    NOT NULL DEFAULT 0,
                        UNIQUE(Year, FilialId),
                        FOREIGN KEY (FilialId)      REFERENCES Filials(Id),
                        FOREIGN KEY (CompiledById)  REFERENCES Users(Id),
                        FOREIGN KEY (AddressedToId) REFERENCES Users(Id)
                    );");

                    ExecuteCommand(conn, @"INSERT OR IGNORE INTO YearlyReports 
                                (Year, FilialId, CompiledById, AddressedToId, CreatedAt,
                                 PlannedGas, ActualGas, Completion, 
                                 PurchasePrice, SalePrice, Revenue, Cost, Profit)
                            VALUES
                                (2025, 1, 28, 1, '2026-01-20',
                                 728000, 720000, 98.90,
                                 1240, 1590, 1144800, 892800, 252000);");

                    ExecuteCommand(conn, @"CREATE TABLE IF NOT EXISTS TelegramUsers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Surname TEXT NOT NULL,
                        Name TEXT NOT NULL,
                        SecondName TEXT,
                        UserId INTEGER,
                        ChatId INTEGER UNIQUE,
                        CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
                    );");

                    ExecuteCommand(conn, @"CREATE TABLE IF NOT EXISTS BotNotifications (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Type TEXT NOT NULL,
                        Data TEXT NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        IsSent INTEGER DEFAULT 0
                    );");

                    ExecuteCommand(conn, @"CREATE TABLE IF NOT EXISTS ApprovalNotified (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        RequestId INTEGER NOT NULL,
                        NotifiedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании БД: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private static void ExecuteCommand(SqliteConnection conn, string commandText)
        {
            using (var cmd = new SqliteCommand(commandText, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}