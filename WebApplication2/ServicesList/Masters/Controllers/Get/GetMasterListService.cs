using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Masters.Controllers.Get {
    public class GetMasterListService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<IEnumerable<Master>>> Init(int id) {

            if (_context.Masters == null) {
                return new NotFoundResult();
            }
            return await _context.Masters.Where(t => t.UserId == id).ToListAsync();
        }
    }
}
