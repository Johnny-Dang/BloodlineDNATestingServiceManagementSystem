using DAL.Entities;
using DAL.Repositories;
using System.Collections.Generic;

namespace BLL.Services
{
    public class BookingService
    {
        private readonly BookingRepository _repo;

        public BookingService()
        {
            _repo = new BookingRepository();
        }

        public List<Booking> GetAll()
        {
            return _repo.GetAll();
        }

        public List<Booking> GetByUserId(int userId)
        {
            return _repo.GetByUserId(userId);
        }

        public List<Booking> Search(string searchText)
        {
            return _repo.Search(searchText);
        }

        public void Add(Booking booking)
        {
            _repo.Add(booking);
        }

        public void Update(Booking booking)
        {
            _repo.Update(booking);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        public Booking GetById(int id)
        {
            return _repo.GetById(id);
        }
    }
}
