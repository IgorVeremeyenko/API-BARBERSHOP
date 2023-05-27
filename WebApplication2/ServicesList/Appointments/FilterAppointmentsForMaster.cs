using WebApplication2.Models;

namespace WebApplication2.Services.Appointments
{
    public class FilterAppointmentsForMaster
    {
        public List<Appointment> GetAppointments(int id, List<Appointment> appointments)
        {
            List<Appointment> result = new List<Appointment>();
            result = appointments.Where(p => p.MasterId == id).ToList();
            return result;
        }
    }
}
