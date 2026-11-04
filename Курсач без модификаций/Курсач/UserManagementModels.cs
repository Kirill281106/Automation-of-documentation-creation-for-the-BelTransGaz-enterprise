namespace вход_курсач
{
    // Модель для отображения в DataGridView (без Id-шников)
    internal class FilialDisplayItem
    {
        public string FilialName      { get; set; } = string.Empty;
        public string Address         { get; set; } = string.Empty;
        public string Contact         { get; set; } = string.Empty;
        public string ManagerFullName { get; set; } = string.Empty;
    }

    // Модель для редактирования филиала (с Id для UPDATE/DELETE)
    internal class FilialEditItem
    {
        public int    Id        { get; set; }
        public string Name      { get; set; } = string.Empty;
        public string Address   { get; set; } = string.Empty;
        public string Contact   { get; set; } = string.Empty;
        public int?   ManagerId { get; set; }
    }

    // Модель для ComboBox выбора начальника
    internal class EmployeeItem
    {
        public int    Id       { get; set; }
        public string FullName { get; set; } = string.Empty;
        public override string ToString() => FullName;
    }

    internal class UserManagementItem
    {
        public int    Id           { get; set; }
        public string Login        { get; set; } = string.Empty;
        public string Name         { get; set; } = string.Empty;
        public string Surname      { get; set; } = string.Empty;
        public string SecondName   { get; set; } = string.Empty;
        public int    Filial       { get; set; }
        public string FilialName   { get; set; } = string.Empty;
        public int    TeamNum      { get; set; }
        public int    Status       { get; set; }
        public string RoleName     { get; set; } = string.Empty;
        public int    IsActive     { get; set; }
        public string ProtectionNote { get; set; } = string.Empty;
    }

    internal class OperationResult
    {
        public bool   Success      { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static OperationResult Ok() => new OperationResult { Success = true };

        public static OperationResult Fail(string errorMessage) =>
            new OperationResult { Success = false, ErrorMessage = errorMessage };
    }
}
