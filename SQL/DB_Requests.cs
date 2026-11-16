using Dapper;

namespace вход_курсач
{
    // Заявки на регистрацию: подача, список, одобрение, удаление.
    internal class DB_Requests : DB_Base
    {
        // Подача заявки: создаёт неактивного пользователя и запись в таблице Requests.
        // Возвращает null, если логин уже занят или филиал не найден.
        public ClassUser? CreateApplication(string name, string surname, string secondName,
            string login, string password, string filialName)
        {
            using var db = Open();

            long loginTaken = db.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM Users WHERE Login = @Login", new { Login = login });
            if (loginTaken > 0)
                return null;

            int? filialId = db.QueryFirstOrDefault<int?>(
                "SELECT Id FROM Filials WHERE Name = @Name", new { Name = filialName });
            if (filialId == null)
                return null;

            // Пользователь и его заявка создаются вместе: либо обе записи, либо ни одной
            using var transaction = db.BeginTransaction();

            int newId = db.ExecuteScalar<int>(@"
                INSERT INTO Users (Login, Password, Name, Surname, SecondName, Filial, Status)
                VALUES (@Login, @Password, @Name, @Surname, @SecondName, @Filial, @Status)
                RETURNING Id",
                new
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    SecondName = secondName,
                    Filial = filialId.Value,
                    Status = Roles.Request
                });

            db.Execute("INSERT INTO Requests (UserId, RequestStatus) VALUES (@UserId, 0)",
                new { UserId = newId });

            transaction.Commit();
            return FindUser(db, newId);
        }

        // Неподтверждённые заявки филиала текущего пользователя
        public List<ClassUser> ListApplication(int currentUserId)
        {
            using var db = Open();
            return db.Query<ClassUser>(@"
                SELECT
                    u.Id, u.Name, u.Surname, u.SecondName, u.Login, u.Password,
                    u.Filial, u.TeamNum, u.Status, u.IsActive,
                    f.Name AS FilialName
                FROM Users u
                INNER JOIN Filials f ON f.Id = u.Filial
                WHERE u.Filial = (SELECT Filial FROM Users WHERE Id = @UserId)
                  AND u.IsActive = 0
                ORDER BY u.CreatedAt DESC",
                new { UserId = currentUserId }).ToList();
        }

        // Одобрение заявки: пользователь становится активным и получает роль
        public void ApproveRequest(int userId, int newStatus)
        {
            using var db = Open();
            using var transaction = db.BeginTransaction();

            db.Execute("UPDATE Users SET IsActive = 1, Status = @Status WHERE Id = @Id",
                new { Status = newStatus, Id = userId });
            db.Execute("UPDATE Requests SET RequestStatus = 1 WHERE UserId = @UserId",
                new { UserId = userId });

            transaction.Commit();
        }

        // Удаление заявки вместе с неактивным пользователем
        public bool DeleteRequest(int userId)
        {
            using var db = Open();
            using var transaction = db.BeginTransaction();

            db.Execute("DELETE FROM Requests WHERE UserId = @UserId", new { UserId = userId });
            int rows = db.Execute("DELETE FROM Users WHERE Id = @UserId AND IsActive = 0",
                new { UserId = userId });

            transaction.Commit();
            return rows > 0;
        }
    }
}
