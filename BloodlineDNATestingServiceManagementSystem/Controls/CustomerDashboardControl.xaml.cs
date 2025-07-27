using System.Windows.Controls;
using BloodlineDNATestingServiceManagementSystem.ViewModel;
using BLL.Services;

namespace BloodlineDNATestingServiceManagementSystem.Controls;

public partial class CustomerDashboardControl : UserControl
{
    public CustomerDashboardControl(CustomerDashboardService service, int customerId)
    {
        InitializeComponent();
        DataContext = new CustomerDashboardViewModel(service, customerId);
    }
} 