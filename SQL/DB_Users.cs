using Dapper;
using Npgsql;

namespace вход_курсач
{
    // Операции с пользователями: вход, роли, список, добавление, изменение, удаление.
    internal class DB_Users : DB_Base
    {
        // Первые 16 пользователей — системные учётные записи, администраторов среди них
        // нельзя менять и удалять, в списках они не показываются.
        private const int ProtectedAdminCount = 16;
        private const string NotProtectedAdminSql = "NOT (u.Status = 6 AND u.Id <= 16)";

        // ─── Вход ─────────────────────────────────────────────────────────────

        public ClassUser? Authenticate(string login, string password)
        {
            using var db = Open();
            return db.QueryFirstOrDefault<ClassUser>(
                "SELECT * FROM Users WHERE Login = @Login AND Password = @Password",
                new { Login = login, Password = password });
        }

        // ─── Чтение ───────────────────────────────────────────────────────────

        public ClassUser? GetUserById(int userId)
        {
            using var db = Open();
            return FindUser(db, userId);
        }

        public int GetUserFilial(int userId)
        {
            using var db = Open();
            return db.ExecuteScalar<int>(
                "SELECT Filial FROM Users WHERE Id = @Id", new { Id = userId });
        }

        public List<RoleItem> GetRoles()
        {
            using var db = Open();
            return db.Query<RoleItem>(
                "SELECT Status, RoleName FROM Status_and_RoleName ORDER BY Status").ToList();
        }

        // Список пользователей для окна «Учётные записи».
        // Отдел кадров и администратор видят свой филиал, генеральный директор — все филиалы.
        public List<UserManagementItem> GetUsersForManagement(int currentUserId)
        {
            using var db = Open();
            var currentUser = FindUser(db, currentUserId);
            if (currentUser == null)
                return new List<UserManagementItem>();

            string filialFilter;
            if (currentUser.Status == Roles.Director)
                filialFilter = "";
            else if (currentUser.Status == Roles.Hr || currentUser.Status == Roles.Admin)
                filialFilter = "AND u.Filial = @Filial";
            else
                return new List<UserManagementItem>();

            string sql = $@"
                SELECT
                    u.Id, u.Login, u.Name, u.Surname, u.SecondName,
                    u.Filial,
                    f.Name AS FilialName,
                    COALESCE(u.TeamNum, 0) AS TeamNum,
                    u.Status,
                    r.RoleName,
                    u.IsActive
                FROM Users u
                INNER JOIN Filials f ON f.Id = u.Filial
                INNER JOIN Status_and_RoleName r ON r.Status = u.Status
                WHERE u.IsActive = 1 AND {NotProtectedAdminSql} {filialFilter}
                ORDER BY u.Surname, u.Name, u.Id";

            return db.Query<UserManagementItem>(sql, new { Filial = currentUser.Filial }).ToList();
        }

        // Активные сотрудники филиала текущего пользователя (для составления графика работы)
        public List<ClassUser> GetActiveEmployeesForSchedule(int currentUserId)
        {
            using var db = Open();
            string sql = $@"
                SELECT u.Id, u.Name, u.Surname, u.SecondName, u.TeamNum, u.Filial, u.Status
                FROM Users u
                WHERE u.Filial = (SELECT Filial FROM Users WHERE Id = @Id)
                  AND u.IsActive = 1
                  AND {NotProtectedAdminSql}
                ORDER BY u.Surname, u.Name";

            return db.Query<ClassUser>(sql, new { Id = currentUserId }).ToList();
        }

        // Состав смены: сотрудники филиала с тем же номером смены, кроме самого пользователя
        public List<EmployeeItem> GetTeamMembers(int filialId, int teamNum, int excludeUserId)
        {
            using var db = Open();
            string sql = $@"
                SELECT u.Id, {FullNameSql} AS FullName
                FROM Users u
                WHERE u.Filial = @Filial
                  AND u.TeamNum = @TeamNum
                  AND u.IsActive = 1
                  AND u.Id <> @Exclude
                ORDER BY u.Surname, u.Name";

            return db.Query<EmployeeItem>(sql,
                new { Filial = filialId, TeamNum = teamNum, Exclude = excludeUserId }).ToList();
        }

        // Начальники смены филиала, кроме самого пользователя (кому можно передать смену)
        public List<EmployeeItem> GetShiftManagersInFilial(int filialId, int excludeUserId)
        {
            using var db = Open();
            string sql = $@"
                SELECT u.Id, {FullNameSql} AS FullName
                FROM Users u
                WHERE u.Filial = @Filial
                  AND u.IsActive = 1
                  AND u.Status = @ShiftManager
                  AND u.Id <> @Exclude
                ORDER BY u.Surname";

            return db.Query<EmployeeItem>(sql,
                new { Filial = filialId, Exclude = excludeUserId, Roles.ShiftManager }).ToList();
        }

        // ─── Добавление ───────────────────────────────────────────────────────

