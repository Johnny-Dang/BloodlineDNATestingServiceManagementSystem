using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FeedBackService
    {
        private readonly FeedBackRepository _repo;

        public FeedBackService()
        {
            _repo = new FeedBackRepository();
        }
        public List<Feedback> GetAll()
        {
            return _repo.GetAll();
        }

        public List<Feedback> Search(string searchText)
        {
            return _repo.Search(searchText);
        }

        public void Add(Feedback feedback)
        {
            _repo.Add(feedback);
        }

        public void Update(Feedback feedback)
        {
            _repo.Update(feedback);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
