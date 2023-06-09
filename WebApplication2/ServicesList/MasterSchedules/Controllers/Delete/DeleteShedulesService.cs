using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication2.Models;

namespace WebApplication2.ServicesList.MasterSchedules.Controllers.Delete {
    public class DeleteShedulesService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(int masterId) {
            try {

                var recordsToDelete = await _context.MasterSchedules
                    .Where(x => x.MasterId == masterId)
                    .ToListAsync();


                if (!recordsToDelete.Any()) {
                    return new NotFoundObjectResult(JsonSerializer.Serialize($"No records found with {masterId}."));
                }


                _context.MasterSchedules.RemoveRange(recordsToDelete);
                await _context.SaveChangesAsync();


                return new OkObjectResult(JsonSerializer.Serialize($"Deleted {recordsToDelete.Count} records with {masterId}."));
            }
            catch (Exception ex) {

                return new StatusCodeResult(500);
            }
        }
    }
}
