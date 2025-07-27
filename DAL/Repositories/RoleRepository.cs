using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class RoleRepository
    {
        private readonly DnatestingServiceContext _context;

        public RoleRepository()
        {
            _context = new DnatestingServiceContext();
        }

        /// <summary>
        /// Lấy tất cả các vai trò từ bảng Role
        /// </summary>
        /// <returns>Danh sách Role</returns>
        public List<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        /// <summary>
        /// Tìm Role theo RoleId
        /// </summary>
        public Role? GetRoleById(int roleId)
        {
            return _context.Roles.FirstOrDefault(r => r.RoleId == roleId);
        }

        /// <summary>
        /// Tìm Role theo tên
        /// </summary>
        public Role? GetRoleByName(string roleName)
        {
            return _context.Roles.FirstOrDefault(r => r.RoleName.ToLower() == roleName.ToLower());
        }

        /// <summary>
        /// Thêm Role mới
        /// </summary>
        public void CreateRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        /// <summary>
        /// Cập nhật Role
        /// </summary>
        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        /// <summary>
        /// Xóa Role
        /// </summary>
        public void DeleteRole(int roleId)
        {
            var role = _context.Roles.Find(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
            }
        }
    }
}
