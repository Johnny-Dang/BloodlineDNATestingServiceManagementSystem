using BLL.Services;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for FeedBackWindow.xaml
    /// </summary>
    public partial class FeedBackWindow : Window
    {
        private readonly FeedBackService _feedbackService;
        private readonly BookingService _bookingService;
        public FeedBackWindow()
        {
            InitializeComponent();
            _feedbackService = new FeedBackService();
            _bookingService = new BookingService();
            LoadFeedbacks();
            LoadBookings();
        }
        public void LoadBookings()
        {
            cbxBookingId.ItemsSource = _bookingService.GetAll();
            cbxBookingId.DisplayMemberPath = "BookingId";
            cbxBookingId.SelectedValuePath = "BookingId";
        }
        private void LoadFeedbacks()
        {
            dgFeedback.ItemsSource = _feedbackService.GetAll();
        }
        private bool ValidateForm(bool isCreate = true, int currentId = 0)
        {
            if (string.IsNullOrWhiteSpace(txtComments.Text) ||
                string.IsNullOrWhiteSpace(txtAnswers.Text) ||
                string.IsNullOrWhiteSpace(txtRating.Text) ||
                string.IsNullOrWhiteSpace(txtCreateDate.Text) ||
                string.IsNullOrWhiteSpace(txtReturnDate.Text) ||
                string.IsNullOrWhiteSpace(txtStatus.Text))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            int bookingId = (int)cbxBookingId.SelectedValue;
            if (isCreate)
            {
                bool exists = _feedbackService.GetAll().Any(f => f.BookingId == bookingId);
                if (exists)
                {
                    MessageBox.Show("This Booking ID already has a Feedback!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            else
            {
                bool exists = _feedbackService.GetAll()
                    .Any(f => f.BookingId == bookingId && f.FeedbackId != currentId);
                if (exists)
                {
                    MessageBox.Show("This Booking ID already has a Feedback!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            if (!int.TryParse(txtRating.Text, out _))
            {
                MessageBox.Show("Rating must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!DateOnly.TryParse(txtCreateDate.Text, out _))
            {
                MessageBox.Show("Invalid Create Date format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!DateOnly.TryParse(txtReturnDate.Text, out _))
            {
                MessageBox.Show("Invalid Return Date format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm(true)) return;

            var feedback = new Feedback
            {
                BookingId = (int)cbxBookingId.SelectedValue,
                Comments = txtComments.Text,
                Answers = txtAnswers.Text,
                Rating = int.Parse(txtRating.Text),
                CreateDate = DateOnly.Parse(txtCreateDate.Text),
                ReturnDate = DateOnly.Parse(txtReturnDate.Text),
                Status = txtStatus.Text
            };
            _feedbackService.Add(feedback);
            LoadFeedbacks();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgFeedback.SelectedItem is not Feedback current)
            {
                MessageBox.Show("Select a record to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!ValidateForm(false, current.FeedbackId)) return;
            var feedback = new Feedback
            {
                FeedbackId = ((Feedback)dgFeedback.SelectedValue).FeedbackId,
                BookingId = (int)cbxBookingId.SelectedValue,
                Comments = txtComments.Text,
                Answers = txtAnswers.Text,
                Rating = int.Parse(txtRating.Text),
                CreateDate = DateOnly.Parse(txtCreateDate.Text),
                ReturnDate = DateOnly.Parse(txtReturnDate.Text),
                Status = txtStatus.Text
            };

            _feedbackService.Update(feedback);
            LoadFeedbacks();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgFeedback.SelectedItem is Feedback selected)
            {
                var confirm = MessageBox.Show($"Delete Feedback ID {selected.FeedbackId}?",
                                               "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm == MessageBoxResult.Yes)
                {
                    _feedbackService.Delete(selected.FeedbackId);
                    LoadFeedbacks();
                }
            }
        }

        private void dgFeedback_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgFeedback.SelectedItem is Feedback selected)
            {
                cbxBookingId.SelectedValue = selected.BookingId;
                txtComments.Text = selected.Comments;
                txtAnswers.Text = selected.Answers;
                txtRating.Text = selected.Rating?.ToString();
                txtCreateDate.Text = selected.CreateDate?.ToString();
                txtReturnDate.Text = selected.ReturnDate?.ToString();
                txtStatus.Text = selected.Status;
            }
        }

        private void btnTestResult_Click(object sender, RoutedEventArgs e)
        {
            TestResultWindow testResultWindow = new TestResultWindow();
            testResultWindow.Show();
            this.Close();
        }

        private void btnDetailResult_Click(object sender, RoutedEventArgs e)
        {
            DetailResultWindow detailResultWindow = new DetailResultWindow();
            detailResultWindow.Show();
            this.Close();   
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text;
            dgFeedback.ItemsSource = _feedbackService.Search(searchText);
        }
    }
}
