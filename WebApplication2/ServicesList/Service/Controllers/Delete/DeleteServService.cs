using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Service.Controllers.Delete {
    public class DeleteServService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(int id) {

            if (_context.Services == null) {
                return new NotFoundResult();
            }
            var service = await _context.Services.FindAsync(id);
            if (service == null) {
                return new NotFoundResult();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
