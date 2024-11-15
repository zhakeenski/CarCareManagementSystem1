using System;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace CarCareManagementSystem1
{
    public class MainForm : MetroForm
    {
        private Button btnTasks, btnProgress, btnInventory, btnProfile;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Настройки формы
            this.Text = "CarCare Service Center";
            this.Size = new Size(800, 600);
            this.Style = MetroFramework.MetroColorStyle.Blue;
            this.Theme = MetroFramework.MetroThemeStyle.Light;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "Welcome to CarCare Service Center",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(200, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Кнопка "Tasks"
            btnTasks = CreateButton("Tasks", new Point(100, 100), BtnTasks_Click);
            this.Controls.Add(btnTasks);

            // Кнопка "Progress"
            btnProgress = CreateButton("Progress", new Point(100, 160), BtnProgress_Click);
            this.Controls.Add(btnProgress);

            // Кнопка "Inventory"
            btnInventory = CreateButton("Inventory", new Point(100, 220), BtnInventory_Click);
            this.Controls.Add(btnInventory);

            // Кнопка "Profile"
            btnProfile = CreateButton("Profile", new Point(100, 280), BtnProfile_Click);
            this.Controls.Add(btnProfile);
        }

        private Button CreateButton(string text, Point location, EventHandler onClick)
        {
            Button button = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(200, 40),
                BackColor = Color.LightSteelBlue,
                FlatStyle = FlatStyle.Flat
            };
            button.Click += onClick;
            return button;
        }

        private void BtnTasks_Click(object sender, EventArgs e)
        {
            ViewTasksForm tasksForm = new ViewTasksForm();
            tasksForm.Show();
        }

        private void BtnProgress_Click(object sender, EventArgs e)
        {
            RecordProgressForm progressForm = new RecordProgressForm();
            progressForm.Show();
        }

        private void BtnInventory_Click(object sender, EventArgs e)
        {
            ManageInventoryForm inventoryForm = new ManageInventoryForm();
            inventoryForm.Show();
        }

        private void BtnProfile_Click(object sender, EventArgs e)
        {
            UpdateProfileForm profileForm = new UpdateProfileForm();
            profileForm.Show();
        }
    }
}
