using Dapper;
using Microsoft.Data.Sqlite;

namespace вход_курсач
{
    internal class ClassUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; } // фамилия
        public string SecondName { get; set; } // отчество 
        public int Filial { get; set; }
        public string FilialName { get; set; }
        public int TeamNum { get; set; }
        public int Status { get; set; }
        public int IsActive { get; set; }


        private string _roleName = "";
        public string RoleName
        {
            get
            {
                if (_roleName == null)
                {
                    using (var conn = new SqliteConnection(Program.ConnectionString))
                    {
                        string sql = "SELECT RoleName FROM Status_and_RoleName WHERE Status = @Status";
                        _roleName = conn.QueryFirstOrDefault<string>(sql, new { Status = this.Status });
                    }
                }
                return _roleName;
            }
        }
    }
}
/*
 * работник может чекнуть график и новые сотрудники могут быть временно не прикреплены к команде
 * отдел кадров получает заявку и подтверждает сотрудника присваивая ему должность и статус
 * начальник смены чекнуть график и составить сменный отчёт
 * Бухгалтер который не относится к минскому филиалу может посмотреть прогноз роста, годовой и квартальный отчёт составить а который относится к минсому филиалу он может смотреть все отчёты а также сравнить  их
 * а так же поиск сортировка добавление удаление
 * нач филиала и его замы могут то же самое а также составить график работы на следующий период(месяц) 
 * нач и его замы минского филиала могут всё что и начальники и замы филиалов бухгалтера как головного так и филиала 
 */
