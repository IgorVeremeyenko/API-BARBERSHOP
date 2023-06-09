using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services.Masters;

namespace WebApplication2.ServicesList.Masters.Controllers.Get {
    public class GetMasterSpecialListService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<List<MastersService>> Init(int id) {

            var masters = await _context.Masters.Where(t => t.UserId == id).ToListAsync();
            var services = await _context.Services.Where(t => t.UserId == id).ToListAsync();
            var masterSchedules = await _context.MasterSchedules.ToListAsync();
            GenerateMasterList generateMasterList = new GenerateMasterList();
            var result = generateMasterList.MasterList(masters, services, masterSchedules);
            return result;
        }
    }
}
