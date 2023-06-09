using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Admins.Controllers.Get {
    public class GetByIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<Admin>> initId(int id) {

            if (_context.Admins == null) {
                return new NotFoundResult();
            }
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null) {
                return new NotFoundResult();
            }

            return admin;
        }
    }
}
