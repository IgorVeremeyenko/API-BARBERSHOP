using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using WebApplication2.Models;
using WebApplication2.Services.Appointments;
using WebApplication2.Services.Cache;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly MyDatabaseContext _context;

        private readonly string cacheAllAppointmentsKey = "appointments_cache_key";

        private readonly string cacheOnlyAppointmentKey = "appointment_by_id_cache_key";

        private readonly string cacheAllCostumersKey = "costumers_cache_key";

        private readonly string cacheOnlyCostumerKey = "costumer_by_id_cache_key";

        private readonly CacheService _cacheService;

        public AppointmentsController(MyDatabaseContext context, CacheService cacheService)
        {
            _context = context;

            _cacheService = cacheService;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
          if (_context.Appointments == null)
          {
              return NotFound();
          }

            var appointments = new List<Appointment>();

            var isCache = _cacheService.TryGetValueFromList(cacheAllAppointmentsKey, appointments);

            if (!isCache) {
                appointments = await _context.Appointments.ToListAsync();
                _cacheService.Set(cacheAllAppointmentsKey, appointments, TimeSpan.FromHours(12));
            }
            else {
                /*appointments = _cacheService.Get<List<Appointment>>(cacheAllAppointmentsKey);*/
                appointments = await _context.Appointments.ToListAsync();
            }

            return Ok(appointments);

        }

        [HttpGet("appointmentsByMasterID/{id}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByMaster(int id) {

            if (_context.Appointments == null) {
                return NotFound();
            }

            var appointments = new List<Appointment>();
            appointments = await _context.Appointments.ToListAsync();
            FilterAppointmentsForMaster filterAppointmentsForMaster = new FilterAppointmentsForMaster();
            var result = filterAppointmentsForMaster.GetAppointments(id, appointments);

            return Ok(result);

        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
          if (_context.Appointments == null)
          {
              return NotFound();
          }

          var appointment = new Appointment();

            var isCache = _cacheService.TryGetValueSingle(cacheOnlyAppointmentKey, appointment);

            if (!isCache) {
                appointment = await _context.Appointments.FindAsync(id);
                _cacheService.Set(cacheOnlyAppointmentKey, appointment, TimeSpan.FromHours(12));
            }
            else {
                /* appointment = _cacheService.Get<Appointment>(cacheOnlyAppointmentKey);*/
                appointment = await _context.Appointments.FindAsync(id);
            }

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }


        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;
            _cacheService.Delete(cacheAllAppointmentsKey);
            _cacheService.Delete(cacheOnlyAppointmentKey);
            _cacheService.Delete(cacheAllCostumersKey);
            _cacheService.Delete(cacheOnlyCostumerKey);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Appointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
          if (_context.Appointments == null)
          {
              return Problem("Entity set 'MyDatabaseContext.Appointments'  is null.");
          }
            int timezoneOffset = appointment.TimezoneOffset;
            DateTime utcTime = appointment.Date;
            DateTime localTime = utcTime.AddMinutes(-timezoneOffset);
            appointment.Date = localTime;
            _context.Appointments.Add(appointment);
            _cacheService.Delete(cacheAllAppointmentsKey);
            _cacheService.Delete(cacheOnlyAppointmentKey);
            _cacheService.Delete(cacheAllCostumersKey);
            _cacheService.Delete(cacheOnlyCostumerKey);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            if (_context.Appointments == null)
            {
                return NotFound();
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            _cacheService.Delete(cacheAllAppointmentsKey);
            _cacheService.Delete(cacheOnlyAppointmentKey);
            _cacheService.Delete(cacheAllCostumersKey);
            _cacheService.Delete(cacheOnlyCostumerKey);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return (_context.Appointments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
