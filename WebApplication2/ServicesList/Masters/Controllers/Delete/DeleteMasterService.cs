using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Masters.Controllers.Delete {
    public class DeleteMasterService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(int id) {

            if (_context.Masters == null) {
                return new NotFoundResult();
            }
            var master = await _context.Masters.FindAsync(id);
            if (master == null) {
                return new NotFoundResult();
            }

            _context.Masters.Remove(master);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
