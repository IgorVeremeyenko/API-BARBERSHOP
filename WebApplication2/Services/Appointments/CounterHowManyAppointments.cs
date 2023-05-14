using WebApplication2.Models;

namespace WebApplication2.Services.Appointments
{
    public class CounterHowManyAppointments
    {

        public int Count(List<Appointment> appointments, int id)
        {
            int count = appointments.Where(p => p.MasterId == id && p.Status == "Выполняется").Count();
            return count;
        }
    }
}
