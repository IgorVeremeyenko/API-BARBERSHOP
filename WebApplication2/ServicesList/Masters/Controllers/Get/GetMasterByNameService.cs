using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Masters.Controllers.Get {
    public class GetMasterByNameService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<Master>> Init(string name, int id) {

            if (_context.Masters == null) {
                return new NotFoundResult();
            }
            var master = await _context.Masters.Where(p => p.Name == name && p.UserId == id).FirstOrDefaultAsync();

            if (master == null) {
                return new NotFoundResult();
            }

            return master;
        }
    }
}
