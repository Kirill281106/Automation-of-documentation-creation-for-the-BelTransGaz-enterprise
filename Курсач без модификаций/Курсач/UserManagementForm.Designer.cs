namespace вход_курсач
{
    partial class UserManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dataGridViewUsers = new DataGridView();
            labelLogin = new Label();
            textBoxLogin = new TextBox();
            labelPassword = new Label();
            textBoxPassword = new TextBox();
            labelName = new Label();
            textBoxName = new TextBox();
            labelSurname = new Label();
            textBoxSurname = new TextBox();
            labelSecondName = new Label();
            textBoxSecondName = new TextBox();
            labelTeam = new Label();
            numericTeam = new NumericUpDown();
            labelRole = new Label();
            comboBoxRole = new ComboBox();
            buttonAdd = new Button();
            buttonUpdate = new Button();
            buttonDelete = new Button();
            labelHint = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericTeam).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewUsers
            // 
            dataGridViewUsers.AllowUserToAddRows = false;
            dataGridViewUsers.AllowUserToDeleteRows = false;
            dataGridViewUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsers.Location = new Point(12, 12);
            dataGridViewUsers.MultiSelect = false;
            dataGridViewUsers.Name = "dataGridViewUsers";
            dataGridViewUsers.ReadOnly = true;
            dataGridViewUsers.RowHeadersWidth = 51;
            dataGridViewUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsers.Size = new Size(938, 568);
            dataGridViewUsers.TabIndex = 0;
            dataGridViewUsers.SelectionChanged += dataGridViewUsers_SelectionChanged;
            // 
            // labelLogin
            // 
            labelLogin.AutoSize = true;
            labelLogin.Location = new Point(974, 18);
            labelLogin.Name = "labelLogin";
            labelLogin.Size = new Size(52, 20);
            labelLogin.TabIndex = 3;
            labelLogin.Text = "Логин";
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(974, 41);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(302, 27);
            textBoxLogin.TabIndex = 4;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(974, 77);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(62, 20);
            labelPassword.TabIndex = 5;
            labelPassword.Text = "Пароль";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(974, 100);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(302, 27);
            textBoxPassword.TabIndex = 6;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(974, 140);
            labelName.Name = "labelName";
            labelName.Size = new Size(39, 20);
            labelName.TabIndex = 7;
            labelName.Text = "Имя";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(974, 163);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(302, 27);
            textBoxName.TabIndex = 8;
            // 
            // labelSurname
            // 
            labelSurname.AutoSize = true;
            labelSurname.Location = new Point(974, 203);
            labelSurname.Name = "labelSurname";
            labelSurname.Size = new Size(73, 20);
            labelSurname.TabIndex = 9;
            labelSurname.Text = "Фамилия";
            // 
            // textBoxSurname
            // 
            textBoxSurname.Location = new Point(974, 226);
            textBoxSurname.Name = "textBoxSurname";
            textBoxSurname.Size = new Size(302, 27);
            textBoxSurname.TabIndex = 10;
            // 
            // labelSecondName
            // 
            labelSecondName.AutoSize = true;
            labelSecondName.Location = new Point(974, 266);
            labelSecondName.Name = "labelSecondName";
            labelSecondName.Size = new Size(72, 20);
            labelSecondName.TabIndex = 11;
            labelSecondName.Text = "Отчество";
            // 
            // textBoxSecondName
            // 
            textBoxSecondName.Location = new Point(974, 289);
            textBoxSecondName.Name = "textBoxSecondName";
            textBoxSecondName.Size = new Size(302, 27);
            textBoxSecondName.TabIndex = 12;
            // 
            // labelTeam
            // 
            labelTeam.AutoSize = true;
            labelTeam.Location = new Point(974, 343);
            labelTeam.Name = "labelTeam";
            labelTeam.Size = new Size(54, 20);
            labelTeam.TabIndex = 15;
            labelTeam.Text = "Смена";
            // 
            // numericTeam
            // 
            numericTeam.Location = new Point(974, 366);
            numericTeam.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numericTeam.Name = "numericTeam";
            numericTeam.Size = new Size(302, 27);
            numericTeam.TabIndex = 16;
            // 
            // labelRole
            // 
            labelRole.AutoSize = true;
            labelRole.Location = new Point(974, 406);
            labelRole.Name = "labelRole";
            labelRole.Size = new Size(42, 20);
            labelRole.TabIndex = 17;
            labelRole.Text = "Роль";
            // 
            // comboBoxRole
            // 
            comboBoxRole.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxRole.FormattingEnabled = true;
            comboBoxRole.Location = new Point(974, 429);
            comboBoxRole.Name = "comboBoxRole";
            comboBoxRole.Size = new Size(302, 28);
            comboBoxRole.TabIndex = 18;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(974, 490);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(142, 44);
            buttonAdd.TabIndex = 19;
            buttonAdd.Text = "Добавить";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(1134, 490);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(142, 44);
            buttonUpdate.TabIndex = 20;
            buttonUpdate.Text = "Изменить";
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Click += buttonUpdate_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(974, 550);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(142, 44);
            buttonDelete.TabIndex = 21;
            buttonDelete.Text = "Удалить";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // labelHint
            // 
            labelHint.Location = new Point(0, 0);
            labelHint.Name = "labelHint";
            labelHint.Size = new Size(100, 23);
            labelHint.TabIndex = 0;
            // 
            // UserManagementForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1294, 619);
            Controls.Add(buttonDelete);
            Controls.Add(buttonUpdate);
            Controls.Add(buttonAdd);
            Controls.Add(comboBoxRole);
            Controls.Add(labelRole);
            Controls.Add(numericTeam);
            Controls.Add(labelTeam);
            Controls.Add(textBoxSecondName);
            Controls.Add(labelSecondName);
            Controls.Add(textBoxSurname);
            Controls.Add(labelSurname);
            Controls.Add(textBoxName);
            Controls.Add(labelName);
            Controls.Add(textBoxPassword);
            Controls.Add(labelPassword);
            Controls.Add(textBoxLogin);
            Controls.Add(labelLogin);
            Controls.Add(dataGridViewUsers);
            Name = "UserManagementForm";
            Text = "Учётные записи";
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericTeam).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewUsers;
        private Label labelLogin;
        private TextBox textBoxLogin;
        private Label labelPassword;
        private TextBox textBoxPassword;
        private Label labelName;
        private TextBox textBoxName;
        private Label labelSurname;
        private TextBox textBoxSurname;
        private Label labelSecondName;
        private TextBox textBoxSecondName;
        private Label labelTeam;
        private NumericUpDown numericTeam;
        private Label labelRole;
        private ComboBox comboBoxRole;
        private Button buttonAdd;
        private Button buttonUpdate;
        private Button buttonDelete;
        private Label labelHint;
    }
}
