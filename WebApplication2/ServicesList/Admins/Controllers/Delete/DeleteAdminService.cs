using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Admins.Controllers.Delete {
    public class DeleteAdminService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> InitDel(int id) {

            if (_context.Admins == null) {
                return new NotFoundResult();
            }
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) {
                return new NotFoundResult();
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
