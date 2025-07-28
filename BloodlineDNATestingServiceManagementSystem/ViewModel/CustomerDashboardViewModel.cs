using BLL.Services;
using DAL.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BloodlineDNATestingServiceManagementSystem.ViewModel;

public class CustomerDashboardViewModel : INotifyPropertyChanged
{
    private readonly CustomerDashboardService _service;
    private readonly int _customerId;

    public ObservableCollection<Booking> Bookings { get; set; } = new();
    public ObservableCollection<BlogPost> BlogPosts { get; set; } = new();

    public CustomerDashboardViewModel(CustomerDashboardService service, int customerId)
    {
        _service = service;
        _customerId = customerId;
        LoadData();
    }

    private void LoadData()
    {
        Bookings.Clear();
        foreach (var b in _service.GetBookingsByCustomerId(_customerId)) Bookings.Add(b);
        BlogPosts.Clear();
        foreach (var p in _service.GetActiveBlogPosts()) BlogPosts.Add(p);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
} 