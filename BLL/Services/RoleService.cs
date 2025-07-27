using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepo;

        public RoleService()
        {
            _roleRepo = new RoleRepository();
        }

        public List<Role> GetAllRoles() => _roleRepo.GetAllRoles();
        public Role? GetRoleById(int id) => _roleRepo.GetRoleById(id);
    }

}
