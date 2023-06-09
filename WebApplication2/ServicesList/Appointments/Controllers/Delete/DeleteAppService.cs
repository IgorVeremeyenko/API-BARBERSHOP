using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Appointments.Controllers.Delete {
    public class DeleteAppService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(
            int id, 
            CacheService cacheService,
            string cacheAllAppointmentsKey,
            int adminId, string cacheAllCostumersKey) {

            if (_context.Appointments == null) {
                return new NotFoundResult();
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) {
                return new NotFoundResult();
            }

            cacheAllAppointmentsKey += adminId.ToString();
            cacheAllCostumersKey += adminId.ToString();
            cacheService.Delete(cacheAllAppointmentsKey);
            cacheService.Delete(cacheAllCostumersKey);

            _context.Appointments.Remove(appointment);
            cacheService.Delete(cacheAllAppointmentsKey);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
