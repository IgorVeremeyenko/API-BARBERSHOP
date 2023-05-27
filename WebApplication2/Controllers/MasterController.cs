using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services.Appointments;
using WebApplication2.Services.Masters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase {

        private readonly MyDatabaseContext _context;

        public MasterController(MyDatabaseContext context) 
        {
            _context = context;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Master>>> GetMasters() {

            if (_context.Masters == null) {
                return NotFound();
            }
            return await _context.Masters.ToListAsync();
        }

        [HttpGet("masterList")]
        public async Task<List<MastersService>> GetMastersList() {

            var masters = await _context.Masters.ToListAsync();
            var services = await _context.Services.ToListAsync();
            var masterSchedules = await _context.MasterSchedules.ToListAsync();
            GenerateMasterList generateMasterList = new GenerateMasterList();
            var result = generateMasterList.MasterList(masters, services, masterSchedules);
            return result;
        }

        [HttpGet("masterID/{id}")]
        public int GetMastersCount(int id) {

            var appointments = _context.Appointments.ToList();
            CounterHowManyAppointments counter = new CounterHowManyAppointments();
            
            return counter.Count(appointments, id);
        }

        [HttpGet("masterName/{name}")]
        public async Task<ActionResult<Master>> GetMasterByName(string name) {


            if (_context.Masters == null) {
                return NotFound();
            }
            var master = await _context.Masters.Where(p => p.Name == name).FirstOrDefaultAsync();

            if (master == null) {
                return NotFound();
            }

            return master;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Master>> GetMaster(int id) {


            if (_context.Masters == null) {
                return NotFound();
            }
            var master = await _context.Masters.FindAsync(id);

            if (master == null) {
                return NotFound();
            }

            return master;

        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Admin>> PostMaster(Master master) {

            if (_context.Masters == null) {
                return Problem("Entity set 'MyDatabaseContext.Masters'  is null.");
            }
            _context.Masters.Add(master);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaster", new { id = master.Id }, master);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaster(int id, Master master) {

            if (id != master.Id) {
                return BadRequest();
            }

            _context.Entry(master).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!MasterExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        private bool MasterExists(int id) {
            return (_context.Masters?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaster(int id) {

            if (_context.Masters  == null) {
                return NotFound();
            }
            var master = await _context.Masters.FindAsync(id);
            if (master == null) {
                return NotFound();
            }

            _context.Masters.Remove(master);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