        public OperationResult AddUser(int currentUserId, string login, string password,
            string name, string surname, string secondName, int filial, int teamNum, int status)
        {
            using var db = Open();
            var currentUser = FindUser(db, currentUserId);

            if (currentUser == null || !CanManageUsers(currentUser))
                return OperationResult.Fail("Недостаточно прав для добавления пользователей.");

            var validation = ValidateManagedUser(currentUser, null, filial, status,
                login, password, name, surname);
            if (!validation.Success)
                return validation;

            long exists = db.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM Users WHERE Login = @Login", new { Login = login.Trim() });
            if (exists > 0)
                return OperationResult.Fail("Пользователь с таким логином уже существует.");

            db.Execute(@"
                INSERT INTO Users (Login, Password, Name, Surname, SecondName, Filial, TeamNum, Status, IsActive)
                VALUES (@Login, @Password, @Name, @Surname, @SecondName, @Filial, @TeamNum, @Status, 1)",
                new
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

        // ─── Изменение ────────────────────────────────────────────────────────

        public OperationResult UpdateUser(int currentUserId, int targetUserId, string login,
            string password, string name, string surname, string secondName,
            int filial, int teamNum, int status)
        {
            using var db = Open();
            var currentUser = FindUser(db, currentUserId);
            var targetUser = FindUser(db, targetUserId);

            if (currentUser == null || targetUser == null)
                return OperationResult.Fail("Пользователь не найден.");

            if (!CanManageUsers(currentUser))
                return OperationResult.Fail("Недостаточно прав для изменения пользователей.");

            if (currentUserId == targetUserId)
                return OperationResult.Fail("Нельзя изменять собственную учётную запись в этом окне.");

            if (IsProtectedAdmin(targetUser))
                return OperationResult.Fail("8 системных администраторов менять нельзя.");

            var validation = ValidateManagedUser(currentUser, targetUser, filial, status,
                login, password, name, surname);
            if (!validation.Success)
                return validation;

            long exists = db.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM Users WHERE Login = @Login AND Id <> @Id",
                new { Login = login.Trim(), Id = targetUserId });
            if (exists > 0)
                return OperationResult.Fail("Логин уже занят другим пользователем.");

            db.Execute(@"
                UPDATE Users
                SET Login = @Login,
                    Password = @Password,
                    Name = @Name,
                    Surname = @Surname,
                    SecondName = @SecondName,
                    Filial = @Filial,
                    TeamNum = @TeamNum,
                    Status = @Status
                WHERE Id = @Id",
                new
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

        // ─── Удаление ─────────────────────────────────────────────────────────

        public OperationResult DeleteUser(int currentUserId, int targetUserId)
        {
            using var db = Open();
            var currentUser = FindUser(db, currentUserId);
            var targetUser = FindUser(db, targetUserId);

            if (currentUser == null || targetUser == null)
                return OperationResult.Fail("Пользователь не найден.");

            if (!CanManageUsers(currentUser))
                return OperationResult.Fail("Недостаточно прав для удаления пользователей.");

            if (currentUserId == targetUserId)
                return OperationResult.Fail("Нельзя удалить собственную учётную запись.");

            if (IsProtectedAdmin(targetUser))
                return OperationResult.Fail("8 системных администраторов удалять нельзя.");

            if (currentUser.Status == Roles.Hr)
            {
                if (currentUser.Filial != targetUser.Filial)
                    return OperationResult.Fail("Отдел кадров может работать только со своим филиалом.");

                if (targetUser.Status == Roles.Manager)
                    return OperationResult.Fail("Отдел кадров не может удалять начальника филиала и его заместителей.");

                if (targetUser.Status == Roles.Admin)
                    return OperationResult.Fail("Администраторов может удалять только администратор.");
            }

            try
            {
                // Заявка и пользователь удаляются вместе: либо обе записи, либо ни одной
                using var transaction = db.BeginTransaction();
                db.Execute("DELETE FROM Requests WHERE UserId = @Id", new { Id = targetUserId });
                db.Execute("DELETE FROM Users WHERE Id = @Id", new { Id = targetUserId });
                transaction.Commit();
            }
            catch (PostgresException ex) when (ex.SqlState == ForeignKeyViolation)
            {
                return OperationResult.Fail(
                    "Нельзя удалить пользователя: он указан в отчётах, графике работы, плане или как начальник филиала.");
            }

            return OperationResult.Ok();
        }

        // ─── Проверки прав ────────────────────────────────────────────────────

        private static bool CanManageUsers(ClassUser user)
        {
            return user.Status == Roles.Hr || user.Status == Roles.Admin;
        }

        private static bool IsProtectedAdmin(ClassUser user)
        {
            return user.Status == Roles.Admin && user.Id <= ProtectedAdminCount;
        }

        private static OperationResult ValidateManagedUser(ClassUser currentUser, ClassUser? targetUser,
            int filial, int status, string login, string password, string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(surname))
                return OperationResult.Fail("Заполните логин, пароль, имя и фамилию.");

            if (filial <= 0)
                return OperationResult.Fail("Выберите филиал.");

            if (currentUser.Status == Roles.Hr)
            {
                if (filial != currentUser.Filial)
                    return OperationResult.Fail("Отдел кадров может назначать пользователей только в свой филиал.");

                if (status >= Roles.Manager)
                    return OperationResult.Fail("Отдел кадров не может добавлять или назначать начальника, заместителя и администратора.");

                if (targetUser != null && targetUser.Status >= Roles.Manager)
                    return OperationResult.Fail("Отдел кадров не может изменять начальника, заместителя и администратора.");
            }

            return OperationResult.Ok();
        }
    }
}
