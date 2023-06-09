using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Masters.Controllers.Get {
    public class GetMasterByIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<Master>> Init(int id) {

            if (_context.Masters == null) {
                return new NotFoundResult();
            }
            var master = await _context.Masters.FindAsync(id);

            if (master == null) {
                return new NotFoundResult();
            }

            return master;
        }
    }
}
