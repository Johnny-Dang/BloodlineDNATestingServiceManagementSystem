using BloodlineDNATestingServiceManagementSystem.Controls;
using System;
using System.Linq;
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
                return;
            }
            
            // Mặc định mở tab quản lý đặt lịch
            TabItem bookingTab = MainTabControl.Items[0] as TabItem;
            bookingTab.Content = new BookingControl();
            MainTabControl.SelectedIndex = 0;
        }

        private void SidebarButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;
            
            // Kiểm tra phiên đăng nhập trước khi thực hiện thao tác
            if (SessionManager.CurrentUser == null)
            {
                ShowLoginRequiredMessage();
                return;
            }

            if (button.Name == "btnBooking")
            {
                TabItem tabItem = FindOrCreateTab("tabBooking", "Quản lý đặt lịch");
                tabItem.Content = new BookingControl();
                MainTabControl.SelectedItem = tabItem;
            }
            else if (button.Name == "btnConsultation")
            {
                TabItem tabItem = FindOrCreateTab("tabConsultation", "Quản lý tư vấn");
                tabItem.Content = new ConsultationControl();
                MainTabControl.SelectedItem = tabItem;
            }
        }

        private TabItem FindOrCreateTab(string tabName, string header)
        {
            // Kiểm tra xem tab đã tồn tại chưa
            if (MainTabControl.Items.Cast<TabItem>().Any(t => t.Name == tabName))
            {
                return MainTabControl.Items.Cast<TabItem>().First(t => t.Name == tabName);
            }
            
            // Nếu chưa có, tạo tab mới
            var newTab = new TabItem
            {
                Name = tabName,
                Header = header
            };
            
            MainTabControl.Items.Add(newTab);
            return newTab;
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            Window window = null;
            
            switch (button.Name)
            {
                case "btnTestResult":
                    window = new TestResultWindow();
                    break;
                case "btnDetailResult":
                    window = new DetailResultWindow();
                    break;
                case "btnFeedback":
                    window = new FeedBackWindow();
                    break;
            }

            if (window != null)
            {
                window.Show();
                this.Close();
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.CurrentUser = null;
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        
        private void ShowLoginRequiredMessage()
        {
            MessageBox.Show("Vui lòng đăng nhập trước!", "Yêu cầu đăng nhập", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close(); 
        }
    }
}