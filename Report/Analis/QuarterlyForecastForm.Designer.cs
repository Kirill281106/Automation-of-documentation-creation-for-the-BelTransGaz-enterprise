namespace вход_курсач
{
    partial class QuarterlyForecastForm
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
            pbChart = new PictureBox();
            lblForecast = new Label();
            lblTrend = new Label();
            ((System.ComponentModel.ISupportInitialize)pbChart).BeginInit();
            SuspendLayout();
            // 
            // pbChart
            // 
            pbChart.Location = new Point(12, 12);
            pbChart.Name = "pbChart";
            pbChart.Size = new Size(808, 279);
            pbChart.TabIndex = 0;
            pbChart.TabStop = false;
            pbChart.Paint += pbChart_Paint;
            // 
            // lblForecast
            // 
            lblForecast.AutoSize = true;
            lblForecast.Location = new Point(12, 319);
            lblForecast.Name = "lblForecast";
            lblForecast.Size = new Size(50, 20);
            lblForecast.TabIndex = 1;
            lblForecast.Text = "label1";
            // 
            // lblTrend
            // 
            lblTrend.AutoSize = true;
            lblTrend.Location = new Point(12, 374);
            lblTrend.Name = "lblTrend";
            lblTrend.Size = new Size(50, 20);
            lblTrend.TabIndex = 2;
            lblTrend.Text = "label1";
            // 
            // QuarterlyForecastForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(903, 421);
            Controls.Add(lblTrend);
            Controls.Add(lblForecast);
            Controls.Add(pbChart);
            Name = "QuarterlyForecastForm";
            Text = "QuarterlyForecastForm";
            ((System.ComponentModel.ISupportInitialize)pbChart).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbChart;
        private Label lblForecast;
        private Label lblTrend;
    }
}