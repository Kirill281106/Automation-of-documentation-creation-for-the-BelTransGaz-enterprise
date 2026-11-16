
namespace вход_курсач
{
    public partial class Login : Form
    {
        private readonly DB_Users _userDb = new DB_Users();

        public Login()
        {
            InitializeComponent();
            ApplyGazpromStyle();
        }

        private void ApplyGazpromStyle()
        {
            Design.GazpromStyle.StyleForm(this);
            Design.GazpromStyle.StyleLabel(label2);
            Design.GazpromStyle.StyleLabel(label3);
            Design.GazpromStyle.StyleTextBox(LoginTXT);
            Design.GazpromStyle.StyleTextBox(PaswTXT);
            Design.GazpromStyle.StyleButton(Войти);
            Design.GazpromStyle.StyleLinkLabel(linkLabel1);
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

            ClassUser? user = _userDb.Authenticate(login, password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            if (user.Status == Roles.Request)
            {
                MessageBox.Show("Ваш аккаунт ещё не подтверждён");
                return;
            }

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