using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository
    {
        private readonly DnatestingServiceContext _context;

        public UserRepository()
        {
            _context = new DnatestingServiceContext();
        }

        public User? GetUserByEmailAndPassword(string email, string password)
        {
            return _context.Users.Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email && u.Password == password && u.Status == "Active");
        }
    }
}
