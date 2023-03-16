using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly MyDatabaseContext _context;

        public StatisticsController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Statistics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Statistic>>> GetStatistics()
        {
          if (_context.Statistics == null)
          {
              return NotFound();
          }
            return await _context.Statistics.ToListAsync();
        }

        // GET: api/Statistics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Statistic>> GetStatistic(int id)
        {
          if (_context.Statistics == null)
          {
              return NotFound();
          }
            var statistic = await _context.Statistics.FindAsync(id);

            if (statistic == null)
            {
                return NotFound();
            }

            return statistic;
        }

        // PUT: api/Statistics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatistic(int id, Statistic statistic)
        {
            if (id != statistic.Id)
            {
                return BadRequest();
            }

            _context.Entry(statistic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatisticExists(id))
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

        // POST: api/Statistics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Statistic>> PostStatistic(Statistic statistic)
        {
          if (_context.Statistics == null)
          {
              return Problem("Entity set 'MyDatabaseContext.Statistics'  is null.");
          }
            _context.Statistics.Add(statistic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatistic", new { id = statistic.Id }, statistic);
        }

        // DELETE: api/Statistics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatistic(int id)
        {
            if (_context.Statistics == null)
            {
                return NotFound();
            }
            var statistic = await _context.Statistics.FindAsync(id);
            if (statistic == null)
            {
                return NotFound();
            }

            _context.Statistics.Remove(statistic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatisticExists(int id)
        {
            return (_context.Statistics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
