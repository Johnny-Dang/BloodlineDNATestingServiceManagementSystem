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

        public ServiceRepository(DnatestingServiceContext context)
        {
            _context = context;
        }
        
        public List<Service> GetAll()
        {
            return _context.Services.Where(s => s.Status == "Hoạt động").ToList();
        }

        public Service GetById(int id)
        {
            return _context.Services.Find(id);
        }

        public void Add(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public void Update(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Services.Find(id);
            if (entity != null)
            {
                _context.Services.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}