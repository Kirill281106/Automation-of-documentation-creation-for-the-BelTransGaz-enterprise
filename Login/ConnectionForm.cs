namespace вход_курсач
{
    // Окно параметров подключения к PostgreSQL. Показывается при первом запуске
    // и если подключиться с сохранёнными параметрами не удалось.
    public partial class ConnectionForm : Form
    {
        public DbSettings Settings { get; private set; }

        public ConnectionForm(DbSettings settings)
        {
            InitializeComponent();
            Settings = settings;

            txtHost.Text = settings.Host;
            txtPort.Text = settings.Port.ToString();
            txtDatabase.Text = settings.Database;
            txtUsername.Text = settings.Username;
            txtPassword.Text = settings.Password;

            Design.GazpromStyle.StyleAll(this);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text.Trim(), out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("Порт должен быть числом от 1 до 65535.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtHost.Text) ||
                string.IsNullOrWhiteSpace(txtDatabase.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Заполните сервер, базу данных и пользователя.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Settings = new DbSettings
            {
                Host = txtHost.Text.Trim(),
                Port = port,
                Database = txtDatabase.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
