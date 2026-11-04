namespace вход_курсач
{
    partial class Authorization
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LoginTxt = new TextBox();
            PasswordTxt = new TextBox();
            SecondNameTxt = new TextBox();
            NameTxt = new TextBox();
            FilialTxt = new TextBox();
            FilialCombo = new ComboBox();
            SurnameTxt = new TextBox();
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            SuspendLayout();
            // 
            // LoginTxt
            // 
            LoginTxt.Location = new Point(702, 160);
            LoginTxt.Name = "LoginTxt";
            LoginTxt.Size = new Size(253, 27);
            LoginTxt.TabIndex = 0;
            // 
            // PasswordTxt
            // 
            PasswordTxt.Location = new Point(702, 233);
            PasswordTxt.Name = "PasswordTxt";
            PasswordTxt.Size = new Size(257, 27);
            PasswordTxt.TabIndex = 1;
            // 
            // SecondNameTxt
            // 
            SecondNameTxt.Location = new Point(202, 304);
            SecondNameTxt.Name = "SecondNameTxt";
            SecondNameTxt.Size = new Size(253, 27);
            SecondNameTxt.TabIndex = 2;
            // 
            // NameTxt
            // 
            NameTxt.Location = new Point(202, 160);
            NameTxt.Name = "NameTxt";
            NameTxt.Size = new Size(253, 27);
            NameTxt.TabIndex = 3;
            // 
            // FilialCombo
            // 
            FilialCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            FilialCombo.Location = new Point(698, 304);
            FilialCombo.Name = "FilialCombo";
            FilialCombo.Size = new Size(257, 27);
            FilialCombo.TabIndex = 4;
            // 
            // SurnameTxt
            // 
            SurnameTxt.Location = new Point(202, 230);
            SurnameTxt.Name = "SurnameTxt";
            SurnameTxt.Size = new Size(253, 27);
            SurnameTxt.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new Point(421, 427);
            button1.Name = "button1";
            button1.Size = new Size(192, 58);
            button1.TabIndex = 8;
            button1.Text = "Подать заявку";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(512, 163);
            label1.Name = "label1";
            label1.Size = new Size(146, 20);
            label1.TabIndex = 9;
            label1.Text = "Login пользователя";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(512, 236);
            label2.Name = "label2";
            label2.Size = new Size(162, 20);
            label2.TabIndex = 10;
            label2.Text = "Пароль пользователя";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 236);
            label3.Name = "label3";
            label3.Size = new Size(130, 20);
            label3.TabIndex = 11;
            label3.Text = "Введите отчество";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 310);
            label4.Name = "label4";
            label4.Size = new Size(136, 20);
            label4.TabIndex = 12;
            label4.Text = "Введите фамилию";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 163);
            label5.Name = "label5";
            label5.Size = new Size(97, 20);
            label5.TabIndex = 13;
            label5.Text = "Введите имя";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(512, 310);
            label6.Name = "label6";
            label6.Size = new Size(62, 20);
            label6.TabIndex = 14;
            label6.Text = "Филиал";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label7.Location = new Point(32, 38);
            label7.Name = "label7";
            label7.Size = new Size(985, 46);
            label7.TabIndex = 15;
            label7.Text = "Приветствуем, вас на форме авторизации нового сотрудника";
            // 
            // Authorization
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1120, 613);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(SurnameTxt);
            Controls.Add(FilialCombo);
            Controls.Add(NameTxt);
            Controls.Add(SecondNameTxt);
            Controls.Add(PasswordTxt);
            Controls.Add(LoginTxt);
            Name = "Authorization";
            Text = "Authorization";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox LoginTxt;
        private TextBox PasswordTxt;
        private TextBox SecondNameTxt;
        private TextBox NameTxt;
        private TextBox FilialTxt;
        private TextBox SurnameTxt;
        private ComboBox FilialCombo;
        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
    }
}