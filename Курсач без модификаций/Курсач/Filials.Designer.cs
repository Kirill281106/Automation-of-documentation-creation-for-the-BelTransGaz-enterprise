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
            cmbManager = new ComboBox();
            lblName = new Label();
            lblAddress = new Label();
            lblContact = new Label();
            lblManager = new Label();
            btnClear = new Button();
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
            Добавить.Location = new Point(762, 450);
            Добавить.Name = "Добавить";
            Добавить.Size = new Size(90, 50);
            Добавить.TabIndex = 6;
            Добавить.Text = "Добавить";
            Добавить.UseVisualStyleBackColor = true;
            Добавить.Click += Добавить_Click_1;
            // 
            // Изменить
            // 
            Изменить.Location = new Point(862, 450);
            Изменить.Name = "Изменить";
            Изменить.Size = new Size(100, 50);
            Изменить.TabIndex = 7;
            Изменить.Text = "Изменить";
            Изменить.UseVisualStyleBackColor = true;
            Изменить.Click += Изменить_Click;
            // 
            // Удалить
            // 
            Удалить.Location = new Point(972, 450);
            Удалить.Name = "Удалить";
            Удалить.Size = new Size(100, 50);
            Удалить.TabIndex = 8;
            Удалить.Text = "Удалить";
            Удалить.UseVisualStyleBackColor = true;
            Удалить.Click += Удалить_Click;
            // 
            // txtName
            // 
            txtName.Location = new Point(762, 36);
            txtName.Name = "txtName";
            txtName.Size = new Size(310, 27);
            txtName.TabIndex = 1;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(762, 100);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(310, 27);
            txtAddress.TabIndex = 2;
            // 
            // txtContact
            // 
            txtContact.Location = new Point(762, 164);
            txtContact.Name = "txtContact";
            txtContact.Size = new Size(310, 27);
            txtContact.TabIndex = 3;
            // 
            // cmbManager
            // 
            cmbManager.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbManager.Location = new Point(762, 228);
            cmbManager.Name = "cmbManager";
            cmbManager.Size = new Size(310, 28);
            cmbManager.TabIndex = 4;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(762, 12);
            lblName.Name = "lblName";
            lblName.Size = new Size(141, 20);
            lblName.TabIndex = 1;
            lblName.Text = "Название филиала";
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(762, 76);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(115, 20);
            lblAddress.TabIndex = 2;
            lblAddress.Text = "Адрес филиала";
            // 
            // lblContact
            // 
            lblContact.AutoSize = true;
            lblContact.Location = new Point(762, 140);
            lblContact.Name = "lblContact";
            lblContact.Size = new Size(133, 20);
            lblContact.TabIndex = 3;
            lblContact.Text = "Телефон филиала";
            // 
            // lblManager
            // 
            lblManager.AutoSize = true;
            lblManager.Location = new Point(762, 204);
            lblManager.Name = "lblManager";
            lblManager.Size = new Size(149, 20);
            lblManager.TabIndex = 4;
            lblManager.Text = "Начальник филиала";
            // 
            // btnClear
            // 
            btnClear.Location = new Point(0, 0);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 5;
            // 
            // Filials
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 530);
            Controls.Add(dataGridView1);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            Controls.Add(lblContact);
            Controls.Add(txtContact);
            Controls.Add(lblManager);
            Controls.Add(cmbManager);
            Controls.Add(btnClear);
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
        private Button btnClear;
        private TextBox txtName;
        private TextBox txtAddress;
        private TextBox txtContact;
        private ComboBox cmbManager;
        private Label lblName;
        private Label lblAddress;
        private Label lblContact;
        private Label lblManager;
    }
}
