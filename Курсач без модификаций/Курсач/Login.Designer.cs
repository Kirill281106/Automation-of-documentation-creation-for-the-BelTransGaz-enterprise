namespace вход_курсач
{
    partial class Login
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
            Войти = new Button();
            LoginTXT = new TextBox();
            PaswTXT = new TextBox();
            label2 = new Label();
            label3 = new Label();
            linkLabel1 = new LinkLabel();
            SuspendLayout();
            // 
            // Войти
            // 
            Войти.Location = new Point(130, 222);
            Войти.Name = "Войти";
            Войти.Size = new Size(98, 40);
            Войти.TabIndex = 0;
            Войти.Text = "Войти";
            Войти.UseVisualStyleBackColor = true;
            Войти.Click += button1_Click;
            // 
            // LoginTXT
            // 
            LoginTXT.Location = new Point(205, 51);
            LoginTXT.Name = "LoginTXT";
            LoginTXT.Size = new Size(167, 27);
            LoginTXT.TabIndex = 1;
            // 
            // PaswTXT
            // 
            PaswTXT.Location = new Point(205, 134);
            PaswTXT.Name = "PaswTXT";
            PaswTXT.Size = new Size(167, 27);
            PaswTXT.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 137);
            label2.Name = "label2";
            label2.Size = new Size(72, 20);
            label2.TabIndex = 4;
            label2.Text = "password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(41, 54);
            label3.Name = "label3";
            label3.Size = new Size(43, 20);
            label3.TabIndex = 3;
            label3.Text = "login";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(130, 294);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(98, 20);
            linkLabel1.TabIndex = 5;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Нет аккаунта";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 365);
            Controls.Add(linkLabel1);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(PaswTXT);
            Controls.Add(LoginTXT);
            Controls.Add(Войти);
            Name = "Login";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Войти;
        private TextBox LoginTXT;
        private TextBox PaswTXT;
        private Label label2;
        private Label label3;
        private LinkLabel linkLabel1;
    }
}
