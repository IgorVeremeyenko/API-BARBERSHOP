using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Service.Controllers.Put {
    public class PutServService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(int id, Models.Service service) {

            if (id != service.Id) {
                return new BadRequestResult();
            }

            _context.Entry(service).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!ServiceExists(id, _context)) {
                    return new NotFoundResult();
                }
                else {
                    throw;
                }
            }

            return new NoContentResult();
        }

        private bool ServiceExists(int id, MyDatabaseContext context) {
            return (context.Services?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
