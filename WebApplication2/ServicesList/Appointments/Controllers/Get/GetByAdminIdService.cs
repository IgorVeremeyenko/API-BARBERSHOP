using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Appointments.Controllers.Get {
    public class GetByAdminIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<Appointment>> Init(int id) {

            if (_context.Appointments == null) {
                return new NotFoundResult();
            }

            Appointment appointment = new Appointment();

            appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) {
                return new NotFoundResult();
            }

            return appointment;
        }
    }
}
