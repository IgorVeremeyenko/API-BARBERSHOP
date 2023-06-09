using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.Masters.Controllers.Put {
    public class PutMasterService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(int id, Master master) {

            if(id != master.Id) {
                return new BadRequestResult();
            }

            _context.Entry(master).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!MasterExists(id, _context)) {
                    return new NotFoundResult();
                }
                else {
                    throw;
                }
            }

            return new NoContentResult();
        }
        private bool MasterExists(int id, MyDatabaseContext context) {
            return (context.Masters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
