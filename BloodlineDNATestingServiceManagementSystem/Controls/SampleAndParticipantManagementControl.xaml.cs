using BLL.Services;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BloodlineDNATestingServiceManagementSystem.Controls
{
    public partial class SampleAndParticipantManagementControl : UserControl
    {
        private readonly ParticipantService _participantService;
        private readonly SampleService _sampleService;
        private readonly BookingService _bookingService;
        private Participant _selectedParticipant;
        private Sample _selectedSample;
        private List<Participant> _participants;
        private List<Sample> _samples;
        private List<Booking> _bookings;

        public SampleAndParticipantManagementControl()
        {
            InitializeComponent();
            _participantService = new ParticipantService();
            _sampleService = new SampleService();
            _bookingService = new BookingService();
            LoadData();
            SetupRoleBasedAccess();
        }

        private void SetupRoleBasedAccess()
        {
            var currentUser = SessionManager.CurrentUser;
            if (currentUser == null) return;

            var roleName = currentUser.Role.RoleName;
            
            // Admin and Manager have full CRUD access
            if (roleName == "ADMIN" || roleName == "MANAGER")
            {
                // All buttons enabled
                return;
            }
            
            // Staff can view, create, update but not delete
            if (roleName == "STAFF")
            {
                btnParticipantDelete.IsEnabled = false;
                btnSampleDelete.IsEnabled = false;
            }
            
            // Customer can only view their own data
            if (roleName == "CUSTOMER")
            {
                btnParticipantCreate.IsEnabled = false;
                btnParticipantUpdate.IsEnabled = false;
                btnParticipantDelete.IsEnabled = false;
                btnSampleCreate.IsEnabled = false;
                btnSampleUpdate.IsEnabled = false;
                btnSampleDelete.IsEnabled = false;
            }
        }

        private void LoadData()
        {
            try
            {
                var currentUser = SessionManager.CurrentUser;
                
                if (currentUser == null)
                {
                    MessageBox.Show("Vui lòng đăng nhập để sử dụng tính năng này.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var roleName = currentUser.Role.RoleName;
                
                // Load participants based on user role
                if (roleName == "CUSTOMER")
                {
                    _participants = _participantService.GetByUserId(currentUser.UserId);
                }
                else
                {
                    _participants = _participantService.GetAll();
                }
                
                dgParticipants.ItemsSource = _participants;

                // Load samples based on user role
                if (roleName == "CUSTOMER")
                {
                    _samples = _sampleService.GetByUserId(currentUser.UserId);
                }
                else
                {
                    _samples = _sampleService.GetAll();
                }
                
                dgSamples.ItemsSource = _samples;

                // Load bookings for sample creation
                _bookings = _bookingService.GetAll();
                cbxSampleBookingId.ItemsSource = _bookings;
                cbxSampleBookingId.DisplayMemberPath = "BookingId";
                cbxSampleBookingId.SelectedValuePath = "BookingId";

                ClearParticipantForm();
                ClearSampleForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var searchText = txtSearch.Text.Trim();
                var currentUser = SessionManager.CurrentUser;
                
                if (currentUser == null) return;

                var roleName = currentUser.Role.RoleName;
                
                // Search participants
                if (roleName == "CUSTOMER")
                {
                    _participants = _participantService.GetByUserId(currentUser.UserId)
                        .Where(p => p.FullName.ToLower().Contains(searchText.ToLower()) ||
                                   (p.IdentityNumber != null && p.IdentityNumber.ToLower().Contains(searchText.ToLower())))
                        .ToList();
                }
                else
                {
                    _participants = _participantService.Search(searchText);
                }
                dgParticipants.ItemsSource = _participants;

                // Search samples
                if (roleName == "CUSTOMER")
                {
                    _samples = _sampleService.GetByUserId(currentUser.UserId)
                        .Where(s => s.SampleType.ToLower().Contains(searchText.ToLower()) ||
                                   s.TypeOfCollection.ToLower().Contains(searchText.ToLower()) ||
                                   s.Participant.FullName.ToLower().Contains(searchText.ToLower()))
                        .ToList();
                }
                else
                {
                    _samples = _sampleService.Search(searchText);
                }
                dgSamples.ItemsSource = _samples;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            LoadData();
        }

        #region Participant Management

        private void dgParticipants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedParticipant = dgParticipants.SelectedItem as Participant;
            if (_selectedParticipant != null)
            {
                LoadParticipantData();
            }
        }

        private void LoadParticipantData()
        {
            if (_selectedParticipant == null) return;

            txtParticipantFullName.Text = _selectedParticipant.FullName;
            txtParticipantDateOfBirth.Text = _selectedParticipant.DateOfBirth.ToString("yyyy-MM-dd");
            txtParticipantIdentityNumber.Text = _selectedParticipant.IdentityNumber ?? "";
            txtParticipantRelationship.Text = _selectedParticipant.RelationshipToCustomer ?? "";
            txtParticipantAddress.Text = _selectedParticipant.Address ?? "";
            txtParticipantCollectionMethod.Text = _selectedParticipant.CollectionMethod ?? "";
            txtParticipantQuestionalbleRelationship.Text = _selectedParticipant.QuestionalbleRelationship;

            // Set gender combobox
            foreach (ComboBoxItem item in cbxParticipantGender.Items)
            {
                if (item.Content.ToString() == _selectedParticipant.Gender)
                {
                    cbxParticipantGender.SelectedItem = item;
                    break;
                }
            }
        }

        private void ClearParticipantForm()
        {
            txtParticipantFullName.Clear();
            txtParticipantDateOfBirth.Clear();
            txtParticipantIdentityNumber.Clear();
            txtParticipantRelationship.Clear();
            txtParticipantAddress.Clear();
            txtParticipantCollectionMethod.Clear();
            txtParticipantQuestionalbleRelationship.Clear();
            cbxParticipantGender.SelectedIndex = -1;
            _selectedParticipant = null;
        }

        private void btnParticipantCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtParticipantFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên người tham gia.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var participant = new Participant
                {
                    FullName = txtParticipantFullName.Text.Trim(),
                    DateOfBirth = DateOnly.Parse(txtParticipantDateOfBirth.Text),
                    Gender = (cbxParticipantGender.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Nam",
                    IdentityNumber = txtParticipantIdentityNumber.Text.Trim(),
                    RelationshipToCustomer = txtParticipantRelationship.Text.Trim(),
                    Address = txtParticipantAddress.Text.Trim(),
                    CollectionMethod = txtParticipantCollectionMethod.Text.Trim(),
                    QuestionalbleRelationship = txtParticipantQuestionalbleRelationship.Text.Trim()
                };

                _participantService.Add(participant);
                MessageBox.Show("Thêm người tham gia thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                
                ClearParticipantForm();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm người tham gia: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnParticipantUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedParticipant == null)
                {
                    MessageBox.Show("Vui lòng chọn người tham gia để cập nhật.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtParticipantFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên người tham gia.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _selectedParticipant.FullName = txtParticipantFullName.Text.Trim();
                _selectedParticipant.DateOfBirth = DateOnly.Parse(txtParticipantDateOfBirth.Text);
                _selectedParticipant.Gender = (cbxParticipantGender.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Nam";
                _selectedParticipant.IdentityNumber = txtParticipantIdentityNumber.Text.Trim();
                _selectedParticipant.RelationshipToCustomer = txtParticipantRelationship.Text.Trim();
                _selectedParticipant.Address = txtParticipantAddress.Text.Trim();
                _selectedParticipant.CollectionMethod = txtParticipantCollectionMethod.Text.Trim();
                _selectedParticipant.QuestionalbleRelationship = txtParticipantQuestionalbleRelationship.Text.Trim();

                _participantService.Update(_selectedParticipant);
                MessageBox.Show("Cập nhật người tham gia thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật người tham gia: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnParticipantDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedParticipant == null)
                {
                    MessageBox.Show("Vui lòng chọn người tham gia để xóa.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa người tham gia '{_selectedParticipant.FullName}'?", 
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    _participantService.Delete(_selectedParticipant.ParticipantId);
                    MessageBox.Show("Xóa người tham gia thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    ClearParticipantForm();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa người tham gia: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Sample Management

        private void dgSamples_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSample = dgSamples.SelectedItem as Sample;
            if (_selectedSample != null)
            {
                LoadSampleData();
            }
        }

        private void LoadSampleData()
        {
            if (_selectedSample == null) return;

            cbxSampleBookingId.SelectedValue = _selectedSample.BookingId;
            cbxSampleParticipantId.SelectedValue = _selectedSample.ParticipantId;
            txtSampleReceivedDate.Text = _selectedSample.ReceivedDate?.ToString("yyyy-MM-dd") ?? "";
            txtSampleUserId.Text = _selectedSample.UserId?.ToString() ?? "";

            // Set sample type combobox
            foreach (ComboBoxItem item in cbxSampleType.Items)
            {
                if (item.Content.ToString() == _selectedSample.SampleType)
                {
                    cbxSampleType.SelectedItem = item;
                    break;
                }
            }

            // Set collection type combobox
            foreach (ComboBoxItem item in cbxTypeOfCollection.Items)
            {
                if (item.Content.ToString() == _selectedSample.TypeOfCollection)
                {
                    cbxTypeOfCollection.SelectedItem = item;
                    break;
                }
            }
        }

        private void ClearSampleForm()
        {
            cbxSampleBookingId.SelectedIndex = -1;
            cbxSampleParticipantId.SelectedIndex = -1;
            txtSampleReceivedDate.Clear();
            txtSampleUserId.Clear();
            cbxSampleType.SelectedIndex = -1;
            cbxTypeOfCollection.SelectedIndex = -1;
            _selectedSample = null;
        }

        private void cbxSampleBookingId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxSampleBookingId.SelectedItem is Booking selectedBooking)
            {
                // Load participants for this booking
                var participants = _participantService.GetByBookingId(selectedBooking.BookingId);
                cbxSampleParticipantId.ItemsSource = participants;
                cbxSampleParticipantId.DisplayMemberPath = "FullName";
                cbxSampleParticipantId.SelectedValuePath = "ParticipantId";
            }
        }

        private void btnSampleCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbxSampleBookingId.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn Booking ID.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbxSampleParticipantId.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn người tham gia.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbxSampleType.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn loại mẫu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbxTypeOfCollection.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn phương thức thu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var sample = new Sample
                {
                    BookingId = (int)cbxSampleBookingId.SelectedValue,
                    ParticipantId = (int)cbxSampleParticipantId.SelectedValue,
                    SampleType = (cbxSampleType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Máu",
                    TypeOfCollection = (cbxTypeOfCollection.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Tại phòng khám",
                    ReceivedDate = !string.IsNullOrWhiteSpace(txtSampleReceivedDate.Text) ? 
                        DateOnly.Parse(txtSampleReceivedDate.Text) : null,
                    UserId = !string.IsNullOrWhiteSpace(txtSampleUserId.Text) ? 
                        int.Parse(txtSampleUserId.Text) : null
                };

                _sampleService.Add(sample);
                MessageBox.Show("Thêm mẫu vật thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                
                ClearSampleForm();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm mẫu vật: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSampleUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedSample == null)
                {
                    MessageBox.Show("Vui lòng chọn mẫu vật để cập nhật.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbxSampleBookingId.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn Booking ID.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbxSampleParticipantId.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn người tham gia.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbxSampleType.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn loại mẫu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbxTypeOfCollection.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn phương thức thu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _selectedSample.BookingId = (int)cbxSampleBookingId.SelectedValue;
                _selectedSample.ParticipantId = (int)cbxSampleParticipantId.SelectedValue;
                _selectedSample.SampleType = (cbxSampleType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Máu";
                _selectedSample.TypeOfCollection = (cbxTypeOfCollection.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Tại phòng khám";
                _selectedSample.ReceivedDate = !string.IsNullOrWhiteSpace(txtSampleReceivedDate.Text) ? 
                    DateOnly.Parse(txtSampleReceivedDate.Text) : null;
                _selectedSample.UserId = !string.IsNullOrWhiteSpace(txtSampleUserId.Text) ? 
                    int.Parse(txtSampleUserId.Text) : null;

                _sampleService.Update(_selectedSample);
                MessageBox.Show("Cập nhật mẫu vật thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật mẫu vật: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSampleDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedSample == null)
                {
                    MessageBox.Show("Vui lòng chọn mẫu vật để xóa.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa mẫu vật ID {_selectedSample.SampleId}?", 
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    _sampleService.Delete(_selectedSample.SampleId);
                    MessageBox.Show("Xóa mẫu vật thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    ClearSampleForm();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa mẫu vật: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
} 