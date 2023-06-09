using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.Costumer;
using WebApplication2.Services.Cache;
using WebApplication2.Services.Costumers;

namespace WebApplication2.ServicesList.Costumers.Controllers.Get
{
    public class GetCostListService {

        private CalculateRating _rating = new CalculateRating();

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<IEnumerable<CostumersWithRating>>> Init(
            CacheService cacheService,
            string cacheAllCostumersKey,
            int id) {

            if (_context.Costumers == null) {
                return new NotFoundResult();
            }

            cacheAllCostumersKey += id.ToString();

            var costumers = new List<Costumer>();

            var ratingCostumers = new List<CostumersWithRating>();

            ratingCostumers = cacheService.Get<List<CostumersWithRating>>(cacheAllCostumersKey);

            if (ratingCostumers == null) {
                costumers = await _context.Costumers.Where(t => t.UserId == id).Include(p => p.Appointments).Include(s => s.Statistics).ToListAsync();
                ratingCostumers = _rating.Rating(costumers);
                cacheService.Set(cacheAllCostumersKey, ratingCostumers, TimeSpan.FromHours(12));
            }
            
            return new OkObjectResult(ratingCostumers);
        }
    }
}
