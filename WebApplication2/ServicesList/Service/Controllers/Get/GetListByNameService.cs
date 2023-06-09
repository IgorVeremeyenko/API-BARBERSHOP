using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Service.Controllers.Get {
    public class GetListByNameService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<IEnumerable<Models.Service>>> Init(int id) {

            if (_context.Services == null) {
                return new NotFoundResult();
            }
            var services = await _context.Services.Where(t => t.UserId == id).ToListAsync();
            ServiceListGroupedByName serviceListGroupedByCategory = new ServiceListGroupedByName();
            var result = serviceListGroupedByCategory.GetNames(services);

            return result;
        }
    }
}
