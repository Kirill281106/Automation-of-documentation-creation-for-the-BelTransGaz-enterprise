using Dapper;
using Npgsql;

namespace вход_курсач
{
    // Операции с филиалами: список, добавление, изменение, удаление.
    internal class DB_Filials : DB_Base
    {
        // Id и название всех филиалов (для выпадающего списка при регистрации)
        public List<FilialItem> GetFilials()
        {
            using var db = Open();
            return db.Query<FilialItem>("SELECT Id, Name FROM Filials ORDER BY Name").ToList();
        }

        // Все филиалы с ФИО начальника (для таблицы в окне «Филиалы»)
        public List<FilialDisplayItem> GetFilialsForDisplay()
        {
            using var db = Open();
            string sql = $@"
                SELECT
                    f.Name AS FilialName,
                    f.Address,
                    f.Contact,
                    COALESCE({FullNameSql}, '—') AS ManagerFullName
                FROM Filials f
                LEFT JOIN Users u ON u.Id = f.ManagerId
                ORDER BY f.Name";

            return db.Query<FilialDisplayItem>(sql).ToList();
        }

        public FilialEditItem? GetFilialByName(string filialName)
        {
            using var db = Open();
            return db.QueryFirstOrDefault<FilialEditItem>(
                "SELECT Id, Name, Address, Contact, ManagerId FROM Filials WHERE Name = @Name",
                new { Name = filialName });
        }

        // Новый филиал создаётся без начальника (ManagerId = NULL)
        public OperationResult AddFilial(string name, string address, string contact)
        {
            var validation = ValidateFields(name, address, contact);
            if (!validation.Success)
                return validation;

            using var db = Open();
            long exists = db.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM Filials WHERE Name = @Name", new { Name = name.Trim() });
            if (exists > 0)
                return OperationResult.Fail("Филиал с таким названием уже существует.");

            db.Execute(
                "INSERT INTO Filials (Name, Address, Contact) VALUES (@Name, @Address, @Contact)",
                new { Name = name.Trim(), Address = address.Trim(), Contact = contact.Trim() });

            return OperationResult.Ok();
        }

        public OperationResult UpdateFilial(int filialId, string name, string address, string contact)
        {
            var validation = ValidateFields(name, address, contact);
            if (!validation.Success)
                return validation;

            using var db = Open();
            long exists = db.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM Filials WHERE Name = @Name AND Id <> @Id",
                new { Name = name.Trim(), Id = filialId });
            if (exists > 0)
                return OperationResult.Fail("Филиал с таким названием уже существует.");

            db.Execute(
                "UPDATE Filials SET Name = @Name, Address = @Address, Contact = @Contact WHERE Id = @Id",
                new { Id = filialId, Name = name.Trim(), Address = address.Trim(), Contact = contact.Trim() });

            return OperationResult.Ok();
        }

        public OperationResult DeleteFilial(int filialId)
        {
            using var db = Open();
            long usersInFilial = db.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM Users WHERE Filial = @Filial", new { Filial = filialId });
            if (usersInFilial > 0)
                return OperationResult.Fail("Нельзя удалить филиал, к которому привязаны сотрудники.");

            try
            {
                int rows = db.Execute("DELETE FROM Filials WHERE Id = @Id", new { Id = filialId });
                return rows > 0 ? OperationResult.Ok() : OperationResult.Fail("Филиал не найден.");
            }
            catch (PostgresException ex) when (ex.SqlState == ForeignKeyViolation)
            {
                return OperationResult.Fail("Нельзя удалить филиал, по которому есть отчёты или план поставки.");
            }
        }

        private static OperationResult ValidateFields(string name, string address, string contact)
        {
            if (string.IsNullOrWhiteSpace(name))
                return OperationResult.Fail("Введите название филиала.");
            if (string.IsNullOrWhiteSpace(address))
                return OperationResult.Fail("Введите адрес филиала.");
            if (string.IsNullOrWhiteSpace(contact))
                return OperationResult.Fail("Введите контактный телефон.");

            return OperationResult.Ok();
        }
    }
}
