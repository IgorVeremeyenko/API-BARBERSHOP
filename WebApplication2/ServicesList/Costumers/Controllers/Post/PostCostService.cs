using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.Costumer;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Costumers.Controllers.Post
{
    public class PostCostService {

        private readonly MyDatabaseContext _context = new();

        public async Task<ActionResult<Costumer>?> Init(
            CacheService cacheService,
            Costumer costumer,
            string cacheAllCostumersKey,
            int id) {

            if (_context.Costumers == null) {
                return null;
            }
            cacheAllCostumersKey += id.ToString();
            _context.Costumers.Add(costumer);
            cacheService.Delete(cacheAllCostumersKey);
            await _context.SaveChangesAsync();
            return costumer;
        }
    }
}
