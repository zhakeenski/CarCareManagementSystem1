using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace CarCareManagementSystem1
{
    public class ManageInventoryForm : MetroForm
    {
        private DataGridView dgvInventory;
        private TextBox txtPartName, txtQuantity;
        private Button btnRequestParts, btnUseParts, btnCheckShortage;

        private string connectionString = "Data Source=KZ070BRO12;Initial Catalog=CarCareDB;Integrated Security=True";

        public ManageInventoryForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Настройки формы
            this.Text = "Manage Inventory";
            this.Size = new Size(800, 600);
            this.Style = MetroFramework.MetroColorStyle.Red;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "Manage Parts Inventory",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Таблица инвентаря
            dgvInventory = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(750, 300),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvInventory);

            // Поля ввода
            txtPartName = CreateTextBox("Part Name", new Point(20, 380));
            txtQuantity = CreateTextBox("Quantity", new Point(250, 380));
            this.Controls.AddRange(new Control[] { txtPartName, txtQuantity });

            // Кнопка "Request Parts"
            btnRequestParts = CreateButton("Request Parts", new Point(20, 420), BtnRequestParts_Click);
            this.Controls.Add(btnRequestParts);

            // Кнопка "Use Parts"
            btnUseParts = CreateButton("Use Parts", new Point(200, 420), BtnUseParts_Click);
            this.Controls.Add(btnUseParts);

            // Кнопка "Check Shortage"
            btnCheckShortage = CreateButton("Check Shortage", new Point(380, 420), BtnCheckShortage_Click);
            this.Controls.Add(btnCheckShortage);

            // Загрузка данных инвентаря
            LoadInventoryData();
        }

        private TextBox CreateTextBox(string placeholder, Point location)
        {
            TextBox textBox = new TextBox
            {
                Location = location,
                Size = new Size(200, 30),
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

        private Button CreateButton(string text, Point location, EventHandler onClick)
        {
            Button button = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(150, 30),
                BackColor = Color.LightSteelBlue,
                FlatStyle = FlatStyle.Flat
            };
            button.Click += onClick;
            return button;
        }

        private void LoadInventoryData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM PartsInventory";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvInventory.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading inventory data: {ex.Message}");
                }
            }
        }

        private void BtnRequestParts_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO PartsInventory (PartName, Quantity, Price) VALUES (@PartName, @Quantity, 0)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PartName", txtPartName.Text);
                    command.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text));

                    command.ExecuteNonQuery();
                    MessageBox.Show("Parts requested successfully!");
                    LoadInventoryData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error requesting parts: {ex.Message}");
                }
            }
        }

        private void BtnUseParts_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE PartsInventory SET Quantity = Quantity - @Quantity WHERE PartName = @PartName";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PartName", txtPartName.Text);
                    command.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text));

                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected > 0 ? "Parts used successfully!" : "Part not found.");
                    LoadInventoryData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error using parts: {ex.Message}");
                }
            }
        }

        private void BtnCheckShortage_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT PartName, Quantity FROM PartsInventory WHERE Quantity < 5";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvInventory.DataSource = table;
                    MessageBox.Show("Showing parts with low stock.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error checking shortage: {ex.Message}");
                }
            }
        }
    }
}
