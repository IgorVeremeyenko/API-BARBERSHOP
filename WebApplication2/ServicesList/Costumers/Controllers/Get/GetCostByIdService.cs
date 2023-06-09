using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.Costumer;

namespace WebApplication2.ServicesList.Costumers.Controllers.Get
{
    public class GetCostByIdService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<Costumer>> Init(int id) {

            if (_context.Costumers == null) {
                return new NotFoundResult();
            }
            var costumer = new Costumer();

            costumer = await _context.Costumers.FindAsync(id);


            if (costumer == null) {
                return new NotFoundResult();
            }

            return costumer;
        }
    }
}
