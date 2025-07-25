using System;
using System.Windows;
using System.Windows.Controls;

namespace BloodlineDNATestingServiceManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            var user = SessionManager.CurrentUser;
            if (user != null)
            {
                txtUserInfo.Text = $"Welcome, {user.FullName} ({user.Role.RoleName})";
            }
            else
            {
                MessageBox.Show("Session not found. Please login again.");
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void SidebarButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            if (button.Name == "btnBooking")
            {
                MainTabControl.SelectedItem = tabBooking;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.CurrentUser = null;
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}