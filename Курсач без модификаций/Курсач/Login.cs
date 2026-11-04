namespace вход_курсач
{
    public partial class Login : Form
    {
        private DB _userDb;

        public Login()
        {
            InitializeComponent();
            _userDb = new DB(Program.ConnectionString);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string login = LoginTXT.Text.Trim();
            string password = PaswTXT.Text.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            ClassUser user = _userDb.Authenticate(login, password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            if (user.Status == 0)
            {
                MessageBox.Show("Ваш аккаунт ещё не подтверждён");
                return;
            }

            //MessageBox.Show($"Добро пожаловать, {user.Name}! Роль: {user.RoleName}");
            Main mainForm = new Main(user.Status, user.Id);
            this.Hide();
            mainForm.ShowDialog();
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Authorization authorization = new Authorization();
            this.Hide();
            authorization.ShowDialog();
            this.Close();
        }
    }
}