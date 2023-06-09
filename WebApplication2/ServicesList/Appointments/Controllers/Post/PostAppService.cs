using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Appointments.Controllers.Post {
    public class PostAppService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<Appointment>?> Init(
            Appointment appointment,
            CacheService cacheService,
            string cacheAllAppointmentsKey,
            int id, string cacheAllCostumersKey) {

            if (_context.Appointments == null) {
                return null;
            }
            cacheAllAppointmentsKey += id.ToString();
            cacheAllCostumersKey += id.ToString();
            cacheService.Delete(cacheAllAppointmentsKey);
            cacheService.Delete(cacheAllCostumersKey);
            int timezoneOffset = appointment.TimezoneOffset;
            DateTime utcTime = appointment.Date;
            DateTime localTime = utcTime.AddMinutes(-timezoneOffset);
            appointment.Date = localTime;
            _context.Appointments.Add(appointment);
            cacheService.Delete(cacheAllAppointmentsKey);
            await _context.SaveChangesAsync();
            return appointment;
        }
    }
}
