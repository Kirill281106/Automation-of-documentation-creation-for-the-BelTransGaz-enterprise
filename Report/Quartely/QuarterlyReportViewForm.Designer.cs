namespace вход_курсач
{
    partial class QuarterlyReportViewForm
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
            btnExportWord = new Button();
            listBoxReport = new ListBox();
            SuspendLayout();
            // 
            // btnExportWord
            // 
            btnExportWord.Location = new Point(29, 387);
            btnExportWord.Name = "btnExportWord";
            btnExportWord.Size = new Size(194, 29);
            btnExportWord.TabIndex = 1;
            btnExportWord.Text = "Импортировать в Word";
            btnExportWord.UseVisualStyleBackColor = true;
            btnExportWord.Click += btnExportWord_Click;
            // 
            // listBoxReport
            // 
            listBoxReport.FormattingEnabled = true;
            listBoxReport.Location = new Point(12, 12);
            listBoxReport.Name = "listBoxReport";
            listBoxReport.Size = new Size(685, 364);
            listBoxReport.TabIndex = 2;
            // 
            // QuarterlyReportViewForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(listBoxReport);
            Controls.Add(btnExportWord);
            Name = "QuarterlyReportViewForm";
            Text = "QuarterlyReportViewForm";
            ResumeLayout(false);
        }

        #endregion
        private Button btnExportWord;
        private ListBox listBoxReport;
    }
}