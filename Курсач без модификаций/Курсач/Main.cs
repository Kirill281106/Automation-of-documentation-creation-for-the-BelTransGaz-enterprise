using Dapper;
using System.Text;

namespace вход_курсач
{
    public partial class Main : Form
    {
        public int Status;
        public int UserId;
        public Main(int Status, int userId)
        {
            InitializeComponent();
            this.Status = Status;
            this.UserId = userId;
            ConfigureAccess();
        }
        public void Main_FormClosing()
        {
            Application.Exit();
        }
        private void заявкиНаСозданиеНовогоПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewUsers newUsers = new NewUsers(this.UserId);
            this.Hide();
            newUsers.ShowDialog();
            this.Show();
        }

        private void данныеФилиаловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filials f = new Filials();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void учётныеЗаписиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Status != 2 && this.Status != 6)
            {
                MessageBox.Show("Окно управления пользователями доступно только отделу кадров и администраторам.",
                    "Недостаточно прав", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UserManagementForm form = new UserManagementForm(this.UserId, this.Status);
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void ConfigureAccess()
        {
            пользователиToolStripMenuItem.Visible = this.Status == 2 || this.Status == 6;
        }

        private void графикРаботыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkScheduleForm workScheduleForm = new WorkScheduleForm(this.UserId, this.Status);
            this.Hide();
            workScheduleForm.ShowDialog();
            this.Show();
        }

        private void информацияОПредприToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
