using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Service.Controllers.Get {
    public class GetAppFilterService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<IEnumerable<Categorized>>> Init(int id) {

            if (_context.Services == null) {
                return new NotFoundResult();
            }
            var services = await _context.Services.Where(t => t.UserId == id).ToListAsync();
            FilteredAppointments filteredAppointments = new FilteredAppointments();
            var result = filteredAppointments.GetList(services);

            return result;
        }
    }
}
