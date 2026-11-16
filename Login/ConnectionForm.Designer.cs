namespace вход_курсач
{
    partial class ConnectionForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblHost = new Label();
            txtHost = new TextBox();
            lblPort = new Label();
            txtPort = new TextBox();
            lblDatabase = new Label();
            txtDatabase = new TextBox();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnConnect = new Button();
            SuspendLayout();
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Location = new Point(25, 28);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(120, 20);
            lblHost.TabIndex = 0;
            lblHost.Text = "Сервер:";
            // 
            // txtHost
            // 
            txtHost.Location = new Point(165, 25);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(210, 27);
            txtHost.TabIndex = 1;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(25, 73);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(120, 20);
            lblPort.TabIndex = 2;
            lblPort.Text = "Порт:";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(165, 70);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(210, 27);
            txtPort.TabIndex = 3;
            // 
            // lblDatabase
            // 
            lblDatabase.AutoSize = true;
            lblDatabase.Location = new Point(25, 118);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Size = new Size(120, 20);
            lblDatabase.TabIndex = 4;
            lblDatabase.Text = "База данных:";
            // 
            // txtDatabase
            // 
            txtDatabase.Location = new Point(165, 115);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.Size = new Size(210, 27);
            txtDatabase.TabIndex = 5;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(25, 163);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(120, 20);
            lblUsername.TabIndex = 6;
            lblUsername.Text = "Пользователь:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(165, 160);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(210, 27);
            txtUsername.TabIndex = 7;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(25, 208);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(120, 20);
            lblPassword.TabIndex = 8;
            lblPassword.Text = "Пароль:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(165, 205);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(210, 27);
            txtPassword.TabIndex = 9;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(125, 260);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(150, 40);
            btnConnect.TabIndex = 10;
            btnConnect.Text = "Подключиться";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // ConnectionForm
            // 
            AcceptButton = btnConnect;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 325);
            Controls.Add(lblHost);
            Controls.Add(txtHost);
            Controls.Add(lblPort);
            Controls.Add(txtPort);
            Controls.Add(lblDatabase);
            Controls.Add(txtDatabase);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnConnect);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConnectionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Подключение к PostgreSQL";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHost;
        private TextBox txtHost;
        private Label lblPort;
        private TextBox txtPort;
        private Label lblDatabase;
        private TextBox txtDatabase;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnConnect;
    }
}
