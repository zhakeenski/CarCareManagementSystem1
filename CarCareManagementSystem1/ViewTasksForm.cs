using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace CarCareManagementSystem1
{
    public class ViewTasksForm : MetroForm
    {
        private DataGridView dgvTasks;
        private Button btnLoadTasks;

        private string connectionString = "Data Source=KZ070BRO12;Initial Catalog=CarCareDB;Integrated Security=True";

        public ViewTasksForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Настройки формы
            this.Text = "View Tasks";
            this.Size = new Size(800, 600);
            this.Style = MetroFramework.MetroColorStyle.Green;

            // Таблица задач
            dgvTasks = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(750, 400),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvTasks);

            // Кнопка "Load Tasks"
            btnLoadTasks = new Button
            {
                Text = "Load Tasks",
                Location = new Point(20, 480),
                Size = new Size(150, 30),
                BackColor = Color.LightSteelBlue
            };
            btnLoadTasks.Click += BtnLoadTasks_Click;
            this.Controls.Add(btnLoadTasks);
        }

        private void BtnLoadTasks_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Tasks";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvTasks.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading tasks: {ex.Message}");
                }
            }
        }
    }
}
