namespace вход_курсач
{
    // Роли (статусы) пользователей — значения столбца Users.Status
    // и таблицы Status_and_RoleName.
    internal static class Roles
    {
        public const int Request = 0;       // заявка, ещё не подтверждена
        public const int Worker = 1;        // работник
        public const int Hr = 2;            // отдел кадров
        public const int ShiftManager = 3;  // начальник смены
        public const int Accountant = 4;    // бухгалтер
        public const int Manager = 5;       // начальник филиала / заместитель
        public const int Admin = 6;         // администратор
        public const int Director = 7;      // генеральный директор
    }

    // Строка таблицы Users
    internal class ClassUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;     // фамилия
        public string SecondName { get; set; } = string.Empty;  // отчество
        public int Filial { get; set; }
        public string FilialName { get; set; } = string.Empty;
        public int TeamNum { get; set; }
        public int Status { get; set; }
        public int IsActive { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
