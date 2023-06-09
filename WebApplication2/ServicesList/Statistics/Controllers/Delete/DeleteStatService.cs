using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Statistics.Controllers.Delete {
    public class DeleteStatService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(int id) {

            if (_context.Statistics == null) {
                return new NotFoundResult();
            }
            var statistic = await _context.Statistics.FindAsync(id);
            if (statistic == null) {
                return new NotFoundResult();
            }

            _context.Statistics.Remove(statistic);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
