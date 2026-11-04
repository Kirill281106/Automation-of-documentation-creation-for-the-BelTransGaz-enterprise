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

                try
                {
                    // Таблица филиалов — ManagerId ссылается на Users.Id
                    ExecuteCommand(conn, @"CREATE TABLE IF NOT EXISTS Filials (
                        Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name      TEXT    NOT NULL UNIQUE,
                        Address   TEXT    NOT NULL,
                        Contact   TEXT    NOT NULL,
                        ManagerId INTEGER,
                        FOREIGN KEY (ManagerId) REFERENCES Users(Id)
                    );");

                    ExecuteCommand(conn, @"CREATE TABLE IF NOT EXISTS Users (
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

                    // Начальники филиалов — статус 5, активные
                    ExecuteCommand(conn, @"INSERT OR IGNORE INTO Users
                        (Id, Surname, Name, SecondName, Filial, TeamNum, Status, Login, Password, IsActive) VALUES
                        (1,  'Копец',       'Виталий',  'Анатольевич',  1, 0, 5, 'kopets',      'kopets',      1),
                        (2,  'Мазурчик',    'Денис',    'Анатольевич',  2, 0, 5, 'mazurchik',   'mazurchik',   1),
                        (3,  'Ермачёнок',   'Юрий',     'Владимирович', 3, 0, 5, 'ermachenok',  'ermachenok',  1),
                        (4,  'Лапин',       'Сергей',   'Михайлович',   4, 0, 5, 'lapin',       'lapin',       1),
                        (5,  'Кольков',     'Валерий',  'Валерьевич',   5, 0, 5, 'kolkov',      'kolkov',      1),
                        (6,  'Беляков',     'Владимир', 'Евгеньевич',   6, 0, 5, 'belyakov',    'belyakov',    1),
                        (7,  'Хлопонин',    'Юрий',     'Викторович',   7, 0, 5, 'hloponin',    'hloponin',    1),
                        (8,  'Можиловский', 'Сергей',   'Сергеевич',    8, 0, 5, 'mozhilovsky', 'mozhilovsky', 1);");

                    // Системные администраторы (Id 9-16 — защищённые)
                    ExecuteCommand(conn, @"INSERT OR IGNORE INTO Users
                        (Id, Name, Surname, SecondName, Filial, TeamNum, Status, Login, Password, IsActive) VALUES
                        (9,  'Минск',     'Минск',     'Минск',     1, 0, 6, 'Минск',     'Минск',     1),
                        (10, 'Орша',      'Орша',      'Орша',      2, 0, 6, 'Орша',      'Орша',      1),
                        (11, 'Крупки',    'Крупки',    'Крупки',    3, 0, 6, 'Крупки',    'Крупки',    1),
                        (12, 'Несвиж',    'Несвиж',    'Несвиж',    4, 0, 6, 'Несвиж',    'Несвиж',    1),
                        (13, 'Кобрин',    'Кобрин',    'Кобрин',    5, 0, 6, 'Кобрин',    'Кобрин',    1),
                        (14, 'Слоним',    'Слоним',    'Слоним',    6, 0, 6, 'Слоним',    'Слоним',    1),
                        (15, 'Гомель',    'Гомель',    'Гомель',    7, 0, 6, 'Гомель',    'Гомель',    1),
                        (16, 'Осиповичи', 'Осиповичи', 'Осиповичи', 8, 0, 6, 'Осиповичи', 'Осиповичи', 1);");

                    // Филиалы с ManagerId
                    ExecuteCommand(conn, @"INSERT OR IGNORE INTO Filials (Id, Name, Address, Contact, ManagerId) VALUES
                        (1, 'Минское УМГ',      'ул. Некрасова, д. 9, Минск',               '+375 17-500-34-62',  1),
                        (2, 'Оршанское УМГ',    'ул. Могилевская, д. 22, г. Орша',           '+375 216-29-02-62',  2),
                        (3, 'Крупское УМГ',     'ул. Знаменская, д. 6, Крупки',              '+375 1796-4-25-71',  3),
                        (4, 'Несвижское УМГ',   'ул. Вокзальная, д. 3, Несвиж',              '+375 1770-6-22-62',  4),
                        (5, 'Кобринское УМГ',   'ул. Интернациональная, д. 78, Кобрин',       '+375 1642-2-10-72',  5),
                        (6, 'Слонимское УМГ',   'ул. Промышленная, д. 13, Слоним',           '+375 1562-7-22-62',  6),
                        (7, 'Гомельское УМГ',   'ул. Советская, д. 109, Гомель',             '+375 232-49-02-62',  7),
                        (8, 'Осиповичское УМГ', 'ул. Сумченко, д. 2, Осиповичи',             '+375 2235-55-04-2',  8);");

                    ExecuteCommand(conn, @"CREATE TABLE IF NOT EXISTS Status_and_RoleName (
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
                        (6, 'Администратор');");

                    ExecuteCommand(conn, @"CREATE TABLE IF NOT EXISTS Requests (
                        Id            INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId        INTEGER NOT NULL,
                        RequestStatus INTEGER DEFAULT 0,
                        CreatedAt     DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
                    );");

                    ExecuteCommand(conn, @"CREATE TABLE IF NOT EXISTS WorkSchedule (
                        Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId    INTEGER NOT NULL,
                        WorkDate  TEXT    NOT NULL,
                        ShiftType TEXT    NOT NULL,
                        CreatedBy INTEGER NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (UserId)    REFERENCES Users(Id),
                        FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
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
