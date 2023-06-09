using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models.Costumer;
using WebApplication2.Services.Cache;
using WebApplication2.ServicesList.Costumers.Controllers.Delete;
using WebApplication2.ServicesList.Costumers.Controllers.Get;
using WebApplication2.ServicesList.Costumers.Controllers.Post;
using WebApplication2.ServicesList.Costumers.Controllers.Put;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostumersController : ControllerBase
    {

        private readonly CacheService _cacheService;

        private readonly string cacheAllCostumersKey = "costumers_cache_key";

        private readonly GetCostListService _costListService;

        private readonly GetCostByIdService _costByIdService;

        private readonly PutCostByIdService _putCostByIdService;

        private readonly PostCostService _postCostService;

        private readonly DeleteCostService _deleteCostService;

        public CostumersController(
            CacheService cacheService,
            GetCostListService costListService,
            GetCostByIdService costByIdService,
            PutCostByIdService putCostByIdService,
            PostCostService postCostService,
            DeleteCostService deleteCostService) {
            _cacheService = cacheService;
            _costListService = costListService;
            _costByIdService = costByIdService;
            _putCostByIdService = putCostByIdService;
            _postCostService = postCostService;
            _deleteCostService = deleteCostService;
        }

        // GET: api/Costumers
        [HttpGet("allCostumers/{currentAdminId}")]
        public async Task<ActionResult<IEnumerable<CostumersWithRating>>> GetCostumers(int currentAdminId)
        {
            return await _costListService.Init( _cacheService, cacheAllCostumersKey, currentAdminId);
        }

        // GET: api/Costumers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Costumer>> GetCostumer(int id)
        {
            return await _costByIdService.Init( id);
        }

        // PUT: api/Costumers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCostumer(int id, Costumer costumer, int adminId)
        {
            return await _putCostByIdService.Init( id, costumer, cacheAllCostumersKey,adminId,_cacheService);
        }

        // POST: api/Costumers
        [HttpPost("{id}")]
        public async Task<ActionResult<Costumer>> PostCostumer(Costumer costumer, int id)
        {
            
            var obj = _postCostService.Init(_cacheService,costumer,cacheAllCostumersKey,id);
            if(obj == null) {
                return Problem("Entity set 'MyDatabaseContext.Admins'  is null.");
            }

            return CreatedAtAction("GetCostumer", new { id = costumer.Id }, costumer);
        }

        // DELETE: api/Costumers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCostumer(int id, int adminId)
        {
            return await _deleteCostService.Init( _cacheService, cacheAllCostumersKey, id, adminId);
        }

    }
}
