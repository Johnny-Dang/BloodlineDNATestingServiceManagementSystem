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
    /// Interaction logic for DetailResultWindow.xaml
    /// </summary>
    public partial class DetailResultWindow : Window
    {
        private readonly DetailResultService _detailResultService;
        private readonly TestResultService _testResultService;
        public DetailResultWindow()
        {
            InitializeComponent();
            _detailResultService = new DetailResultService();
            _testResultService = new TestResultService();
            LoadDetailResults();
            LoadTestResult();
        }
        public void LoadTestResult()
        {
            cbxResultConclution.ItemsSource = _testResultService.GetAll();
            cbxResultConclution.DisplayMemberPath = "ResultConclution";
            cbxResultConclution.SelectedValuePath = "TestResultId";
        }
        private void LoadDetailResults()
        {
            dgDetailResult.ItemsSource = _detailResultService.GetAll();
        }
        private bool Validate()
        {
            if (string.IsNullOrEmpty(txtLocusName.Text) ||
                string.IsNullOrWhiteSpace(txtP1A1.Text) ||
                string.IsNullOrWhiteSpace(txtP1A2.Text) ||
                string.IsNullOrWhiteSpace(txtP2A1.Text) ||
                string.IsNullOrWhiteSpace(txtP2A2.Text) ||
                string.IsNullOrWhiteSpace(txtPaternityIndex.Text) ||
                cbxResultConclution.SelectedValue == null)
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                return;
            }
            var detail = new DetailResult
            {
                TestResultId = (int)cbxResultConclution.SelectedValue,
                LocusName = txtLocusName.Text,
                P1Allele1 = txtP1A1.Text,
                P1Allele2 = txtP1A2.Text,
                P2Allele1 = txtP2A1.Text,
                P2Allele2 = txtP2A2.Text,
                PaternityIndex = decimal.Parse(txtPaternityIndex.Text),
            };
            _detailResultService.Add(detail);
            LoadDetailResults();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(!Validate())
            {
                return;
            }
            var detail = new DetailResult
            {
                DetailResultId = ((DetailResult)dgDetailResult.SelectedItem).DetailResultId,
                TestResultId = (int)cbxResultConclution.SelectedValue,
                LocusName = txtLocusName.Text,
                P1Allele1 = txtP1A1.Text,
                P1Allele2 = txtP1A2.Text,
                P2Allele1 = txtP2A1.Text,
                P2Allele2 = txtP2A2.Text,
                PaternityIndex = decimal.Parse(txtPaternityIndex.Text),
            };
            _detailResultService.Update(detail);
            LoadDetailResults();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgDetailResult.SelectedItem is DetailResult selected)
            {
                var confirm = MessageBox.Show($"Delete ID {selected.DetailResultId}?",
                                               "Confirm", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    _detailResultService.Delete(selected.DetailResultId);
                    LoadDetailResults();
                }
            }
        }

        private void dgDetailResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDetailResult.SelectedItem is DetailResult selected)
            {
                cbxResultConclution.SelectedValue = selected.TestResultId;
                txtLocusName.Text = selected.LocusName;
                txtP1A1.Text = selected.P1Allele1;
                txtP1A2.Text = selected.P1Allele2;
                txtP2A1.Text = selected.P2Allele1;
                txtP2A2.Text = selected.P2Allele2;
                txtPaternityIndex.Text = selected.PaternityIndex?.ToString();
            }
        }

        private void btnTestResult_Click(object sender, RoutedEventArgs e)
        {
            TestResultWindow testResultWindow = new TestResultWindow();
            testResultWindow.Show();
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
            dgDetailResult.ItemsSource = _detailResultService.Search(searchText);
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
