using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class DetailResultService
    {
        private readonly DetailResultRepositorys _repo;

        public DetailResultService()
        {
            _repo = new DetailResultRepositorys();
        }

        public List<DetailResult> GetAll() => _repo.GetAll();

        public List<DetailResult> Search(string searchText) => _repo.Search(searchText);
        public void Add(DetailResult detail) => _repo.Add(detail);

        public void Update(DetailResult detail) => _repo.Update(detail);

        public void Delete(int id) => _repo.Delete(id);
    }
}
