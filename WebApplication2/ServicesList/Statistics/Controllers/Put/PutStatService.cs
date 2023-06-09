using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Statistics.Controllers.Put {
    public class PutStatService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(int id, Statistic statistic) {

            if (id != statistic.Id) {
                return new BadRequestResult();
            }

            _context.Entry(statistic).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!StatisticExists(id, _context)) {
                    return new NotFoundResult();
                }
                else {
                    throw;
                }
            }

            return new NoContentResult();
        }

        private bool StatisticExists(int id, MyDatabaseContext context) {
            return (context.Statistics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
