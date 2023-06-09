using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Admins.Controllers.Get {
    public class GetListService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<IEnumerable<Admin>>> InitList() {

            if (_context.Admins == null) {
                return new NotFoundResult();
            }
            return (ActionResult<IEnumerable<Admin>>)await _context.Admins.ToListAsync();
        }
    }
}
