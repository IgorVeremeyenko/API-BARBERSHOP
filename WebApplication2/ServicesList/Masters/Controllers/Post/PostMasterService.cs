using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Masters.Controllers.Post {
    public class PostMasterService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();
        public async Task<ActionResult<Master>?> Init(Master master) {

            if (_context.Masters == null) {
                return null;
            }
            _context.Masters.Add(master);
            await _context.SaveChangesAsync();

            return master;
        }

    }
}
