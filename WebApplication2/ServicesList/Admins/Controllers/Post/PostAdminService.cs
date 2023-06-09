using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Admins.Controllers.Post {
    public class PostAdminService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<Admin>?> InitPost(Admin admin) {

            if (_context.Admins == null) {
                return null;
            }
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return admin;
        }
    }
}
