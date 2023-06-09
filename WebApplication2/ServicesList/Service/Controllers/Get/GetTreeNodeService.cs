using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.TreeNodes;
using WebApplication2.Services;

namespace WebApplication2.ServicesList.Service.Controllers.Get {
    public class GetTreeNodeService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<ActionResult<IEnumerable<TreeNode>>> Init(int id) {

            if (_context.Services == null) {
                return new NotFoundResult();
            }
            var services = await _context.Services.Where(t => t.UserId == id).ToListAsync();
            GenerateTreeNodeListServices generateTreeNodeListServices = new GenerateTreeNodeListServices();
            var result = generateTreeNodeListServices.GetTreeNode(_context, services);
            return result;
        }
    }
}
