using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.MasterSchedules.Controllers.Put {
    public class PutShedulesService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(int id, MasterSchedule masterSchedule) {

            if (id != masterSchedule.Id) {
                return new BadRequestResult();
            }

            _context.Entry(masterSchedule).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!MasterScheduleExists(id, _context)) {
                    return new NotFoundResult();
                }
                else {
                    throw;
                }
            }

            return new NoContentResult();
        }

        private bool MasterScheduleExists(int id, MyDatabaseContext context) {
            return (context.MasterSchedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
