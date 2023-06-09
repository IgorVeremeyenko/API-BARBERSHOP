using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.Costumer;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Costumers.Controllers.Put
{
    public class PutCostByIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(
            int id, Costumer costumer, 
            string cacheAllCostumersKey,
            int adminId,
            CacheService cacheService) {

            if (id != costumer.Id) {
                return new BadRequestResult();
            }

            cacheAllCostumersKey += adminId.ToString();

            _context.Entry(costumer).State = EntityState.Modified;

            try {
                cacheService.Delete(cacheAllCostumersKey);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!CostumerExists(id, _context)) {
                    return new NotFoundResult();
                }
                else {
                    throw;
                }
            }

            return new NoContentResult();
        }
        private bool CostumerExists(int id, MyDatabaseContext context) {
            return (context.Costumers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
