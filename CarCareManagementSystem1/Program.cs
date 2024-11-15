using CarCareManagementSystem1;
using System;
using System.Windows.Forms;

namespace CarCareManagementSystem1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
