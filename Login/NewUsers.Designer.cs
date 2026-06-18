namespace вход_курсач
{
    partial class NewUsers
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
            dataGridView1 = new DataGridView();
            AddBT = new Button();
            comboBoxRole = new ComboBox();
            DeleteRequests = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(938, 497);
            dataGridView1.TabIndex = 0;
            // 
            // AddBT
            // 
            AddBT.Location = new Point(970, 92);
            AddBT.Name = "AddBT";
            AddBT.Size = new Size(140, 50);
            AddBT.TabIndex = 1;
            AddBT.Text = "Одобрить";
            AddBT.UseVisualStyleBackColor = true;
            AddBT.Click += AddBT_Click;
            // 
            // comboBoxRole
            // 
            comboBoxRole.AllowDrop = true;
            comboBoxRole.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxRole.FormattingEnabled = true;
            comboBoxRole.Location = new Point(970, 12);
            comboBoxRole.Name = "comboBoxRole";
            comboBoxRole.Size = new Size(298, 28);
            comboBoxRole.TabIndex = 2;
            // 
            // DeleteRequests
            // 
            DeleteRequests.Location = new Point(970, 180);
            DeleteRequests.Name = "DeleteRequests";
            DeleteRequests.Size = new Size(140, 50);
            DeleteRequests.TabIndex = 3;
            DeleteRequests.Text = "Отклонить";
            DeleteRequests.UseVisualStyleBackColor = true;
            DeleteRequests.Click += DeleteRequests_Click;
            // 
            // NewUsers
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1312, 521);
            Controls.Add(DeleteRequests);
            Controls.Add(comboBoxRole);
            Controls.Add(AddBT);
            Controls.Add(dataGridView1);
            Name = "NewUsers";
            Text = "NewUsers";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Button AddBT;
        private ComboBox comboBoxRole;
        private Button DeleteRequests;
    }
}