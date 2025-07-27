using DAL.Entities;
using DAL.Repositories;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ServiceService
    {
        private readonly ServiceRepository _repo;

        public ServiceService()
        {
            _repo = new ServiceRepository();
        }

        public List<Service> GetAll()
        {
            return _repo.GetAll();
        }

        public Service GetById(int id)
        {
            return _repo.GetById(id);
        }
    }
}