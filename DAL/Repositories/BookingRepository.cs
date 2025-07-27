using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.User)
                .Include(b => b.UpdateByNavigation)
                .ToList();
        }

        public List<Booking> GetByUserId(int userId)
        {
            return _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.User)
                .Include(b => b.UpdateByNavigation)
                .Where(b => b.UserId == userId)
                .ToList();
        }

        public Booking GetById(int id)
        {
            return _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.User)
                .Include(b => b.UpdateByNavigation)
                .FirstOrDefault(b => b.BookingId == id);
        }

        public List<Booking> Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return GetAll();

            searchText = searchText.ToLower();
            return _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.User)
                .Include(b => b.UpdateByNavigation)
                .Where(b => 
                    b.BookingId.ToString().Contains(searchText) ||
                    (b.Status != null && b.Status.ToLower().Contains(searchText)) ||
                    (b.Note != null && b.Note.ToLower().Contains(searchText)) ||
                    (b.Service != null && b.Service.ServiceName.ToLower().Contains(searchText)) ||
                    (b.User != null && b.User.FullName.ToLower().Contains(searchText)) ||
                    (b.TotalPrice != null && b.TotalPrice.ToString().Contains(searchText))
                )
                .ToList();
        }

        public void Add(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public void Update(Booking booking)
        {
            var existingBooking = _context.Bookings.Find(booking.BookingId);
            if (existingBooking != null)
            {
                _context.Entry(existingBooking).CurrentValues.SetValues(booking);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }
        }
    }
}
