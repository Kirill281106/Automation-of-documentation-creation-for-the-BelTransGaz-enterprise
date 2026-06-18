namespace вход_курсач
{
    partial class CreateQuarterlyReportForm
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

        private void InitializeComponent()
        {
            cmbQuarter = new ComboBox();
            cmbYear = new ComboBox();
            btnCreate = new Button();
            btnOpen = new Button();
            dataGridView1 = new DataGridView();
            ExportBTN = new Button();
            Open_Analis = new Button();
            label1 = new Label();
            txtPurchasePrice = new TextBox();
            l1 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtSalePrice = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // cmbQuarter
            // 
            cmbQuarter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbQuarter.Location = new Point(776, 198);
            cmbQuarter.Name = "cmbQuarter";
            cmbQuarter.Size = new Size(151, 28);
            cmbQuarter.TabIndex = 4;
            // 
            // cmbYear
            // 
            cmbYear.Location = new Point(776, 257);
            cmbYear.Name = "cmbYear";
            cmbYear.Size = new Size(151, 28);
            cmbYear.TabIndex = 3;
            // 
            // btnCreate
            // 
            btnCreate.BackColor = Color.LightGreen;
            btnCreate.Location = new Point(776, 457);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(94, 29);
            btnCreate.TabIndex = 2;
            btnCreate.Text = "Создать";
            btnCreate.UseVisualStyleBackColor = false;
            btnCreate.Click += btnCreate_Click_1;
            // 
            // btnOpen
            // 
            btnOpen.Location = new Point(762, 17);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(94, 29);
            btnOpen.TabIndex = 1;
            btnOpen.Text = "Открыть";
            btnOpen.Click += btnOpen_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnHeadersHeight = 29;
            dataGridView1.Location = new Point(12, 17);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(705, 498);
            dataGridView1.TabIndex = 0;
            // 
            // ExportBTN
            // 
            ExportBTN.Location = new Point(762, 72);
            ExportBTN.Name = "ExportBTN";
            ExportBTN.Size = new Size(160, 29);
            ExportBTN.TabIndex = 5;
            ExportBTN.Text = "открыть в Word";
            ExportBTN.UseVisualStyleBackColor = true;
            ExportBTN.Click += ExportBTN_Click;
            // 
            // Open_Analis
            // 
            Open_Analis.Location = new Point(762, 123);
            Open_Analis.Name = "Open_Analis";
            Open_Analis.Size = new Size(218, 33);
            Open_Analis.TabIndex = 6;
            Open_Analis.Text = "Смотреть прогноз";
            Open_Analis.UseVisualStyleBackColor = true;
            Open_Analis.Click += Open_Analis_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(776, 314);
            label1.Name = "label1";
            label1.Size = new Size(204, 20);
            label1.TabIndex = 7;
            label1.Text = "Закупочная цена за 1000 м³";
            // 
            // txtPurchasePrice
            // 
            txtPurchasePrice.Location = new Point(776, 337);
            txtPurchasePrice.Name = "txtPurchasePrice";
            txtPurchasePrice.Size = new Size(151, 27);
            txtPurchasePrice.TabIndex = 8;
            // 
            // l1
            // 
            l1.AutoSize = true;
            l1.Location = new Point(776, 175);
            l1.Name = "l1";
            l1.Size = new Size(92, 20);
            l1.TabIndex = 9;
            l1.Text = "№ квартала";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(776, 234);
            label3.Name = "label3";
            label3.Size = new Size(32, 20);
            label3.TabIndex = 10;
            label3.Text = "год";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(776, 387);
            label2.Name = "label2";
            label2.Size = new Size(188, 20);
            label2.TabIndex = 11;
            label2.Text = "Цена продажи за 1000 м³";
            // 
            // txtSalePrice
            // 
            txtSalePrice.Location = new Point(776, 410);
            txtSalePrice.Name = "txtSalePrice";
            txtSalePrice.Size = new Size(125, 27);
            txtSalePrice.TabIndex = 12;
            // 
            // CreateQuarterlyReportForm
            // 
            ClientSize = new Size(1057, 527);
            Controls.Add(txtSalePrice);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(l1);
            Controls.Add(txtPurchasePrice);
            Controls.Add(label1);
            Controls.Add(Open_Analis);
            Controls.Add(ExportBTN);
            Controls.Add(dataGridView1);
            Controls.Add(btnOpen);
            Controls.Add(btnCreate);
            Controls.Add(cmbYear);
            Controls.Add(cmbQuarter);
            Name = "CreateQuarterlyReportForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Квартальные отчёты";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ComboBox cmbQuarter;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Button ExportBTN;
        private Button Open_Analis;
        private Label label1;
        private TextBox txtPurchasePrice;
        private Label l1;
        private Label label3;
        private Label label2;
        private TextBox txtSalePrice;
    }
}