using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class ConsultantRepository
    {
        private readonly DnatestingServiceContext _context;

        public ConsultantRepository()
        {
            _context = new DnatestingServiceContext();
        }

        public List<Consultant> GetAll()
        {
            return _context.Consultants
                .Include(c => c.User)
                .ToList();
        }

        public List<Consultant> GetByUserId(int userId)
        {
            return _context.Consultants
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToList();
        }

        public Consultant GetById(int id)
        {
            return _context.Consultants
                .Include(c => c.User)
                .FirstOrDefault(c => c.ConsultantId == id);
        }

        public List<Consultant> Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return GetAll();

            searchText = searchText.ToLower();
            
            return _context.Consultants
                .Include(c => c.User)
                .Where(c => 
                    c.Content.ToLower().Contains(searchText) || 
                    (c.Notes != null && c.Notes.ToLower().Contains(searchText)) || 
                    (c.Status != null && c.Status.ToLower().Contains(searchText)) ||
                    c.Name.ToLower().Contains(searchText) ||
                    c.Phone.ToLower().Contains(searchText) ||
                    c.Type.ToLower().Contains(searchText) ||
                    c.User.FullName.ToLower().Contains(searchText))
                .ToList();
        }

        public void Add(Consultant consultant)
        {
            _context.Consultants.Add(consultant);
            _context.SaveChanges();
        }

        public void Update(Consultant consultant)
        {
            var existing = _context.Consultants.Find(consultant.ConsultantId);
            if (existing == null)
                throw new Exception("Không tìm thấy yêu cầu tư vấn");

            // Cập nhật thông tin
            _context.Entry(existing).CurrentValues.SetValues(consultant);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var consultant = _context.Consultants.Find(id);
            if (consultant != null)
            {
                _context.Consultants.Remove(consultant);
                _context.SaveChanges();
            }
        }
    }
}