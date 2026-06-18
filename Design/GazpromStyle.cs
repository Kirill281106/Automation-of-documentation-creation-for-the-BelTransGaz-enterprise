using System;
using System.Windows.Forms;

namespace вход_курсач.Design
{
    public static class GazpromStyle
    {
        // Газпром цвета
        public static readonly Color PrimaryBlue = Color.FromArgb(0, 102, 204);      // #0066CC - основной синий
        public static readonly Color DarkBlue = Color.FromArgb(0, 51, 153);          // #003399 - тёмный синий
        public static readonly Color LightBlue = Color.FromArgb(230, 242, 255);      // Светлый синий фон
        public static readonly Color DarkGray = Color.FromArgb(51, 51, 51);          // #333333 - тёмно-серый текст
        public static readonly Color MediumGray = Color.FromArgb(128, 128, 128);     // Средний серый
        public static readonly Color LightGray = Color.FromArgb(240, 240, 240);      // Светлый серый фон

        /// <summary>
        /// Применяет стиль кнопки Газпрома
        /// </summary>
        public static void StyleButton(Button button)
        {
            button.BackColor = PrimaryBlue;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = DarkBlue;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 80, 160);
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Применяет стиль текстового поля
        /// </summary>
        public static void StyleTextBox(TextBox textBox)
        {
            textBox.BackColor = Color.White;
            textBox.ForeColor = DarkGray;
            textBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Применяет стиль формы
        /// </summary>
        public static void StyleForm(Form form)
        {
            form.BackColor = LightGray;
            form.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            form.ForeColor = DarkGray;
        }

        /// <summary>
        /// Применяет стиль к ComboBox
        /// </summary>
        public static void StyleComboBox(ComboBox comboBox)
        {
            comboBox.BackColor = Color.White;
            comboBox.ForeColor = DarkGray;
            comboBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        }
        /// <summary>
        /// Применяет стиль к Label
        /// </summary>
        public static void StyleLabel(Label label)
        {
            label.ForeColor = DarkGray;
            label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            label.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Применяет стиль к DataGridView
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.ForeColor = DarkGray;
            dgv.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = DarkBlue;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgv.RowHeadersDefaultCellStyle.BackColor = LightGray;
            dgv.RowHeadersDefaultCellStyle.ForeColor = DarkGray;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = LightBlue;
            dgv.GridColor = Color.FromArgb(200, 200, 200);
            dgv.EnableHeadersVisualStyles = false;
        }

        /// <summary>
        /// Применяет стиль к LinkLabel
        /// </summary>
        public static void StyleLinkLabel(LinkLabel linkLabel)
        {
            linkLabel.LinkColor = PrimaryBlue;
            linkLabel.VisitedLinkColor = DarkBlue;
            linkLabel.ActiveLinkColor = DarkBlue;
            linkLabel.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            linkLabel.ForeColor = DarkGray;
        }

        /// <summary>
        /// Применяет стиль к GroupBox
        /// </summary>
        public static void StyleGroupBox(GroupBox groupBox)
        {
            groupBox.ForeColor = DarkGray;
            groupBox.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            groupBox.BackColor = LightGray;
        }

        /// <summary>
        /// Применяет стиль к Panel
        /// </summary>
        public static void StylePanel(Panel panel, bool useBlueBackground = false)
        {
            panel.BackColor = useBlueBackground ? LightBlue : LightGray;
        }

        /// <summary>
        /// Создаёт заголовок с синим фоном
        /// </summary>
        public static Panel CreateHeaderPanel(string title)
        {
            var panel = new Panel
            {
                BackColor = PrimaryBlue,
                Height = 50,
                Dock = DockStyle.Top
            };

            var label = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Dock = DockStyle.Fill
            };

            panel.Controls.Add(label);
            return panel;
        }
    }
}
