using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.Entities;

namespace BloodlineDNATestingServiceManagementSystem.View
{
    public partial class UserManagementPage : UserControl
    {
        private readonly UserService _userService;

        public UserManagementPage()
        {
            InitializeComponent();
            _userService = new UserService();
            LoadUserData();

        }

        private void LoadUserData()
        {
            var currentUser = SessionManager.CurrentUser;
            if (currentUser == null) return;

            string role = currentUser.Role.RoleName.ToLower();
            List<User> users = _userService.GetAllUsers();

            if (role == "admin")
            {
                dgUsers.ItemsSource = users;
            }
            else if (role == "manager")
            {
                dgUsers.ItemsSource = users
                    .Where(u => u.Role.RoleName.ToLower() == "staff" || u.Role.RoleName.ToLower() == "customer")
                    .ToList();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào trang này!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Visibility = Visibility.Collapsed;
            }
        }

        // Add to UserManagementPage.xaml.cs
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UserDialogWindow();
            if (dialog.ShowDialog() == true)
            {
                _userService.CreateUser(dialog.User);
                LoadUserData();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = dgUsers.SelectedItem as User;
            if (selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn User để sửa.");
                return;
            }
            var dialog = new UserDialogWindow(selectedUser);
            if (dialog.ShowDialog() == true)
            {
                _userService.UpdateUser(dialog.User);
                LoadUserData();
            }
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = dgUsers.SelectedItem as User;
            if (selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn User để xóa.");
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa user: {selectedUser.FullName}?", "Xác nhận", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _userService.DeleteUser(selectedUser.UserId);
                LoadUserData();
            }
        }
    }
}
