using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.MasterSchedules.Controllers.Get {
    public class GetShedulesByIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<MasterSchedule>> Init(int id) {

            if (_context.MasterSchedules == null) {
                return new NotFoundResult();
            }
            var masterSchedule = await _context.MasterSchedules.FindAsync(id);

            if (masterSchedule == null) {
                return new NotFoundResult();
            }

            return masterSchedule;
        }
    }
}
