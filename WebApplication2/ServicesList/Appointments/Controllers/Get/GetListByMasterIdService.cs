using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services.Appointments;

namespace WebApplication2.ServicesList.Appointments.Controllers.Get {
    public class GetListByMasterIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<IEnumerable<Appointment>>> InitGetList(int id) {

            if (_context.Appointments == null) {
                return new NotFoundResult();
            }

            var appointments = new List<Appointment>();
            appointments = await _context.Appointments.ToListAsync();
            FilterAppointmentsForMaster filterAppointmentsForMaster = new FilterAppointmentsForMaster();
            var result = filterAppointmentsForMaster.GetAppointments(id, appointments);

            return new OkObjectResult(result);
        }
    }
}
