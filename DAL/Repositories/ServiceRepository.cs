using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class ServiceRepository
    {
        private readonly DnatestingServiceContext _context;
        
        public ServiceRepository()
        {
            _context = new DnatestingServiceContext();  
        }
        
        public List<Service> GetAll()
        {
            return _context.Services.Where(s => s.Status == "Hoạt động").ToList();
        }

        public Service GetById(int id)
        {
            return _context.Services.Find(id);
        }
    }
}