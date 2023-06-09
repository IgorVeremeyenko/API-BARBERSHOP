using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Appointments.Controllers.Get
{
    public class GetAppListService
    {
        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<IEnumerable<Appointment>>> InitList(
            CacheService _cacheService,
            string cacheAllAppointmentsKey,
            int id)
        {

            if (_context.Appointments == null)
            {
                return new NotFoundResult();
            }

            cacheAllAppointmentsKey += id.ToString();

            var appointments = new List<Appointment>();

            appointments = _cacheService.Get<List<Appointment>>(cacheAllAppointmentsKey);

            if (appointments == null)
            {
                appointments = await _context.Appointments.Where(t => t.UserId == id).ToListAsync();
                _cacheService.Set(cacheAllAppointmentsKey, appointments, TimeSpan.FromHours(12));
            }

            return new OkObjectResult(appointments);
        }
    }
}
