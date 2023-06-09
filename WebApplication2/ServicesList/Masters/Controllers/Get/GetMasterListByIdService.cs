using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services.Appointments;

namespace WebApplication2.ServicesList.Masters.Controllers.Get {
    public class GetMasterListByIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public int Init(int id) {

            var appointments = _context.Appointments.ToList();
            CounterHowManyAppointments counter = new CounterHowManyAppointments();

            return counter.Count(appointments, id);
        }
    }
}
