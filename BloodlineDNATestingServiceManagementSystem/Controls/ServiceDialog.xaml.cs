using System.Windows;
using DAL.Entities;
using System.Collections.ObjectModel;

namespace BloodlineDNATestingServiceManagementSystem.Controls;

public partial class ServiceDialog : Window
{
    public Service Service { get; private set; }
    public ObservableCollection<string> ServiceTypes { get; set; } = new() { "Dân sự", "Hành chính" };
    public ObservableCollection<string> PackageTypes { get; set; } = new() { "Tiêu chuẩn (2-5 ngày)", "Lấy nhanh (6-24 tiếng)", "Tiêu chuẩn (10-14 ngày)", "Lấy nhanh (7-10 ngày)" };
    public ObservableCollection<string> Statuses { get; set; } = new() { "Hoạt động", "Ngừng hoạt động" };

    public ServiceDialog(Service? service = null)
    {
        InitializeComponent();
        DataContext = this;
        if (service != null)
        {
            Service = new Service
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                ServiceType = service.ServiceType,
                PackageType = service.PackageType,
                Price = service.Price,
                Status = service.Status,
                ExtraSampleFee = service.ExtraSampleFee
            };
            txtServiceName.Text = Service.ServiceName;
            cbServiceType.SelectedItem = Service.ServiceType;
            cbPackageType.SelectedItem = Service.PackageType;
            txtPrice.Text = Service.Price.ToString();
            cbStatus.SelectedItem = Service.Status;
            txtExtraSampleFee.Text = Service.ExtraSampleFee?.ToString() ?? "";
        }
        else
        {
            Service = new Service();
            cbServiceType.SelectedIndex = 0;
            cbPackageType.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
        }
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        Service.ServiceName = txtServiceName.Text.Trim();
        Service.ServiceType = cbServiceType.SelectedItem?.ToString() ?? "";
        Service.PackageType = cbPackageType.SelectedItem?.ToString() ?? "";
        Service.Status = cbStatus.SelectedItem?.ToString() ?? "";
        if (decimal.TryParse(txtPrice.Text, out var price)) Service.Price = price;
        if (decimal.TryParse(txtExtraSampleFee.Text, out var extra)) Service.ExtraSampleFee = extra;
        DialogResult = true;
        Close();
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
} 