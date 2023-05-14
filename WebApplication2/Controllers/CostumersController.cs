using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services.Cache;
using WebApplication2.Services.Costumers;
using static WebApplication2.Services.Costumers.CalculateRating;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostumersController : ControllerBase
    {
        private readonly MyDatabaseContext _context;

        private readonly CacheService _cacheService;

        private readonly string cacheAllCostumersKey = "costumers_cache_key";

        private readonly string cacheOnlyCostumerKey = "costumer_by_id_cache_key";

        private CalculateRating _rating;

        public CostumersController(MyDatabaseContext context, CacheService cacheService, CalculateRating rating)
        {
            _context = context;

            _cacheService = cacheService;

            _rating = rating;
        }

        // GET: api/Costumers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostumersWithRating>>> GetCostumers()
        {
          if (_context.Costumers == null)
          {
              return NotFound();
          }
            var costumers = new List<Costumer>();

            var ratingCostumers = new List<CostumersWithRating>();

            var isCache = _cacheService.TryGetValueFromList(cacheAllCostumersKey, costumers);

            if (!isCache) {
                costumers = await _context.Costumers.Include(p => p.Appointments).Include(s => s.Statistics).ToListAsync();
                ratingCostumers = _rating.Rating(costumers);
                _cacheService.Set(cacheAllCostumersKey, ratingCostumers, TimeSpan.FromHours(12));
            }
            else {

                ratingCostumers = _cacheService.Get<List<CostumersWithRating>>(cacheAllCostumersKey);
            }



            return Ok(ratingCostumers);
        }

        // GET: api/Costumers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Costumer>> GetCostumer(int id)
        {
          if (_context.Costumers == null)
          {
              return NotFound();
          }
            var costumer = new Costumer();

            var isCache = _cacheService.TryGetValueSingle(cacheOnlyCostumerKey, costumer);

            if (!isCache) {
                costumer = await _context.Costumers.FindAsync(id);
                _cacheService.Set(cacheOnlyCostumerKey, costumer, TimeSpan.FromHours(12));
            }
            else {
                /*costumer = _cacheService.Get<Costumer>(cacheOnlyCostumerKey);*/
                costumer = await _context.Costumers.FindAsync(id);
            }

            if (costumer == null)
            {
                return NotFound();
            }

            return costumer;
        }

        // PUT: api/Costumers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCostumer(int id, Costumer costumer)
        {
            if (id != costumer.Id)
            {
                return BadRequest();
            }

            _context.Entry(costumer).State = EntityState.Modified;

            try
            {
                _cacheService.Delete(cacheAllCostumersKey);
                _cacheService.Delete(cacheOnlyCostumerKey);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostumerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Costumers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Costumer>> PostCostumer(Costumer costumer)
        {
          if (_context.Costumers == null)
          {
              return Problem("Entity set 'MyDatabaseContext.Costumers'  is null.");
          }
            _context.Costumers.Add(costumer);
            _cacheService.Delete(cacheAllCostumersKey);
            _cacheService.Delete(cacheOnlyCostumerKey);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCostumer", new { id = costumer.Id }, costumer);
        }

        // DELETE: api/Costumers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCostumer(int id)
        {
            if (_context.Costumers == null)
            {
                return NotFound();
            }
            var costumer = await _context.Costumers.FindAsync(id);
            if (costumer == null)
            {
                return NotFound();
            }

            _context.Costumers.Remove(costumer);
            _cacheService.Delete(cacheAllCostumersKey);
            _cacheService.Delete(cacheOnlyCostumerKey);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CostumerExists(int id)
        {
            return (_context.Costumers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
