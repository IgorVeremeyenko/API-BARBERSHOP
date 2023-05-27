using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.TreeNodes;
using WebApplication2.Services;
using WebApplication2.ServicesList;
using WebApplication2.ServicesList.Service;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase {
        private readonly MyDatabaseContext _context;

        public ServicesController(MyDatabaseContext context) {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices() {
            if (_context.Services == null) {
                return NotFound();
            }
            return await _context.Services.ToListAsync();
        }
        [HttpGet("TreeNode")]
        public async Task<ActionResult<IEnumerable<TreeNode>>> GetTreeNodeServices() {
            if (_context.Services == null) {
                return NotFound();
            }
            var services = await _context.Services.ToListAsync();
            GenerateTreeNodeListServices generateTreeNodeListServices = new GenerateTreeNodeListServices();
            var result = generateTreeNodeListServices.GetTreeNode(_context, services);
            return result;
        }

        [HttpGet("appointmentFilter")]
        public async Task<ActionResult<IEnumerable<Categorized>>> GetServicesForAppointment() {
            if (_context.Services == null) {
                return NotFound();
            }
            var services = await _context.Services.ToListAsync();
            FilteredAppointments filteredAppointments = new FilteredAppointments();
            var result = filteredAppointments.GetList(services);

            return result;
        }

        [HttpGet("listGroupedByCategory")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesListCategory() {
            if (_context.Services == null) {
                return NotFound();
            }
            var services = await _context.Services.ToListAsync();
            ServiceListGroupedByCategory serviceListGroupedByCategory = new ServiceListGroupedByCategory();
            var result = serviceListGroupedByCategory.GetCategories(services);

            return result;
        }

        [HttpGet("listGroupedByName")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesListName() {
            if (_context.Services == null) {
                return NotFound();
            }
            var services = await _context.Services.ToListAsync();
            ServiceListGroupedByName serviceListGroupedByCategory = new ServiceListGroupedByName();
            var result = serviceListGroupedByCategory.GetNames(services);

            return result;
        }

        [HttpGet("loadServicesForEditComponent/{category}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesForEditing(string category) {
            if (_context.Services == null) {
                return NotFound();
            }
            var services = await _context.Services.ToListAsync();
            FilteredByName filteredAppointments = new FilteredByName();
            var result = filteredAppointments.GetList(category, services);

            return result;
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
          if (_context.Services == null)
          {
              return NotFound();
          }
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            if (id != service.Id)
            {
                return BadRequest();
            }

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
          if (_context.Services == null)
          {
              return Problem("Entity set 'MyDatabaseContext.Services'  is null.");
          }
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetService", new { id = service.Id }, service);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            if (_context.Services == null)
            {
                return NotFound();
            }
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return (_context.Services?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
