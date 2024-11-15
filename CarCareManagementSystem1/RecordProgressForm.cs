using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace CarCareManagementSystem1
{
    public class RecordProgressForm : MetroForm
    {
        private TextBox txtTaskID, txtStatus, txtCompletionTime, txtComments;
        private Button btnUpdateProgress;
        private DataGridView dgvProgressLog;

        private string connectionString = "Data Source=KZ070BRO12;Initial Catalog=CarCareDB;Integrated Security=True";

        public RecordProgressForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Настройки формы
            this.Text = "Record Task Progress";
            this.Size = new Size(800, 600);
            this.Style = MetroFramework.MetroColorStyle.Orange;

            // Поля ввода
            txtTaskID = CreateTextBox("Task ID", new Point(20, 60));
            txtStatus = CreateTextBox("Status (e.g., Completed)", new Point(20, 100));
            txtCompletionTime = CreateTextBox("Completion Time (YYYY-MM-DD HH:MM:SS)", new Point(20, 140));
            txtComments = CreateTextBox("Additional Comments", new Point(20, 180));
            this.Controls.AddRange(new Control[] { txtTaskID, txtStatus, txtCompletionTime, txtComments });

            // Кнопка обновления
            btnUpdateProgress = new Button
            {
                Text = "Update Progress",
                Location = new Point(20, 220),
                Size = new Size(150, 30),
                BackColor = Color.LightSteelBlue
            };
            btnUpdateProgress.Click += BtnUpdateProgress_Click;
            this.Controls.Add(btnUpdateProgress);

            // Таблица логов
            dgvProgressLog = new DataGridView
            {
                Location = new Point(20, 270),
                Size = new Size(750, 300),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvProgressLog);

            // Загрузка логов при открытии
            LoadProgressLog();
        }

        private TextBox CreateTextBox(string placeholder, Point location)
        {
            TextBox textBox = new TextBox
            {
                Location = location,
                Size = new Size(300, 30),
                ForeColor = Color.Gray,
                Text = placeholder
            };

            textBox.GotFocus += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };

            return textBox;
        }

        private void BtnUpdateProgress_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE Tasks SET Status = @Status, CompletionTime = @CompletionTime, Comments = @Comments WHERE ID = @TaskID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Status", txtStatus.Text);
                    command.Parameters.AddWithValue("@CompletionTime", txtCompletionTime.Text);
                    command.Parameters.AddWithValue("@Comments", txtComments.Text);
                    command.Parameters.AddWithValue("@TaskID", int.Parse(txtTaskID.Text));

                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected > 0 ? "Progress updated successfully!" : "Task not found.");

                    // Логируем изменения
                    LogProgressUpdate(int.Parse(txtTaskID.Text), txtStatus.Text);
                    LoadProgressLog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating progress: {ex.Message}");
                }
            }
        }

        private void LogProgressUpdate(int taskId, string progressDescription)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO ProgressLog (TaskID, UpdateTime, ProgressDescription) VALUES (@TaskID, GETDATE(), @ProgressDescription)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TaskID", taskId);
                    command.Parameters.AddWithValue("@ProgressDescription", progressDescription);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error logging progress: {ex.Message}");
                }
            }
        }

        private void LoadProgressLog()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT TaskID, UpdateTime, ProgressDescription FROM ProgressLog";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvProgressLog.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading progress log: {ex.Message}");
                }
            }
        }
    }
}
