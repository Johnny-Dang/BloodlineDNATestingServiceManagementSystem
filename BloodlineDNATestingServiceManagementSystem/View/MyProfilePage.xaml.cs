using System;
using System.Windows;
using System.Windows.Controls;
using DAL.Entities;
using BLL.Services;

namespace BloodlineDNATestingServiceManagementSystem.View
{
    public partial class MyProfilePage : UserControl
    {
        private User _currentUser;
        private readonly UserService _userService;

        public MyProfilePage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _userService = new UserService();
            LoadProfile();
        }

        private void LoadProfile()
        {
            txtFullName.Text = _currentUser.FullName;
            txtEmail.Text = _currentUser.Email;
            txtPhoneNumber.Text = _currentUser.PhoneNumber;
            txtAddress.Text = _currentUser.Address;
            txtRole.Text = _currentUser.Role?.RoleName ?? "";
            txtStatus.Text = _currentUser.Status;
            txtPassword.Password = _currentUser.Password;

            if (_currentUser.DateOfBirth != null)
                dpDateOfBirth.SelectedDate = DateTime.Parse(_currentUser.DateOfBirth.ToString());

            cbGender.SelectedIndex = _currentUser.Gender?.ToLower() switch
            {
                "nam" => 0,
                "nữ" => 1,
                "khác" => 2,
                _ => -1
            };
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            _currentUser.FullName = txtFullName.Text;
            _currentUser.Email = txtEmail.Text;
            _currentUser.PhoneNumber = txtPhoneNumber.Text;
            _currentUser.Address = txtAddress.Text;
            _currentUser.Password = txtPassword.Password;

            if (dpDateOfBirth.SelectedDate != null)
                _currentUser.DateOfBirth = DateOnly.FromDateTime(dpDateOfBirth.SelectedDate.Value);

            if (cbGender.SelectedItem is ComboBoxItem selectedGender)
                _currentUser.Gender = selectedGender.Content.ToString();

            try
            {
                _userService.UpdateUser(_currentUser);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
