using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Service.Controllers.Post {
    public class PostServService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();
        public async Task<ActionResult<Models.Service>?> Init(Models.Service service) {

            if (_context.Services == null) {
                return new BadRequestResult();
            }
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return service;
        }
    }
}
