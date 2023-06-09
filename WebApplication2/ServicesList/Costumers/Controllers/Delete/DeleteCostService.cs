using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Costumers.Controllers.Delete {
    public class DeleteCostService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(
            CacheService cacheService,
            string cacheAllCostumersKey,
            int id, int adminId) {

            if (_context.Costumers == null) {
                return new NotFoundResult();
            }
            cacheAllCostumersKey += adminId.ToString();
            var costumer = await _context.Costumers.FindAsync(id);
            if (costumer == null) {
                return new NotFoundResult();
            }

            _context.Costumers.Remove(costumer);
            cacheService.Delete(cacheAllCostumersKey);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
