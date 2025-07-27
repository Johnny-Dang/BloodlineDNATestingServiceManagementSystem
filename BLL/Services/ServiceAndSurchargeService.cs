using DAL.Entities;
using DAL.Repositories;
using System.Collections.Generic;

namespace BLL.Services;

public class ServiceAndSurchargeService
{
    private readonly ServiceRepository _serviceRepo;
    private readonly SurchargePriceRepository _surchargeRepo;

    public ServiceAndSurchargeService(ServiceRepository serviceRepo, SurchargePriceRepository surchargeRepo)
    {
        _serviceRepo = serviceRepo;
        _surchargeRepo = surchargeRepo;
    }

    // Service CRUD
    public IEnumerable<Service> GetAllServices() => _serviceRepo.GetAll();
    public Service? GetServiceById(int id) => _serviceRepo.GetById(id);
    public void AddService(Service service) => _serviceRepo.Add(service);
    public void UpdateService(Service service) => _serviceRepo.Update(service);
    public void DeleteService(int id) => _serviceRepo.Delete(id);

    // SurchargePrice CRUD
    public IEnumerable<SurchargePrice> GetAllSurcharges() => _surchargeRepo.GetAll();
    public SurchargePrice? GetSurchargeById(int id) => _surchargeRepo.GetById(id);
    public void AddSurcharge(SurchargePrice surcharge) => _surchargeRepo.Add(surcharge);
    public void UpdateSurcharge(SurchargePrice surcharge) => _surchargeRepo.Update(surcharge);
    public void DeleteSurcharge(int id) => _surchargeRepo.Delete(id);
} 