using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.TreeNodes;
using WebApplication2.Services;
using WebApplication2.ServicesList;
using WebApplication2.ServicesList.Service;
using WebApplication2.ServicesList.Service.Controllers.Delete;
using WebApplication2.ServicesList.Service.Controllers.Get;
using WebApplication2.ServicesList.Service.Controllers.Post;
using WebApplication2.ServicesList.Service.Controllers.Put;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase {

        private readonly GetAppFilterService _getAppFilterService;

        private readonly GetGroupListService _getGroupListService;

        private readonly GetListByNameService _getListByNameService;

        private readonly GetListForMasterEditService _getListForMasterEditService;

        private readonly GetServByIdService _getServByIdService;

        private readonly GetServListService _getServListService;

        private readonly GetTreeNodeService _getTreeNodeService;

        private readonly PostServService _postServService;

        private readonly PutServService _putServService;

        private readonly DeleteServService _deleteServService;

        public ServicesController(
            GetAppFilterService getAppFilterService,
            GetGroupListService getGroupListService,
            GetListByNameService getListByNameService,
            GetListForMasterEditService getListForMasterEditService,
            GetServByIdService getServByIdService,
            GetServListService getServListService,
            GetTreeNodeService getTreeNodeService,
            PostServService postServService,
            PutServService putServService,
            DeleteServService deleteServService) {

            _getAppFilterService = getAppFilterService;
            _getGroupListService = getGroupListService;
            _getListByNameService = getListByNameService;
            _getListForMasterEditService = getListForMasterEditService;
            _getServByIdService = getServByIdService;
            _getServListService = getServListService;
            _getTreeNodeService = getTreeNodeService;
            _postServService = postServService;
            _putServService = putServService;
            _deleteServService = deleteServService;
        }

        // GET: api/Services
        [HttpGet("allServices/{id}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices(int id) {

            return await _getServListService.Init(id);
        }
        [HttpGet("TreeNode/{id}")]
        public async Task<ActionResult<IEnumerable<TreeNode>>> GetTreeNodeServices(int id) {
            
            return await _getTreeNodeService.Init(id);
        }

        [HttpGet("appointmentFilter/{id}")]
        public async Task<ActionResult<IEnumerable<Categorized>>> GetServicesForAppointment(int id) {

            return await _getAppFilterService.Init(id);
        }

        [HttpGet("listGroupedByCategory/{id}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesListCategory(int id) {
            
            return await _getGroupListService.Init(id);
        }

        [HttpGet("listGroupedByName/{id}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesListName(int id) {
           
            return await _getListByNameService.Init(id);
        }

        [HttpGet("loadServicesForEditComponent/{category,id}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesForEditing(string category, int id) {
            
            return await _getListForMasterEditService.Init(category, id);
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            return await _getServByIdService.Init(id);
        }

        // PUT: api/Services/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            return await _putServService.Init(id, service);
        }

        // POST: api/Services
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            return await _postServService.Init(service);            
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            return await _deleteServService.Init(id);
        }

    }
}
