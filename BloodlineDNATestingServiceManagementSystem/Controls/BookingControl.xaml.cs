using BLL.Services;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BloodlineDNATestingServiceManagementSystem.Controls
{
    public partial class BookingControl : UserControl
    {
        private readonly BookingService _bookingService;
        private readonly ServiceService _serviceService;
        
        private const string ROLE_CUSTOMER = "CUSTOMER";
        private const string ROLE_MANAGER = "MANAGER";
        private const string ROLE_STAFF = "STAFF";
        private const string ROLE_ADMIN = "ADMIN";

        public BookingControl()
        {
            InitializeComponent();
            
            _bookingService = new BookingService();
            _serviceService = new ServiceService();
            
            var user = SessionManager.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("Vui lòng đăng nhập trước!", "Lỗi xác thực", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadData();
            LoadServices();
            SetUIBasedOnRole();
            
            txtAppointmentDate.Text = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
            txtNumberSample.Text = "1";
        }

        private void LoadData()
        {
            string roleName = SessionManager.CurrentUser.Role.RoleName;
            
            if (roleName == ROLE_CUSTOMER)
            {
                dgBookings.ItemsSource = _bookingService.GetByUserId(SessionManager.CurrentUser.UserId);
            }
            else
            {
                dgBookings.ItemsSource = _bookingService.GetAll();
            }
        }

        private void LoadServices()
        {
            cbxServiceId.ItemsSource = _serviceService.GetAll();
            cbxServiceId.DisplayMemberPath = "ServiceName";
            cbxServiceId.SelectedValuePath = "ServiceId";
        }

        private void SetUIBasedOnRole()
        {
            string roleName = SessionManager.CurrentUser.Role.RoleName;
            
            switch(roleName)
            {
                case ROLE_ADMIN:
                    btnCreate.Visibility = Visibility.Visible;
                    btnUpdate.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Visible;
                    SetAllInputsEnabled(true);
                    break;
                    
                case ROLE_MANAGER:
                    btnCreate.Visibility = Visibility.Visible;
                    btnUpdate.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Collapsed;
                    SetAllInputsEnabled(false);
                    cbxStatus.IsEnabled = true;
                    break;
                    
                case ROLE_STAFF:
                    btnCreate.Visibility = Visibility.Visible;
                    btnUpdate.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Collapsed;
                    SetAllInputsEnabled(false);
                    cbxStatus.IsEnabled = true;
                    break;
                    
                case ROLE_CUSTOMER:
                    btnCreate.Visibility = Visibility.Visible;
                    btnUpdate.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Collapsed;
                    SetAllInputsEnabled(true);
                    cbxStatus.IsEnabled = false;
                    break;
            }
            
            SetStatusOptions();
        }
        
        private void SetAllInputsEnabled(bool isEnabled)
        {
            cbxServiceId.IsEnabled = isEnabled;
            txtNumberSample.IsEnabled = isEnabled;
            txtAppointmentDate.IsEnabled = isEnabled;
            txtNote.IsEnabled = isEnabled;
        }
        
        private void SetStatusOptions()
        {
            cbxStatus.Items.Clear();
            
            cbxStatus.Items.Add("Chờ xác nhận");
            cbxStatus.Items.Add("Đã xác nhận");
            cbxStatus.Items.Add("Đang xử lý");
            cbxStatus.Items.Add("Hoàn thành");
            cbxStatus.Items.Add("Hủy bỏ");
            cbxStatus.SelectedIndex = 0;
            
            if (SessionManager.CurrentUser.Role.RoleName == ROLE_CUSTOMER)
            {
                cbxStatus.IsEnabled = false;
            }
        }

        private bool ValidateForm()
        {
            if (cbxServiceId.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbxServiceId.Focus();
                return false;
            }

            if (!int.TryParse(txtNumberSample.Text, out int numberSample) || numberSample <= 0)
            {
                MessageBox.Show("Số lượng mẫu phải là số nguyên dương.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNumberSample.Focus();
                return false;
            }

            if (!DateOnly.TryParse(txtAppointmentDate.Text, out _))
            {
                MessageBox.Show("Ngày hẹn không hợp lệ. Vui lòng nhập theo định dạng MM/DD/YYYY.", 
                               "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAppointmentDate.Focus();
                return false;
            }

            return true;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            string roleName = SessionManager.CurrentUser.Role.RoleName;
            if (roleName != ROLE_CUSTOMER && roleName != ROLE_ADMIN && roleName != ROLE_STAFF && roleName != ROLE_MANAGER)
            {
                MessageBox.Show("Bạn không có quyền đặt lịch mới.", "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidateForm()) return;

            try
            {
                var booking = new Booking
                {
                    ServiceId = (int)cbxServiceId.SelectedValue,
                    UserId = SessionManager.CurrentUser.UserId,
                    NumberSample = int.Parse(txtNumberSample.Text),
                    BookingDate = DateOnly.FromDateTime(DateTime.Now),
                    AppointmentDate = DateOnly.Parse(txtAppointmentDate.Text),
                    Note = txtNote.Text,
                    Status = cbxStatus.SelectedItem.ToString(),
                    UpdateDate = DateOnly.FromDateTime(DateTime.Now),
                    UpdateBy = SessionManager.CurrentUser.UserId
                };

                Service selectedService = (Service)cbxServiceId.SelectedItem;
                booking.TotalPrice = selectedService.Price + ((booking.NumberSample - 1) * (selectedService.ExtraSampleFee ?? 0));

                _bookingService.Add(booking);
                
                MessageBox.Show("Đặt lịch thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                
                LoadData();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đặt lịch: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItem is not Booking selectedBooking)
            {
                MessageBox.Show("Vui lòng chọn một đặt lịch để cập nhật.", "Chưa chọn", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string roleName = SessionManager.CurrentUser.Role.RoleName;
            
            try
            {
                switch (roleName)
                {
                    case ROLE_ADMIN:
                    case ROLE_MANAGER:
                    case ROLE_STAFF:
                        if (!ValidateForm()) return;
                        
                        var updatedBooking = new Booking
                        {
                            BookingId = selectedBooking.BookingId,
                            UserId = selectedBooking.UserId,
                            ServiceId = (int)cbxServiceId.SelectedValue,
                            NumberSample = int.Parse(txtNumberSample.Text),
                            BookingDate = selectedBooking.BookingDate,
                            AppointmentDate = DateOnly.Parse(txtAppointmentDate.Text),
                            Note = txtNote.Text,
                            Status = cbxStatus.SelectedItem.ToString(),
                            UpdateDate = DateOnly.FromDateTime(DateTime.Now),
                            UpdateBy = SessionManager.CurrentUser.UserId
                        };
                        
                        Service selectedService = (Service)cbxServiceId.SelectedItem;
                        updatedBooking.TotalPrice = selectedService.Price + ((updatedBooking.NumberSample - 1) * (selectedService.ExtraSampleFee ?? 0));
                        
                        _bookingService.Update(updatedBooking);
                        MessageBox.Show("Cập nhật đặt lịch thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                        break;
                        
                    case ROLE_CUSTOMER:
                        if (selectedBooking.UserId != SessionManager.CurrentUser.UserId)
                        {
                            MessageBox.Show("Bạn chỉ có thể cập nhật đặt lịch của chính mình.", 
                                          "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        
                        if (selectedBooking.Status != "Chờ xác nhận")
                        {
                            MessageBox.Show("Bạn chỉ có thể cập nhật đặt lịch có trạng thái 'Chờ xác nhận'.",
                                          "Không thể cập nhật", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        
                        if (!ValidateForm()) return;
                        
                        var customerBooking = new Booking
                        {
                            BookingId = selectedBooking.BookingId,
                            UserId = selectedBooking.UserId,
                            ServiceId = (int)cbxServiceId.SelectedValue,
                            NumberSample = int.Parse(txtNumberSample.Text),
                            BookingDate = selectedBooking.BookingDate,
                            AppointmentDate = DateOnly.Parse(txtAppointmentDate.Text),
                            Note = txtNote.Text,
                            Status = selectedBooking.Status,
                            UpdateDate = DateOnly.FromDateTime(DateTime.Now),
                            UpdateBy = SessionManager.CurrentUser.UserId
                        };
                        
                        Service custService = (Service)cbxServiceId.SelectedItem;
                        customerBooking.TotalPrice = custService.Price + ((customerBooking.NumberSample - 1) * (custService.ExtraSampleFee ?? 0));
                        
                        _bookingService.Update(customerBooking);
                        MessageBox.Show("Cập nhật đặt lịch thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                        break;
                        
                    default:
                        MessageBox.Show("Bạn không có quyền cập nhật đặt lịch.", "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (dgBookings.SelectedItem is not Booking selectedBooking)
            {
                MessageBox.Show("Vui lòng chọn một đặt lịch để xóa.", "Chưa chọn", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SessionManager.CurrentUser.Role.RoleName != ROLE_ADMIN)
            {
                MessageBox.Show("Chỉ quản trị viên mới có quyền xóa đặt lịch.", "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa đặt lịch ID {selectedBooking.BookingId}?", 
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    
                if (result == MessageBoxResult.Yes)
                {
                    _bookingService.Delete(selectedBooking.BookingId);
                    MessageBox.Show("Xóa đặt lịch thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    LoadData();
                    return;
                }

                IEnumerable<Booking> searchResults;
                
                if (roleName == ROLE_CUSTOMER)
                {
                    searchResults = _bookingService.Search(searchTerm)
                        .Where(b => b.UserId == SessionManager.CurrentUser.UserId);
                }
                else
                {
                    searchResults = _bookingService.Search(searchTerm);
                }
                
                dgBookings.ItemsSource = searchResults;
                
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
            cbxServiceId.SelectedIndex = -1;
            txtNumberSample.Text = "1";
            txtAppointmentDate.Text = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
            txtNote.Text = "";
            txtTotalPrice.Text = "";
            
            if (cbxStatus.Items.Count > 0)
                cbxStatus.SelectedIndex = 0;
            
            dgBookings.SelectedItem = null;
        }

        private void dgBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgBookings.SelectedItem is Booking selectedBooking)
            {
                cbxServiceId.SelectedValue = selectedBooking.ServiceId;
                txtNumberSample.Text = selectedBooking.NumberSample.ToString();
                txtAppointmentDate.Text = selectedBooking.AppointmentDate?.ToString("MM/dd/yyyy");
                txtNote.Text = selectedBooking.Note;
                
                for (int i = 0; i < cbxStatus.Items.Count; i++)
                {
                    if (cbxStatus.Items[i].ToString() == selectedBooking.Status)
                    {
                        cbxStatus.SelectedIndex = i;
                        break;
                    }
                }

                string roleName = SessionManager.CurrentUser?.Role.RoleName;
                
                if (roleName == ROLE_CUSTOMER)
                {
                    bool isCustomerBooking = selectedBooking.UserId == SessionManager.CurrentUser.UserId;
                    bool canEdit = isCustomerBooking && selectedBooking.Status == "Chờ xác nhận";
                    
                    SetAllInputsEnabled(canEdit);
                    cbxStatus.IsEnabled = false;
                    btnUpdate.IsEnabled = canEdit;
                }
                else if (roleName == ROLE_MANAGER || roleName == ROLE_STAFF)
                {
                    SetAllInputsEnabled(true);
                    btnUpdate.IsEnabled = true;
                }
                else if (roleName == ROLE_ADMIN)
                {
                    SetAllInputsEnabled(true);
                    btnUpdate.IsEnabled = true;
                }
                
                UpdatePrice();
            }
        }

        private void cbxServiceId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePrice();
        }

        private void txtNumberSample_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            if (cbxServiceId.SelectedItem is Service selectedService && 
                int.TryParse(txtNumberSample.Text, out int numberSample) && 
                numberSample > 0)
            {
                decimal totalPrice = selectedService.Price + ((numberSample - 1) * (selectedService.ExtraSampleFee ?? 0));
                txtTotalPrice.Text = $"{totalPrice:N0} VND";
            }
            else
            {
                txtTotalPrice.Text = "";
            }
        }
    }
}