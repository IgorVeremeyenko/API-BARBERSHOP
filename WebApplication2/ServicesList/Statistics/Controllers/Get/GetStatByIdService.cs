using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Statistics.Controllers.Get {
    public class GetStatByIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<Statistic>> Init(int id) {

            if (_context.Statistics == null) {
                return new NotFoundResult();
            }
            var statistic = await _context.Statistics.FindAsync(id);

            if (statistic == null) {
                return new NotFoundResult();
            }

            return statistic;
        }
    }
}
