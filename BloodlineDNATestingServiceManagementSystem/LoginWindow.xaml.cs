using BLL.Services;
using DAL.Repositories;
using System.Windows;

namespace BloodlineDNATestingServiceManagementSystem
{
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService;

        public LoginWindow()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text.Trim();
            var password = txtPassword.Text;
            var user = _userService.Authenticate(email, password);

            if (user != null)
            {
                SessionManager.CurrentUser = user;

                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                // Hiện lỗi ngay dưới nút đăng nhập
                var errorLabel = this.FindName("ErrorLabel") as System.Windows.Controls.Label;
                if (errorLabel != null)
                {
                    errorLabel.Content = "Email hoặc mật khẩu không đúng hoặc tài khoản bị khóa.";
                    errorLabel.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
