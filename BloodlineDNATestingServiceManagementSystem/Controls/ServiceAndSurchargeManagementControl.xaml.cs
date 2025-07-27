using System.Windows.Controls;
using BloodlineDNATestingServiceManagementSystem.ViewModel;
using BLL.Services;
using DAL.Repositories;
using DAL.Entities;

namespace BloodlineDNATestingServiceManagementSystem.Controls;

public partial class ServiceAndSurchargeManagementControl : UserControl
{
    public ServiceAndSurchargeManagementControl(ServiceAndSurchargeService service, string role)
    {
        InitializeComponent();
        DataContext = new ServiceAndSurchargeManagementViewModel(service, role);
    }
} 