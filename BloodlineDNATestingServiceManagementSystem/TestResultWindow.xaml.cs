using BLL.Services;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BloodlineDNATestingServiceManagementSystem
{
    /// <summary>
    /// Interaction logic for TestResultWindow.xaml
    /// </summary>
    public partial class TestResultWindow : Window
    {
        private readonly TestResultService _testResultService;
        private readonly BookingService _bookingService;
        private readonly DetailResultService _detailResultService;
        public TestResultWindow()
        {
            InitializeComponent();
            _testResultService = new TestResultService();
            _bookingService = new BookingService();
            _detailResultService = new DetailResultService();
            LoadTestResults();
            LoadBookings();
        }
        public void LoadTestResults()
        {
            dgTestResult.ItemsSource = _testResultService.GetAll();
        }
        public void LoadBookings()
        {
            cbxBookingId.ItemsSource = _bookingService.GetAll();
            cbxBookingId.DisplayMemberPath = "BookingId";
            cbxBookingId.SelectedValuePath = "BookingId";
        }


        private bool ValidateForm(bool isCreate = true, int currentId = 0)
        {
            if (cbxBookingId.SelectedValue == null)
            {
                MessageBox.Show("Please select a Booking.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            int bookingId = (int)cbxBookingId.SelectedValue;

            if (isCreate)
            {
                bool exists = _testResultService.GetAll()
                                                .Any(tr => tr.BookingId == bookingId);
                if (exists)
                {
                    MessageBox.Show("This Booking ID already has a Test Result!",
                                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            else
            {
                // Chỉ bắt lỗi nếu BookingId trùng với bản ghi khác
                bool exists = _testResultService.GetAll()
                                                .Any(tr => tr.BookingId == bookingId && tr.TestResultId != currentId);
                if (exists)
                {
                    MessageBox.Show("This Booking ID already has a Test Result!",
                                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(txtResultConclution.Text) ||
                string.IsNullOrWhiteSpace(txtResultDate.Text) ||
                string.IsNullOrWhiteSpace(txtResultFile.Text) ||
                string.IsNullOrWhiteSpace(txtCreatedBy.Text) ||
                string.IsNullOrWhiteSpace(txtCreatedDate.Text)||
                string.IsNullOrWhiteSpace(txtUpdatedBy.Text) ||
                string.IsNullOrWhiteSpace(txtUpdatedDate.Text))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!DateTime.TryParse(txtResultDate.Text, out _))
            {
                MessageBox.Show("Invalid Result Date. Please enter a valid date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!DateTime.TryParse(txtCreatedDate.Text, out _))
            {
                MessageBox.Show("Invalid Created Date. Please enter a valid date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!DateTime.TryParse(txtUpdatedDate.Text, out _))
            {
                MessageBox.Show("Invalid Updated Date. Please enter a valid date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm(true)) return; // true = Create

            var result = new TestResult
            {
                BookingId = (int)cbxBookingId.SelectedValue,
                ResultConclution = txtResultConclution.Text,
                ResultDate = DateOnly.Parse(txtResultDate.Text),
                ResultFile = txtResultFile.Text,
                CreatedBy = txtCreatedBy.Text,
                CreatedDate = DateOnly.Parse(txtCreatedDate.Text),
                UpdatedBy = txtUpdatedBy.Text,
                UpdatedDate = DateOnly.Parse(txtUpdatedDate.Text)
            };

            _testResultService.Add(result);
            LoadTestResults();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgTestResult.SelectedItem is not TestResult current)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            if (!ValidateForm(false, current.TestResultId)) return;
            var selected = new TestResult
            {
                TestResultId = ((TestResult)dgTestResult.SelectedItem).TestResultId,
                BookingId = (int)cbxBookingId.SelectedValue,
                ResultConclution = txtResultConclution.Text,
                ResultDate = DateOnly.Parse(txtResultDate.Text),
                ResultFile = txtResultFile.Text,
                CreatedBy = txtCreatedBy.Text,
                CreatedDate = DateOnly.Parse(txtCreatedDate.Text),
                UpdatedBy = txtUpdatedBy.Text,
                UpdatedDate = DateOnly.Parse(txtUpdatedDate.Text)
            };

            _testResultService.Update(selected);
            LoadTestResults();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTestResult.SelectedItem is TestResult selected)
            {
                var confirm = MessageBox.Show($"Delete ID {selected.TestResultId}?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (confirm != MessageBoxResult.Yes)
                {
                    return;
                }
                _testResultService.Delete(selected.TestResultId);
                LoadTestResults();
            }
        }

        private void dgTestResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTestResult.SelectedItem is TestResult selected)
            {
                txtResultConclution.Text = selected.ResultConclution;
                txtResultDate.Text = selected.ResultDate.ToString();
                txtResultFile.Text = selected.ResultFile;
                txtCreatedBy.Text = selected.CreatedBy;
                txtCreatedDate.Text = selected.CreatedDate.ToString();
                txtUpdatedBy.Text = selected.UpdatedBy;
                txtUpdatedDate.Text = selected.UpdatedDate?.ToString();
                cbxBookingId.SelectedValue = selected.BookingId;

            }
        }

        private void btnDetailResult_Click(object sender, RoutedEventArgs e)
        {
            DetailResultWindow detailResultWindow = new DetailResultWindow();
            detailResultWindow.Show();
            this.Close();
        }

        private void btnFeedback_Click(object sender, RoutedEventArgs e)
        {
            FeedBackWindow feedbackWindow = new FeedBackWindow();
            feedbackWindow.Show();
            this.Close();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text;
            dgTestResult.ItemsSource = _testResultService.Search(searchText);
        }
    }
}
        
