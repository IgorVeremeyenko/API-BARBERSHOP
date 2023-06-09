using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Statistics.Controllers.Get {
    public class GetStatListService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<IEnumerable<Statistic>>> Init() {

            if (_context.Statistics == null) {
                return new NotFoundResult();
            }
            return await _context.Statistics.ToListAsync();
        }
    }
}
