namespace вход_курсач
{
    partial class Filials
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            Добавить = new Button();
            Изменить = new Button();
            Удалить = new Button();
            txtName = new TextBox();
            txtAddress = new TextBox();
            txtContact = new TextBox();
            lblName = new Label();
            lblAddress = new Label();
            lblContact = new Label();
            linkLabel1 = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(721, 506);
            dataGridView1.TabIndex = 0;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // Добавить
            // 
            Добавить.Location = new Point(762, 380);
            Добавить.Name = "Добавить";
            Добавить.Size = new Size(90, 50);
            Добавить.TabIndex = 6;
            Добавить.Text = "Добавить";
            Добавить.UseVisualStyleBackColor = true;
            Добавить.Visible = false;
            Добавить.Click += Добавить_Click_1;
            // 
            // Изменить
            // 
            Изменить.Location = new Point(862, 380);
            Изменить.Name = "Изменить";
            Изменить.Size = new Size(100, 50);
            Изменить.TabIndex = 7;
            Изменить.Text = "Изменить";
            Изменить.UseVisualStyleBackColor = true;
            Изменить.Visible = false;
            Изменить.Click += Изменить_Click;
            // 
            // Удалить
            // 
            Удалить.Location = new Point(972, 380);
            Удалить.Name = "Удалить";
            Удалить.Size = new Size(100, 50);
            Удалить.TabIndex = 8;
            Удалить.Text = "Удалить";
            Удалить.UseVisualStyleBackColor = true;
            Удалить.Visible = false;
            Удалить.Click += Удалить_Click;
            // 
            // txtName
            // 
            txtName.Location = new Point(762, 36);
            txtName.Name = "txtName";
            txtName.Size = new Size(310, 27);
            txtName.TabIndex = 1;
            txtName.Visible = false;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(762, 100);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(310, 27);
            txtAddress.TabIndex = 2;
            txtAddress.Visible = false;
            // 
            // txtContact
            // 
            txtContact.Location = new Point(762, 164);
            txtContact.Name = "txtContact";
            txtContact.Size = new Size(310, 27);
            txtContact.TabIndex = 3;
            txtContact.Visible = false;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(762, 12);
            lblName.Name = "lblName";
            lblName.Size = new Size(141, 20);
            lblName.TabIndex = 1;
            lblName.Text = "Название филиала";
            lblName.Visible = false;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(762, 76);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(115, 20);
            lblAddress.TabIndex = 2;
            lblAddress.Text = "Адрес филиала";
            lblAddress.Visible = false;
            // 
            // lblContact
            // 
            lblContact.AutoSize = true;
            lblContact.Location = new Point(762, 140);
            lblContact.Name = "lblContact";
            lblContact.Size = new Size(133, 20);
            lblContact.TabIndex = 3;
            lblContact.Text = "Телефон филиала";
            lblContact.Visible = false;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(762, 455);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(176, 20);
            linkLabel1.TabIndex = 9;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Открыть сайт компании";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // Filials
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 530);
            Controls.Add(linkLabel1);
            Controls.Add(dataGridView1);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            Controls.Add(lblContact);
            Controls.Add(txtContact);
            Controls.Add(Добавить);
            Controls.Add(Изменить);
            Controls.Add(Удалить);
            Name = "Filials";
            Text = "Управление филиалами";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Button Добавить;
        private Button Изменить;
        private Button Удалить;
        private TextBox txtName;
        private TextBox txtAddress;
        private TextBox txtContact;
        private Label lblName;
        private Label lblAddress;
        private Label lblContact;
        private LinkLabel linkLabel1;
    }
}
