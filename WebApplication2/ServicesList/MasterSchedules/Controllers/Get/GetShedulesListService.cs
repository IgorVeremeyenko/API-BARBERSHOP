using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.MasterSchedules.Controllers.Get {
    public class GetShedulesListService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<IEnumerable<MasterSchedule>>> Init() {

            if (_context.MasterSchedules == null) {
                return new NotFoundResult();
            }
            return await _context.MasterSchedules.ToListAsync();
        }
    }
}
