using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Statistics.Controllers.Post {
    public class PostStatService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<Statistic>?> Init(Statistic statistic) {

            if (_context.Statistics == null) {
                return null;
            }
            _context.Statistics.Add(statistic);
            await _context.SaveChangesAsync();

            return statistic;
        }
    }
}
