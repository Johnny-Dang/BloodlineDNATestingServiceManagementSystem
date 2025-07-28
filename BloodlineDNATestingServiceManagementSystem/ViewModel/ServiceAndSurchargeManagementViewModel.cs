using BLL.Services;
using DAL.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BloodlineDNATestingServiceManagementSystem.Controls;
using System.Windows;

namespace BloodlineDNATestingServiceManagementSystem.ViewModel;

public class ServiceAndSurchargeManagementViewModel : INotifyPropertyChanged
{
    private readonly ServiceAndSurchargeService _service;
    private readonly string _role;

    public ObservableCollection<Service> Services { get; set; } = new();
    public ObservableCollection<SurchargePrice> Surcharges { get; set; } = new();

    private Service? _selectedService;
    public Service? SelectedService { get => _selectedService; set { _selectedService = value; OnPropertyChanged(); } }

    private SurchargePrice? _selectedSurcharge;
    public SurchargePrice? SelectedSurcharge { get => _selectedSurcharge; set { _selectedSurcharge = value; OnPropertyChanged(); } }

    public ICommand AddServiceCommand { get; }
    public ICommand EditServiceCommand { get; }
    public ICommand DeleteServiceCommand { get; }
    public ICommand AddSurchargeCommand { get; }
    public ICommand EditSurchargeCommand { get; }
    public ICommand DeleteSurchargeCommand { get; }

    public ServiceAndSurchargeManagementViewModel(ServiceAndSurchargeService service, string role)
    {
        _service = service;
        _role = role;
        LoadData();
        AddServiceCommand = new RelayCommand(_ => AddService(), _ => CanEditOrAdd());
        EditServiceCommand = new RelayCommand(_ => EditService(), _ => CanEditOrAdd() && SelectedService != null);
        DeleteServiceCommand = new RelayCommand(_ => DeleteService(), _ => CanDelete() && SelectedService != null);
        AddSurchargeCommand = new RelayCommand(_ => AddSurcharge(), _ => CanEditOrAdd());
        EditSurchargeCommand = new RelayCommand(_ => EditSurcharge(), _ => CanEditOrAdd() && SelectedSurcharge != null);
        DeleteSurchargeCommand = new RelayCommand(_ => DeleteSurcharge(), _ => CanDelete() && SelectedSurcharge != null);
    }

    private void LoadData()
    {
        Services.Clear();
        foreach (var s in _service.GetAllServices()) Services.Add(s);
        Surcharges.Clear();
        foreach (var s in _service.GetAllSurcharges()) Surcharges.Add(s);
    }

    private bool CanEditOrAdd() => _role == "Admin" || _role == "Manager";
    private bool CanDelete() => _role == "Admin";

    private void AddService()
    {
        var dialog = new ServiceDialog();
        if (dialog.ShowDialog() == true)
        {
            _service.AddService(dialog.Service);
            LoadData();
        }
    }
    private void EditService()
    {
        if (SelectedService == null) return;
        var dialog = new ServiceDialog(SelectedService);
        if (dialog.ShowDialog() == true)
        {
            _service.UpdateService(dialog.Service);
            LoadData();
        }
    }
    private void DeleteService() { if (SelectedService != null) { _service.DeleteService(SelectedService.ServiceId); LoadData(); } }
    private void AddSurcharge()
    {
        var dialog = new SurchargeDialog();
        if (dialog.ShowDialog() == true)
        {
            _service.AddSurcharge(dialog.Surcharge);
            LoadData();
        }
    }
    private void EditSurcharge()
    {
        if (SelectedSurcharge == null) return;
        var dialog = new SurchargeDialog(SelectedSurcharge);
        if (dialog.ShowDialog() == true)
        {
            _service.UpdateSurcharge(dialog.Surcharge);
            LoadData();
        }
    }
    private void DeleteSurcharge() { if (SelectedSurcharge != null) { _service.DeleteSurcharge(SelectedSurcharge.SurchargeId); LoadData(); } }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

// RelayCommand đơn giản cho ICommand
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;
    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null) { _execute = execute; _canExecute = canExecute; }
    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);
    public void Execute(object? parameter) => _execute(parameter);
    public event EventHandler? CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
} 