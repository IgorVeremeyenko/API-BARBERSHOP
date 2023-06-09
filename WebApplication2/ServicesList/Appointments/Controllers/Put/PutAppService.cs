using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Appointments.Controllers.Put {
    public class PutAppService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(
            CacheService cacheService, 
            string cacheAllAppointmentsKey,
            int id,
            int adminId,
            string cacheAllCostumersKey,
            Appointment appointment) {

            if (id != appointment.Id) {
                return new BadRequestResult();
            }

            cacheAllAppointmentsKey += adminId.ToString();
            cacheAllCostumersKey += adminId.ToString();
            cacheService.Delete(cacheAllAppointmentsKey);
            cacheService.Delete(cacheAllCostumersKey);

            _context.Entry(appointment).State = EntityState.Modified;
            cacheService.Delete(cacheAllAppointmentsKey);

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!AppointmentExists(id, _context)) {
                    return new NotFoundResult();
                }
                else {
                    throw;
                }
            }

            return new NoContentResult();
        }
        private bool AppointmentExists(int id, MyDatabaseContext context) {
            return (context.Appointments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
