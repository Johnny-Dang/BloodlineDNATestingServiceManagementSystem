using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
