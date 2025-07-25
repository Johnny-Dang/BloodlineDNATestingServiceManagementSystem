using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TestResultService
    {
        private readonly TestResultRepository _repo;

        public TestResultService()
        {
            _repo = new TestResultRepository();
        }

        public List<TestResult> GetAll()
        {
            return _repo.GetAll();
        }
        public List<TestResult> Search(string searchText)
        {
            return _repo.Search(searchText);
        }
        public void Add(TestResult result)
        {
            _repo.Add(result);
        }
        public void Update(TestResult item) => _repo.Update(item);
        public void Delete(int id) => _repo.Delete(id);
    }
}
