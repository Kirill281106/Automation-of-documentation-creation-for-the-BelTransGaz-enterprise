using Dapper;

namespace вход_курсач
{
    public partial class Authorization : Form
    {
        private DB _db;

        public Authorization()
        {
            InitializeComponent();
            _db = new DB(Program.ConnectionString);
            this.Load += Authorization_Load;
        }
        public void Authorization_FormClosing()
        {
            Application.Exit();
        }
        private void Authorization_Load(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(Program.ConnectionString))
                {
                    conn.Open();
                    var list = conn.Query("SELECT Id, Name FROM Filials ORDER BY Name").ToList();
                    FilialCombo.DisplayMember = "Name";
                    FilialCombo.ValueMember = "Id";
                    FilialCombo.DataSource = list;
                    FilialCombo.SelectedIndex = -1;

                    if (list.Count == 0)
                    {
                        MessageBox.Show("Список филиалов пуст. Обратитесь к администратору.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке филиалов: {ex.Message}\n\nПожалуйста, перезагрузите приложение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FilialCombo.Items.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = LoginTxt.Text.Trim();
            string password = PasswordTxt.Text.Trim();
            string name = NameTxt.Text.Trim();
            string surname = SurnameTxt.Text.Trim();
            string secondName = SecondNameTxt.Text.Trim();
            string filialName = FilialCombo.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname)||string.IsNullOrEmpty(filialName))
            {
                MessageBox.Show("Заполните обязательные поля: имя, фамилия, логин, пароль и название филиала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var created = _db.CreateApplication(name, surname, secondName, login, password, filialName);

            if (created == null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует или произошла ошибка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Заявка отправлена. Дождитесь подтверждения.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
