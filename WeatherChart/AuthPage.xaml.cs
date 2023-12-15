using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WeatherChart
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Window
    {
        public bool DialogResult
        {
            get; set;
        }

        public bool isAdmin = false;
        public AuthPage()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (username == "admin" && password == "123")
            {
                DialogResult = true;
                isAdmin = true;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                mainWindow.SetAdminStatus(isAdmin);
                Close();
            }
            else
            {
                DialogResult = true;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }
    }
}
