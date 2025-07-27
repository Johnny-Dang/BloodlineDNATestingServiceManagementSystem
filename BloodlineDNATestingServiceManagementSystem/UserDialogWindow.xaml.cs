using System.Windows;
using DAL.Entities;
using DAL.Repositories;
using System.Linq;
using System.Windows.Controls;

namespace BloodlineDNATestingServiceManagementSystem.View
{
    public partial class UserDialogWindow : Window
    {
        public User User { get; private set; }
        private readonly RoleRepository _roleRepo = new RoleRepository();

        public UserDialogWindow(User? user = null)
        {
            InitializeComponent();
            cbRole.ItemsSource = _roleRepo.GetAllRoles();

            if (user != null)
            {
                User = user;
                txtFullName.Text = user.FullName;
                txtEmail.Text = user.Email;
                txtPhone.Text = user.PhoneNumber;
                cbStatus.SelectedItem = cbStatus.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == user.Status);
                cbRole.SelectedValue = user.RoleId;
            }
            else
            {
                User = new User();
                cbStatus.SelectedIndex = 0;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            User.FullName = txtFullName.Text;
            User.Email = txtEmail.Text;
            User.PhoneNumber = txtPhone.Text;
            User.Password = txtPassword.Password; 
            User.Status = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            User.RoleId = (int)cbRole.SelectedValue;
            DialogResult = true;
            Close();
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
