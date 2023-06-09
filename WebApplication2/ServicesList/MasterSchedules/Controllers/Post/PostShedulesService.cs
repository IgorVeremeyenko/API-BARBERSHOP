using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.MasterSchedules.Controllers.Post {
    public class PostShedulesService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();
        public async Task<ActionResult<MasterSchedule>?> Init(MasterSchedule masterSchedule) {

            if (_context.MasterSchedules == null) {
                return null;
            }
            _context.MasterSchedules.Add(masterSchedule);
            await _context.SaveChangesAsync();

            return masterSchedule;
        }
    }
}
