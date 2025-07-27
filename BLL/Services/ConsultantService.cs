using DAL.Entities;
using DAL.Repositories;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ConsultantService
    {
        private readonly ConsultantRepository _repo;

        public ConsultantService()
        {
            _repo = new ConsultantRepository();
        }

        public List<Consultant> GetAll()
        {
            return _repo.GetAll();
        }

        public List<Consultant> GetByUserId(int userId)
        {
            return _repo.GetByUserId(userId);
        }

        public List<Consultant> Search(string searchText)
        {
            return _repo.Search(searchText);
        }

        public void Add(Consultant consultant)
        {
            _repo.Add(consultant);
        }

        public void Update(Consultant consultant)
        {
            _repo.Update(consultant);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        public Consultant GetById(int id)
        {
            return _repo.GetById(id);
        }
    }
}