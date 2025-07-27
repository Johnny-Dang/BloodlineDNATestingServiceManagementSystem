using BLL.Services;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BloodlineDNATestingServiceManagementSystem.Controls
{
    public partial class ConsultationControl : UserControl
    {
        private readonly ConsultantService _consultantService;
        
        // Các hằng số xác định vai trò người dùng
        private const string ROLE_CUSTOMER = "CUSTOMER";
        private const string ROLE_MANAGER = "MANAGER";
        private const string ROLE_STAFF = "STAFF";
        private const string ROLE_ADMIN = "ADMIN";

        public ConsultationControl()
        {
            InitializeComponent();
            
            // Khởi tạo các service
            _consultantService = new ConsultantService();
            
            // Kiểm tra quyền người dùng và hiển thị giao diện tương ứng
            var user = SessionManager.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("Vui lòng đăng nhập trước!", "Lỗi xác thực", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Thiết lập dữ liệu và giao diện
            LoadData();
            SetUIBasedOnRole();
            
            // Đặt ngày mặc định cho trường ngày tư vấn là ngày hiện tại + 1 ngày
            txtConsultDate.Text = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
        }

        // Tải dữ liệu dựa trên vai trò người dùng
        private void LoadData()
        {
            string roleName = SessionManager.CurrentUser.Role.RoleName;
            
            if (roleName == ROLE_CUSTOMER)
            {
                // Khách hàng chỉ xem các yêu cầu tư vấn của họ
                dgConsultations.ItemsSource = _consultantService.GetByUserId(SessionManager.CurrentUser.UserId);
            }
            else
            {
                // Vai trò khác xem tất cả yêu cầu tư vấn
                dgConsultations.ItemsSource = _consultantService.GetAll();
            }
        }

        // Thiết lập giao diện dựa trên vai trò
        private void SetUIBasedOnRole()
        {
            string roleName = SessionManager.CurrentUser.Role.RoleName;
            
            switch (roleName)
            {
                case ROLE_ADMIN:
                    // Admin có thể làm mọi thứ
                    btnCreate.Visibility = Visibility.Visible;
                    btnUpdate.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Visible;
                    SetAllInputsEnabled(true);
                    break;
                    
                case ROLE_MANAGER:
                case ROLE_STAFF:
                    // Manager và Staff chỉ có thể xem và cập nhật
                    btnCreate.Visibility = Visibility.Visible; // Cho phép thêm mới
                    btnUpdate.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Collapsed;
                    // Cho phép nhập tất cả trường
                    SetAllInputsEnabled(true);
                    break;
                    
                case ROLE_CUSTOMER:
                    // Customer có thể tạo yêu cầu mới và xem các yêu cầu của họ
                    btnCreate.Visibility = Visibility.Visible;
                    btnUpdate.Visibility = Visibility.Collapsed;
                    btnDelete.Visibility = Visibility.Collapsed;
                    SetAllInputsEnabled(true);
                    // Khách hàng không được chỉnh sửa trạng thái và ghi chú
                    cbxStatus.IsEnabled = false;
                    txtNotes.IsEnabled = false;
                    break;
            }
            
            // Thiết lập các option cho trạng thái
            SetStatusOptions();
        }
        
        private void SetAllInputsEnabled(bool isEnabled)
        {
            txtName.IsEnabled = isEnabled;
            txtPhone.IsEnabled = isEnabled;
            txtType.IsEnabled = isEnabled;
            txtConsultDate.IsEnabled = isEnabled;
            txtContent.IsEnabled = isEnabled;
            txtNotes.IsEnabled = isEnabled;
            cbxStatus.IsEnabled = isEnabled;
        }
        
        private void SetStatusOptions()
        {
            cbxStatus.Items.Clear();
            
            // Các trạng thái có thể có
            cbxStatus.Items.Add("Chờ xử lý");
            cbxStatus.Items.Add("Đang xử lý");
            cbxStatus.Items.Add("Đã phản hồi");
            cbxStatus.Items.Add("Đã đóng");
            cbxStatus.Items.Add("Hủy bỏ");
            cbxStatus.SelectedIndex = 0;
            
            // Nếu là khách hàng, không cho phép thay đổi trạng thái
            if (SessionManager.CurrentUser.Role.RoleName == ROLE_CUSTOMER)
            {
                cbxStatus.IsEnabled = false;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhone.Focus();
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(txtType.Text))
            {
                MessageBox.Show("Vui lòng nhập loại tư vấn.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtType.Focus();
                return false;
            }

            if (!DateOnly.TryParse(txtConsultDate.Text, out _))
            {
                MessageBox.Show("Ngày tư vấn không hợp lệ. Vui lòng nhập theo định dạng MM/DD/YYYY.", 
                               "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtConsultDate.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtContent.Text))
            {
                MessageBox.Show("Vui lòng nhập nội dung tư vấn.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtContent.Focus();
                return false;
            }

            return true;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra quyền
            string roleName = SessionManager.CurrentUser.Role.RoleName;
            if (roleName != ROLE_CUSTOMER && roleName != ROLE_ADMIN && roleName != ROLE_MANAGER && roleName != ROLE_STAFF)
            {
                MessageBox.Show("Bạn không có quyền tạo yêu cầu tư vấn mới.", "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra dữ liệu
            if (!ValidateForm()) return;

            try
            {
                // Tạo đối tượng tư vấn mới
                var consultant = new Consultant
                {
                    UserId = SessionManager.CurrentUser.UserId,
                    Name = txtName.Text,
                    Phone = txtPhone.Text,
                    Type = txtType.Text,
                    Content = txtContent.Text,
                    ConsultantDate = DateOnly.Parse(txtConsultDate.Text),
                    Notes = txtNotes.Text,
                    Status = cbxStatus.SelectedItem.ToString(),
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                };

                // Lưu vào database
                _consultantService.Add(consultant);
                
                MessageBox.Show("Tạo yêu cầu tư vấn thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Tải lại dữ liệu
                LoadData();
                
                // Xóa form
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo yêu cầu tư vấn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra consultation được chọn
            if (dgConsultations.SelectedItem is not Consultant selectedConsultant)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu tư vấn để cập nhật.", "Chưa chọn", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string roleName = SessionManager.CurrentUser.Role.RoleName;
            
            try
            {
                // Xử lý theo vai trò
                switch (roleName)
                {
                    case ROLE_ADMIN:
                    case ROLE_MANAGER:
                    case ROLE_STAFF:
                        // Admin, Manager, Staff có thể cập nhật mọi thông tin
                        if (!ValidateForm()) return;
                        
                        // Cập nhật thông tin tư vấn
                        var updatedConsultant = new Consultant
                        {
                            ConsultantId = selectedConsultant.ConsultantId,
                            UserId = selectedConsultant.UserId,
                            Name = txtName.Text,
                            Phone = txtPhone.Text,
                            Type = txtType.Text,
                            Content = txtContent.Text,
                            ConsultantDate = DateOnly.Parse(txtConsultDate.Text),
                            Notes = txtNotes.Text,
                            Status = cbxStatus.SelectedItem.ToString(),
                            CreatedDate = selectedConsultant.CreatedDate
                        };
                        
                        _consultantService.Update(updatedConsultant);
                        MessageBox.Show("Cập nhật thông tin tư vấn thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                        break;
                        
                    default:
                        MessageBox.Show("Bạn không có quyền cập nhật yêu cầu tư vấn.", "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra consultation được chọn
            if (dgConsultations.SelectedItem is not Consultant selectedConsultant)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu tư vấn để xóa.", "Chưa chọn", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Chỉ admin có quyền xóa
            if (SessionManager.CurrentUser.Role.RoleName != ROLE_ADMIN)
            {
                MessageBox.Show("Chỉ quản trị viên mới có quyền xóa yêu cầu tư vấn.", "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa yêu cầu tư vấn ID {selectedConsultant.ConsultantId}?", 
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    
                if (result == MessageBoxResult.Yes)
                {
                    _consultantService.Delete(selectedConsultant.ConsultantId);
                    MessageBox.Show("Xóa yêu cầu tư vấn thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            string roleName = SessionManager.CurrentUser?.Role.RoleName;
            
            try
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadData(); // Tải lại tất cả dữ liệu
                    return;
                }

                IEnumerable<Consultant> searchResults;
                
                if (roleName == ROLE_CUSTOMER)
                {
                    // Customer chỉ tìm kiếm trong các yêu cầu tư vấn của mình
                    searchResults = _consultantService.Search(searchTerm)
                        .Where(c => c.UserId == SessionManager.CurrentUser.UserId);
                }
                else
                {
                    // Các vai trò khác tìm trong tất cả yêu cầu tư vấn
                    searchResults = _consultantService.Search(searchTerm);
                }
                
                dgConsultations.ItemsSource = searchResults;
                
                if (!searchResults.Any())
                {
                    MessageBox.Show("Không tìm thấy kết quả nào phù hợp.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetForm()
        {
            txtName.Text = "";
            txtPhone.Text = "";
            txtType.Text = "";
            txtConsultDate.Text = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
            txtContent.Text = "";
            txtNotes.Text = "";
            
            // Reset status combo box
            if (cbxStatus.Items.Count > 0)
                cbxStatus.SelectedIndex = 0;
            
            dgConsultations.SelectedItem = null;
        }

        private void dgConsultations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgConsultations.SelectedItem is Consultant selectedConsultant)
            {
                // Hiển thị thông tin consultation được chọn
                txtName.Text = selectedConsultant.Name;
                txtPhone.Text = selectedConsultant.Phone;
                txtType.Text = selectedConsultant.Type;
                txtConsultDate.Text = selectedConsultant.ConsultantDate.ToString("MM/dd/yyyy");
                txtContent.Text = selectedConsultant.Content;
                txtNotes.Text = selectedConsultant.Notes;
                
                // Chọn đúng trạng thái
                for (int i = 0; i < cbxStatus.Items.Count; i++)
                {
                    if (cbxStatus.Items[i].ToString() == selectedConsultant.Status)
                    {
                        cbxStatus.SelectedIndex = i;
                        break;
                    }
                }

                string roleName = SessionManager.CurrentUser?.Role.RoleName;
                
                // Kiểm tra quyền và hiển thị tương ứng
                if (roleName == ROLE_CUSTOMER)
                {
                    // Customer chỉ xem, không được sửa
                    SetAllInputsEnabled(false);
                }
                else if (roleName == ROLE_STAFF || roleName == ROLE_MANAGER)
                {
                    // Staff và Manager được chỉnh sửa tất cả trường
                    SetAllInputsEnabled(true);
                }
                else if (roleName == ROLE_ADMIN)
                {
                    // Admin được chỉnh sửa tất cả và có thể xóa
                    SetAllInputsEnabled(true);
                }
            }
        }
    }
}