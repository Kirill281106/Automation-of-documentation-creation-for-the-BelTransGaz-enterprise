using Dapper;
using Npgsql;
using System.Data;

namespace вход_курсач
{
    // Общая основа всех классов работы с БД: подключение к PostgreSQL
    // и то, что нужно сразу нескольким классам.
    internal abstract class DB_Base
    {
        // Код ошибки PostgreSQL «нарушение внешнего ключа»
        // (например, попытка удалить пользователя, на которого ссылаются отчёты).
        protected const string ForeignKeyViolation = "23503";

        // SQL-выражение «Фамилия Имя Отчество» для таблицы Users с псевдонимом u
        protected const string FullNameSql = @"
            u.Surname || ' ' || u.Name ||
            CASE WHEN u.SecondName IS NOT NULL AND u.SecondName <> ''
                 THEN ' ' || u.SecondName ELSE '' END";

        protected static NpgsqlConnection Open()
        {
            var conn = new NpgsqlConnection(Program.ConnectionString);
            conn.Open();
            return conn;
        }

        // Пользователь по Id: нужен почти везде для проверки прав и определения филиала
        protected static ClassUser? FindUser(IDbConnection db, int userId)
        {
            return db.QueryFirstOrDefault<ClassUser>(
                "SELECT * FROM Users WHERE Id = @Id", new { Id = userId });
        }
    }
}
