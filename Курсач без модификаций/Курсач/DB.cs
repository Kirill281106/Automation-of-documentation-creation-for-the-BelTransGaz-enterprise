using Dapper;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;
namespace вход_курсач
{
    internal class DB
    {
        private const int HrStatus = 2;
        private const int ManagerStatus = 5;
        private const int AdminStatus = 6;
        private const int ProtectedAdminCount = 16;

        private readonly string _connection;

        public DB(string connection)
        {
            _connection = connection;
        }

        // Возвращает список Id+Name филиалов (для ComboBox)
        public IEnumerable<dynamic> GetFilials()
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                return db.Query("SELECT Id, Name FROM Filials ORDER BY Name").ToList();
            }
        }

        // Возвращает все филиалы с ФИО начальника из таблицы Users (без Id-шников)
        public List<FilialDisplayItem> GetFilialsForDisplay()
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string sql = @"
                    SELECT
                        f.Name    AS FilialName,
                        f.Address,
                        f.Contact,
                        COALESCE(
                            u.Surname || ' ' || u.Name ||
                            CASE WHEN u.SecondName IS NOT NULL AND u.SecondName != ''
                                 THEN ' ' || u.SecondName ELSE '' END,
                            '—'
                        ) AS ManagerFullName
                    FROM Filials f
                    LEFT JOIN Users u ON u.Id = f.ManagerId
                    ORDER BY f.Name";
                return db.Query<FilialDisplayItem>(sql).ToList();
            }
        }

        // Возвращает все активные сотрудники для выбора начальника (ComboBox)
        public List<EmployeeItem> GetActiveEmployees()
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
                    WHERE u.IsActive = 1
                    ORDER BY u.Surname, u.Name";
                return db.Query<EmployeeItem>(sql).ToList();
            }
        }

        // Добавляет новый филиал
        public OperationResult AddFilial(string name, string address, string contact, int? managerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return OperationResult.Fail("Введите название филиала.");
            if (string.IsNullOrWhiteSpace(address))
                return OperationResult.Fail("Введите адрес филиала.");
            if (string.IsNullOrWhiteSpace(contact))
                return OperationResult.Fail("Введите контактный телефон.");

            using (IDbConnection db = new SqliteConnection(_connection))
            {
                long exists = db.ExecuteScalar<long>(
                    "SELECT COUNT(1) FROM Filials WHERE Name = @Name", new { Name = name.Trim() });
                if (exists > 0)
                    return OperationResult.Fail("Филиал с таким названием уже существует.");

                db.Execute(@"
                    INSERT INTO Filials (Name, Address, Contact, ManagerId)
                    VALUES (@Name, @Address, @Contact, @ManagerId)",
                    new
                    {
                        Name = name.Trim(),
                        Address = address.Trim(),
                        Contact = contact.Trim(),
                        ManagerId = managerId.HasValue ? (object)managerId.Value : DBNull.Value
                    });
                return OperationResult.Ok();
            }
        }

        // Возвращает данные одного филиала по названию (для заполнения формы редактирования)
        public FilialEditItem? GetFilialByName(string filialName)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                return db.QueryFirstOrDefault<FilialEditItem>(
                    "SELECT Id, Name, Address, Contact, ManagerId FROM Filials WHERE Name = @Name",
                    new { Name = filialName });
            }
        }

        // Изменяет существующий филиал
        public OperationResult UpdateFilial(int filialId, string name, string address, string contact, int? managerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return OperationResult.Fail("Введите название филиала.");
            if (string.IsNullOrWhiteSpace(address))
                return OperationResult.Fail("Введите адрес филиала.");
            if (string.IsNullOrWhiteSpace(contact))
                return OperationResult.Fail("Введите контактный телефон.");

            using (IDbConnection db = new SqliteConnection(_connection))
            {
                long exists = db.ExecuteScalar<long>(
                    "SELECT COUNT(1) FROM Filials WHERE Name = @Name AND Id <> @Id",
                    new { Name = name.Trim(), Id = filialId });
                if (exists > 0)
                    return OperationResult.Fail("Филиал с таким названием уже существует.");

                db.Execute(@"
                    UPDATE Filials
                    SET Name = @Name, Address = @Address, Contact = @Contact, ManagerId = @ManagerId
                    WHERE Id = @Id",
                    new
                    {
                        Id = filialId,
                        Name = name.Trim(),
                        Address = address.Trim(),
                        Contact = contact.Trim(),
                        ManagerId = managerId.HasValue ? (object)managerId.Value : DBNull.Value
                    });
                return OperationResult.Ok();
            }
        }

        // Удаляет филиал
        public OperationResult DeleteFilial(int filialId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                long usersInFilial = db.ExecuteScalar<long>(
                    "SELECT COUNT(1) FROM Users WHERE Filial = @Filial", new { Filial = filialId });
                if (usersInFilial > 0)
                    return OperationResult.Fail("Нельзя удалить филиал, к которому привязаны сотрудники.");

                int rows = db.Execute("DELETE FROM Filials WHERE Id = @Id", new { Id = filialId });
                return rows > 0 ? OperationResult.Ok() : OperationResult.Fail("Филиал не найден.");
            }
        }

        public ClassUser Authenticate(string login, string password)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string sql = @"SELECT * FROM Users
                               WHERE Login = @Login AND Password = @Password";

                return db.QueryFirstOrDefault<ClassUser>(sql, new
                {
                    Login = login,
                    Password = password
                });
            }
        }
        public ClassUser CreateApplication(string name, string surname, string secondName, string login, string password, string Filial)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string existsSql = @"SELECT * FROM Users WHERE Login = @Login";
                var existing = db.QueryFirstOrDefault<ClassUser>(existsSql, new { Login = login });
                if (existing != null)
                {
                    return null;
                }

                // Пользователь вводит название филиала, а в Users числовой Id филиала
                // Попытаемся найти Id по названию в Filials (Name -> Id). Если нет — 0
                int filialInt = 0;
                if (!string.IsNullOrWhiteSpace(Filial))
                {
                    var filialId = db.QueryFirstOrDefault<int?>("SELECT Id FROM Filials WHERE Name = @Name", new { Name = Filial });
                    if (filialId.HasValue)
                        filialInt = filialId.Value;
                }

                string insertSql = @"INSERT INTO Users (Login, Password, Name, Surname, SecondName, Filial, Status)
                                     VALUES (@Login, @Password, @Name, @Surname, @SecondName, @Filial, @Status);";

                db.Execute(insertSql, new
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    SecondName = secondName,
                    Filial = filialInt,
                    Status = 0
                });

                // Получаем Id только что созданной записи и возвращаем её  
                var idObj = db.ExecuteScalar<long>("SELECT last_insert_rowid();");
                int newId = (int)idObj;

                string selectSql = "SELECT * FROM Users WHERE Id = @Id";
                var createdUser = db.QueryFirstOrDefault<ClassUser>(selectSql, new { Id = newId });

                // новая заявка
                db.Execute("INSERT INTO Requests (UserId, RequestStatus) VALUES (@UserId, @Status)", new { UserId = newId, Status = 0 });
                return createdUser;
            }
        }
        public List<ClassUser> ListApplication(int currentUserId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string getUserFilial = "SELECT Filial FROM Users WHERE Id = @UserId";
                int userFilial = db.ExecuteScalar<int>(getUserFilial, new { UserId = currentUserId });

                string listApp = @"
            SELECT 
                u.Id,
                u.Name,
                u.Surname,
                u.SecondName,
                u.Login,
                u.Password,
                u.Filial,
                u.TeamNum,
                u.Status,
                u.IsActive,
                f.Name as FilialName
            FROM Users u
            INNER JOIN Filials f ON u.Filial = f.Id
            WHERE u.Filial = @Filial AND u.IsActive = 0
            ORDER BY u.CreatedAt DESC";

                return db.Query<ClassUser>(listApp, new { Filial = userFilial }).ToList();
            }
        }
        public bool ApproveRequest(int userId, int newStatus, int processedBy)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                string sql = @"
            UPDATE Users 
            SET IsActive = 1, Status = @Status 
            WHERE Id = @Id";

                return db.Execute(sql, new { Status = newStatus, Id = userId }) > 0;
            }
        }
        public bool DeleteRequest(int userId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                db.Execute("DELETE FROM Requests WHERE UserId = @UserId", new { UserId = userId });

                int rows = db.Execute("DELETE FROM Users WHERE Id = @UserId AND IsActive = 0", new { UserId = userId });

                return rows > 0;
            }
        }

        public List<UserManagementItem> GetUsersForManagement(int currentUserId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                var currentUser = db.QueryFirstOrDefault<ClassUser>("SELECT * FROM Users WHERE Id = @Id", new { Id = currentUserId });
                if (currentUser == null || (currentUser.Status != HrStatus && currentUser.Status != AdminStatus))
                {
                    return new List<UserManagementItem>();
                }

                string whereClause = currentUser.Status == AdminStatus
                    ? "u.IsActive = 1 AND NOT (u.Status = 6 AND u.Id <= 16)"
                    : "u.IsActive = 1 AND u.Filial = @Filial AND NOT (u.Status = 6 AND u.Id <= 16)";


                string sql = $@"
                    SELECT
                        u.Id,
                        u.Login,
                        u.Name,
                        u.Surname,
                        u.SecondName,
                        u.Filial,
                        f.Name AS FilialName,
                        COALESCE(u.TeamNum, 0) AS TeamNum,
                        u.Status,
                        r.RoleName,
                        u.IsActive
                    FROM Users u
                    INNER JOIN Filials f ON f.Id = u.Filial
                    INNER JOIN Status_and_RoleName r ON r.Status = u.Status
                    WHERE {whereClause}
                    ORDER BY u.Surname, u.Name, u.Id";

                var users = db.Query<UserManagementItem>(sql, new { Filial = currentUser.Filial }).ToList();

                return users;
            }
        }

        public OperationResult AddUser(int currentUserId, string login, string password, string name, string surname, string secondName, int filial, int teamNum, int status)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                var currentUser = db.QueryFirstOrDefault<ClassUser>("SELECT * FROM Users WHERE Id = @Id", new { Id = currentUserId });
                if (currentUser == null || (currentUser.Status != HrStatus && currentUser.Status != AdminStatus))
                {
                    return OperationResult.Fail("Недостаточно прав для добавления пользователей.");
                }

                var validation = ValidateManagedUser(currentUser, null, filial, status, login, password, name, surname);
                if (!validation.Success)
                {
                    return validation;
                }

                var exists = db.ExecuteScalar<long>(
                    "SELECT COUNT(1) FROM Users WHERE Login = @Login",
                    new { Login = login.Trim() });

                if (exists > 0)
                {
                    return OperationResult.Fail("Пользователь с таким логином уже существует.");
                }

                string sql = @"
                    INSERT INTO Users (Login, Password, Name, Surname, SecondName, Filial, TeamNum, Status, IsActive)
                    VALUES (@Login, @Password, @Name, @Surname, @SecondName, @Filial, @TeamNum, @Status, 1)";

                db.Execute(sql, new
                {
                    Login = login.Trim(),
                    Password = password.Trim(),
                    Name = name.Trim(),
                    Surname = surname.Trim(),
                    SecondName = (secondName ?? string.Empty).Trim(),
                    Filial = filial,
                    TeamNum = teamNum,
                    Status = status
                });

                return OperationResult.Ok();
            }
        }

        public OperationResult UpdateUser(int currentUserId, int targetUserId, string login, string password, string name, string surname, string secondName, int filial, int teamNum, int status)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                var currentUser = db.QueryFirstOrDefault<ClassUser>("SELECT * FROM Users WHERE Id = @Id", new { Id = currentUserId });
                var targetUser = db.QueryFirstOrDefault<ClassUser>("SELECT * FROM Users WHERE Id = @Id", new { Id = targetUserId });

                if (currentUser == null || targetUser == null)
                {
                    return OperationResult.Fail("Пользователь не найден.");
                }

                if (currentUser.Status != HrStatus && currentUser.Status != AdminStatus)
                {
                    return OperationResult.Fail("Недостаточно прав для изменения пользователей.");
                }

                if (currentUserId == targetUserId)
                {
                    return OperationResult.Fail("Нельзя изменять собственную учётную запись в этом окне.");
                }

                if (IsProtectedAdmin(targetUser))
                {
                    return OperationResult.Fail("8 системных администраторов менять нельзя.");
                }

                var validation = ValidateManagedUser(currentUser, targetUser, filial, status, login, password, name, surname);
                if (!validation.Success)
                {
                    return validation;
                }

                var exists = db.ExecuteScalar<long>(
                    "SELECT COUNT(1) FROM Users WHERE Login = @Login AND Id <> @Id",
                    new { Login = login.Trim(), Id = targetUserId });

                if (exists > 0)
                {
                    return OperationResult.Fail("Логин уже занят другим пользователем.");
                }

                string sql = @"
                    UPDATE Users
                    SET Login = @Login,
                        Password = @Password,
                        Name = @Name,
                        Surname = @Surname,
                        SecondName = @SecondName,
                        Filial = @Filial,
                        TeamNum = @TeamNum,
                        Status = @Status
                    WHERE Id = @Id";

                db.Execute(sql, new
                {
                    Id = targetUserId,
                    Login = login.Trim(),
                    Password = password.Trim(),
                    Name = name.Trim(),
                    Surname = surname.Trim(),
                    SecondName = (secondName ?? string.Empty).Trim(),
                    Filial = filial,
                    TeamNum = teamNum,
                    Status = status
                });

                return OperationResult.Ok();
            }
        }

        public OperationResult DeleteUser(int currentUserId, int targetUserId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                var currentUser = db.QueryFirstOrDefault<ClassUser>("SELECT * FROM Users WHERE Id = @Id", new { Id = currentUserId });
                var targetUser = db.QueryFirstOrDefault<ClassUser>("SELECT * FROM Users WHERE Id = @Id", new { Id = targetUserId });

                if (currentUser == null || targetUser == null)
                {
                    return OperationResult.Fail("Пользователь не найден.");
                }

                if (currentUser.Status != HrStatus && currentUser.Status != AdminStatus)
                {
                    return OperationResult.Fail("Недостаточно прав для удаления пользователей.");
                }

                if (currentUserId == targetUserId)
                {
                    return OperationResult.Fail("Нельзя удалить собственную учётную запись.");
                }

                if (IsProtectedAdmin(targetUser))
                {
                    return OperationResult.Fail("8 системных администраторов удалять нельзя.");
                }

                if (currentUser.Status == HrStatus)
                {
                    if (currentUser.Filial != targetUser.Filial)
                    {
                        return OperationResult.Fail("Отдел кадров может работать только со своим филиалом.");
                    }

                    if (targetUser.Status == ManagerStatus)
                    {
                        return OperationResult.Fail("Отдел кадров не может удалять начальника филиала и его заместителей.");
                    }

                    if (targetUser.Status == AdminStatus)
                    {
                        return OperationResult.Fail("Администраторов может удалять только администратор.");
                    }
                }

                db.Execute("DELETE FROM Requests WHERE UserId = @UserId", new { UserId = targetUserId });
                db.Execute("DELETE FROM Users WHERE Id = @UserId", new { UserId = targetUserId });

                return OperationResult.Ok();
            }
        }

        private OperationResult ValidateManagedUser(ClassUser currentUser, ClassUser? targetUser, int filial, int status, string login, string password, string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(surname))
            {
                return OperationResult.Fail("Заполните логин, пароль, имя и фамилию.");
            }

            if (filial <= 0)
            {
                return OperationResult.Fail("Выберите филиал.");
            }

            if (currentUser.Status == HrStatus)
            {
                if (filial != currentUser.Filial)
                {
                    return OperationResult.Fail("Отдел кадров может назначать пользователей только в свой филиал.");
                }

                if (status >= ManagerStatus)
                {
                    return OperationResult.Fail("Отдел кадров не может добавлять или назначать начальника, заместителя и администратора.");
                }

                if (targetUser != null && targetUser.Status >= ManagerStatus)
                {
                    return OperationResult.Fail("Отдел кадров не может изменять начальника, заместителя и администратора.");
                }
            }

            return OperationResult.Ok();
        }

        private bool IsProtectedAdmin(ClassUser user)
        {
            return user != null && user.Status == AdminStatus && user.Id <= ProtectedAdminCount;
        }

        private bool IsProtectedAdmin(UserManagementItem user)
        {
            return user != null && user.Status == AdminStatus && user.Id <= ProtectedAdminCount;
        }

        // Создаёт таблицу WorkSchedule если её нет.
        // Вызвать один раз из SQL_DB.SQLite_DB() после создания остальных таблиц:
        //   ExecuteCommand(conn, WorkScheduleTableSql);
        public static readonly string WorkScheduleTableSql = @"
    CREATE TABLE IF NOT EXISTS WorkSchedule (
        Id        INTEGER PRIMARY KEY AUTOINCREMENT,
        UserId    INTEGER NOT NULL,
        WorkDate  TEXT    NOT NULL,
        ShiftType TEXT    NOT NULL,
        CreatedBy INTEGER NOT NULL,
        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
        FOREIGN KEY (UserId)    REFERENCES Users(Id),
        FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
    );";

        // Возвращает список записей графика для филиала текущего пользователя.
        // Если filterDate задан — только за этот месяц (формат "yyyy-MM").
        public List<WorkScheduleItem> GetSchedule(int currentUserId, string? filterMonth = null)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                int filial = db.ExecuteScalar<int>(
                    "SELECT Filial FROM Users WHERE Id = @Id", new { Id = currentUserId });

                string monthFilter = string.IsNullOrWhiteSpace(filterMonth)
                    ? ""
                    : "AND strftime('%Y-%m', ws.WorkDate) = @Month";

                string sql = $@"
            SELECT
                ws.Id,
                ws.UserId,
                (u.Surname || ' ' || substr(u.Name,1,1) || '.' ||
                 CASE WHEN u.SecondName IS NOT NULL AND u.SecondName != ''
                      THEN substr(u.SecondName,1,1) || '.' ELSE '' END) AS UserFullName,
                u.TeamNum,
                ws.WorkDate,
                ws.ShiftType,
                f.Id   AS FilialId,
                f.Name AS FilialName
            FROM WorkSchedule ws
            INNER JOIN Users   u ON u.Id = ws.UserId
            INNER JOIN Filials f ON f.Id = u.Filial
            WHERE u.Filial = @Filial AND u.IsActive = 1
            {monthFilter}
            ORDER BY ws.WorkDate, ws.ShiftType, u.Surname";

                return db.Query<WorkScheduleItem>(sql,
                    new { Filial = filial, Month = filterMonth }).ToList();
            }
        }

        // Возвращает активных сотрудников филиала текущего пользователя
        public List<ClassUser> GetActiveEmployeesForSchedule(int currentUserId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                int filial = db.ExecuteScalar<int>(
                    "SELECT Filial FROM Users WHERE Id = @Id", new { Id = currentUserId });

                string sql = @"
            SELECT u.Id, u.Name, u.Surname, u.SecondName, u.TeamNum, u.Filial, u.Status
            FROM Users u
            WHERE u.Filial = @Filial AND u.IsActive = 1
              AND NOT (u.Status = 6 AND u.Id <= 16)
            ORDER BY u.Surname, u.Name";

                return db.Query<ClassUser>(sql, new { Filial = filial }).ToList();
            }
        }

        // Добавляет одну запись в график.
        public OperationResult AddScheduleEntry(int currentUserId, int targetUserId,
            DateTime workDate, string shiftType)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                // Проверяем права: только нач. филиала/зам (5) и выше
                var cur = db.QueryFirstOrDefault<ClassUser>(
                    "SELECT * FROM Users WHERE Id = @Id", new { Id = currentUserId });
                if (cur == null || cur.Status < ManagerStatus)
                    return OperationResult.Fail("Составлять график могут только начальник филиала, его заместители и администратор.");

                var target = db.QueryFirstOrDefault<ClassUser>(
                    "SELECT * FROM Users WHERE Id = @Id", new { Id = targetUserId });
                if (target == null || target.Filial != cur.Filial)
                    return OperationResult.Fail("Сотрудник не найден или принадлежит другому филиалу.");

                if (shiftType != "Дневная" && shiftType != "Ночная")
                    return OperationResult.Fail("Тип смены должен быть «Дневная» или «Ночная».");

                string dateStr = workDate.ToString("yyyy-MM-dd");

                // Дубликат: один сотрудник — одна смена в сутки
                long exists = db.ExecuteScalar<long>(
                    "SELECT COUNT(1) FROM WorkSchedule WHERE UserId = @UserId AND WorkDate = @Date",
                    new { UserId = targetUserId, Date = dateStr });
                if (exists > 0)
                    return OperationResult.Fail("На эту дату у сотрудника уже есть запись в графике.");

                db.Execute(@"
            INSERT INTO WorkSchedule (UserId, WorkDate, ShiftType, CreatedBy)
            VALUES (@UserId, @WorkDate, @ShiftType, @CreatedBy)",
                    new
                    {
                        UserId = targetUserId,
                        WorkDate = dateStr,
                        ShiftType = shiftType,
                        CreatedBy = currentUserId
                    });

                return OperationResult.Ok();
            }
        }

        // удлаить запись из графика
        public OperationResult DeleteScheduleEntry(int currentUserId, int scheduleId)
        {
            using (IDbConnection db = new SqliteConnection(_connection))
            {
                var cur = db.QueryFirstOrDefault<ClassUser>(
                    "SELECT * FROM Users WHERE Id = @Id", new { Id = currentUserId });
                if (cur == null || cur.Status < ManagerStatus)
                    return OperationResult.Fail("Недостаточно прав для удаления записей графика.");

                int rows = db.Execute(
                    "DELETE FROM WorkSchedule WHERE Id = @Id", new { Id = scheduleId });

                return rows > 0
                    ? OperationResult.Ok()
                    : OperationResult.Fail("Запись не найдена.");
            }
        }
    }
}

