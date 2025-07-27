using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class ParticipantRepository
    {
        private readonly DnatestingServiceContext _context;
        
        public ParticipantRepository()
        {
            _context = new DnatestingServiceContext();  
        }
        
        public List<Participant> GetAll()
        {
            return _context.Participants
                .Include(p => p.Samples)
                .ThenInclude(s => s.Booking)
                .ThenInclude(b => b.User)
                .ToList();
        }

        public List<Participant> GetByBookingId(int bookingId)
        {
            return _context.Participants
                .Include(p => p.Samples)
                .ThenInclude(s => s.Booking)
                .ThenInclude(b => b.User)
                .Where(p => p.Samples.Any(s => s.BookingId == bookingId))
                .ToList();
        }

        public List<Participant> GetByUserId(int userId)
        {
            return _context.Participants
                .Include(p => p.Samples)
                .ThenInclude(s => s.Booking)
                .ThenInclude(b => b.User)
                .Where(p => p.Samples.Any(s => s.Booking.UserId == userId))
                .ToList();
        }

        public Participant GetById(int id)
        {
            return _context.Participants
                .Include(p => p.Samples)
                .ThenInclude(s => s.Booking)
                .ThenInclude(b => b.User)
                .FirstOrDefault(p => p.ParticipantId == id);
        }

        public List<Participant> Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return GetAll();

            searchText = searchText.ToLower();
            return _context.Participants
                .Include(p => p.Samples)
                .ThenInclude(s => s.Booking)
                .ThenInclude(b => b.User)
                .Where(p => 
                    p.ParticipantId.ToString().Contains(searchText) ||
                    (p.FullName != null && p.FullName.ToLower().Contains(searchText)) ||
                    (p.IdentityNumber != null && p.IdentityNumber.ToLower().Contains(searchText)) ||
                    (p.RelationshipToCustomer != null && p.RelationshipToCustomer.ToLower().Contains(searchText)) ||
                    (p.Gender != null && p.Gender.ToLower().Contains(searchText))
                )
                .ToList();
        }

        public void Add(Participant participant)
        {
            _context.Participants.Add(participant);
            _context.SaveChanges();
        }

        public void Update(Participant participant)
        {
            var existingParticipant = _context.Participants.Find(participant.ParticipantId);
            if (existingParticipant != null)
            {
                _context.Entry(existingParticipant).CurrentValues.SetValues(participant);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var participant = _context.Participants.Find(id);
            if (participant != null)
            {
                _context.Participants.Remove(participant);
                _context.SaveChanges();
            }
        }
    }
} 