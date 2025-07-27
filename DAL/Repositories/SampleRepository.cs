using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class SampleRepository
    {
        private readonly DnatestingServiceContext _context;
        
        public SampleRepository()
        {
            _context = new DnatestingServiceContext();  
        }
        
        public List<Sample> GetAll()
        {
            return _context.Samples
                .Include(s => s.Booking)
                .ThenInclude(b => b.User)
                .Include(s => s.Participant)
                .Include(s => s.User)
                .ToList();
        }

        public List<Sample> GetByBookingId(int bookingId)
        {
            return _context.Samples
                .Include(s => s.Booking)
                .ThenInclude(b => b.User)
                .Include(s => s.Participant)
                .Include(s => s.User)
                .Where(s => s.BookingId == bookingId)
                .ToList();
        }

        public List<Sample> GetByParticipantId(int participantId)
        {
            return _context.Samples
                .Include(s => s.Booking)
                .ThenInclude(b => b.User)
                .Include(s => s.Participant)
                .Include(s => s.User)
                .Where(s => s.ParticipantId == participantId)
                .ToList();
        }

        public List<Sample> GetByUserId(int userId)
        {
            return _context.Samples
                .Include(s => s.Booking)
                .ThenInclude(b => b.User)
                .Include(s => s.Participant)
                .Include(s => s.User)
                .Where(s => s.Booking.UserId == userId)
                .ToList();
        }

        public Sample GetById(int id)
        {
            return _context.Samples
                .Include(s => s.Booking)
                .ThenInclude(b => b.User)
                .Include(s => s.Participant)
                .Include(s => s.User)
                .FirstOrDefault(s => s.SampleId == id);
        }

        public List<Sample> Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return GetAll();

            searchText = searchText.ToLower();
            return _context.Samples
                .Include(s => s.Booking)
                .ThenInclude(b => b.User)
                .Include(s => s.Participant)
                .Include(s => s.User)
                .Where(s => 
                    s.SampleId.ToString().Contains(searchText) ||
                    (s.TypeOfCollection != null && s.TypeOfCollection.ToLower().Contains(searchText)) ||
                    (s.SampleType != null && s.SampleType.ToLower().Contains(searchText)) ||
                    (s.Participant != null && s.Participant.FullName.ToLower().Contains(searchText)) ||
                    (s.Booking != null && s.Booking.User.FullName.ToLower().Contains(searchText))
                )
                .ToList();
        }

        public void Add(Sample sample)
        {
            _context.Samples.Add(sample);
            _context.SaveChanges();
        }

        public void Update(Sample sample)
        {
            var existingSample = _context.Samples.Find(sample.SampleId);
            if (existingSample != null)
            {
                _context.Entry(existingSample).CurrentValues.SetValues(sample);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var sample = _context.Samples.Find(id);
            if (sample != null)
            {
                _context.Samples.Remove(sample);
                _context.SaveChanges();
            }
        }
    }
} 