using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterSchedulesController : ControllerBase
    {
        private readonly MyDatabaseContext _context;

        public MasterSchedulesController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/MasterSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MasterSchedule>>> GetMasterSchedules()
        {
          if (_context.MasterSchedules == null)
          {
              return NotFound();
          }
            return await _context.MasterSchedules.ToListAsync();
        }

        // GET: api/MasterSchedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MasterSchedule>> GetMasterSchedule(int id)
        {
          if (_context.MasterSchedules == null)
          {
              return NotFound();
          }
            var masterSchedule = await _context.MasterSchedules.FindAsync(id);

            if (masterSchedule == null)
            {
                return NotFound();
            }

            return masterSchedule;
        }

        // PUT: api/MasterSchedules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMasterSchedule(int id, MasterSchedule masterSchedule)
        {
            if (id != masterSchedule.Id)
            {
                return BadRequest();
            }

            _context.Entry(masterSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MasterScheduleExists(id))
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

        // POST: api/MasterSchedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MasterSchedule>> PostMasterSchedule(MasterSchedule masterSchedule)
        {
          if (_context.MasterSchedules == null)
          {
              return Problem("Entity set 'MyDatabaseContext.MasterSchedules'  is null.");
          }
            _context.MasterSchedules.Add(masterSchedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMasterSchedule", new { id = masterSchedule.Id }, masterSchedule);
        }

        // DELETE: api/MasterSchedules/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterSchedule(int id)
        {
            if (_context.MasterSchedules == null)
            {
                return NotFound();
            }
            var masterSchedule = await _context.MasterSchedules.FindAsync(id);
            if (masterSchedule == null)
            {
                return NotFound();
            }

            _context.MasterSchedules.Remove(masterSchedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        [HttpDelete("{masterId}")]
        public async Task<IActionResult> DeleteByCondition(int masterId) {
            try {
                // Получаем все записи из базы данных, удовлетворяющие заданному условию
                var recordsToDelete = await _context.MasterSchedules
                    .Where(x => x.MasterId == masterId)
                    .ToListAsync();

                // Если нет записей для удаления, возвращаем сообщение об ошибке
                if (!recordsToDelete.Any()) {
                    return NotFound(JsonSerializer.Serialize($"No records found with {masterId}."));
                }

                // Удаляем найденные записи из базы данных
                _context.MasterSchedules.RemoveRange(recordsToDelete);
                await _context.SaveChangesAsync();

                // Возвращаем сообщение об успешном удалении
                return Ok(JsonSerializer.Serialize($"Deleted {recordsToDelete.Count} records with {masterId}."));
            }
            catch (Exception ex) {
                // Обрабатываем ошибку и возвращаем сообщение об ошибке
                return StatusCode(StatusCodes.Status500InternalServerError, JsonSerializer.Serialize($"Error deleting records: {ex.Message}"));
            }
        }


        private bool MasterScheduleExists(int id)
        {
            return (_context.MasterSchedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
