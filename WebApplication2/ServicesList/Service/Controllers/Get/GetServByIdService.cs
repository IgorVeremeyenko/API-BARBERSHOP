using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Service.Controllers.Get {
    public class GetServByIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<Models.Service>> Init(int id) {

            if (_context.Services == null) {
                return new NotFoundResult();
            }
            var service = await _context.Services.FindAsync(id);

            if (service == null) {
                return new NotFoundResult();
            }

            return service;
        }
    }
}
