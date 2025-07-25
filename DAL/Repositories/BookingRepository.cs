using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class BookingRepository
    {
        private readonly DnatestingServiceContext _context;
        public BookingRepository()
        {
            _context = new DnatestingServiceContext();  
        }
        public List<Booking> GetAll()
        {
            return _context.Bookings.ToList();
        }
    }
}
