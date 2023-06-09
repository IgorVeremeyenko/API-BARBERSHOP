using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services.Masters;
using WebApplication2.ServicesList.Masters.Controllers.Delete;
using WebApplication2.ServicesList.Masters.Controllers.Get;
using WebApplication2.ServicesList.Masters.Controllers.Post;
using WebApplication2.ServicesList.Masters.Controllers.Put;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase {

        private readonly GetMasterListByIdService _getMasterListByIdService;

        private readonly GetMasterSpecialListService _getMasterSpecialListService;

        private readonly GetMasterListService _getMasterListService;

        private readonly GetMasterByNameService _getMasterByNameService;

        private readonly GetMasterByIdService _getMasterByIdService;

        private readonly PostMasterService _postMasterService;

        private readonly PutMasterService _putMasterService;
        
        private readonly DeleteMasterService _deleteMasterService;

        public MasterController(
            GetMasterListByIdService getMasterListByIdService,
            GetMasterListService getMasterListService,
            GetMasterByNameService getMasterByNameService,
            GetMasterByIdService getMasterByIdService,
            PostMasterService postMasterService,
            PutMasterService putMasterService,
            DeleteMasterService deleteMasterService,
            GetMasterSpecialListService getMasterSpecialListService) {
            _getMasterListByIdService = getMasterListByIdService;
            _getMasterListService = getMasterListService;
            _getMasterByNameService = getMasterByNameService;
            _getMasterByIdService = getMasterByIdService;
            _postMasterService = postMasterService;
            _putMasterService = putMasterService;
            _deleteMasterService = deleteMasterService;
            _getMasterSpecialListService = getMasterSpecialListService;
        }
        // GET: api/<ValuesController>
        [HttpGet("allMasters/{userId}")]
        public async Task<ActionResult<IEnumerable<Master>>> GetMasters(int userId) {

            return await _getMasterListService.Init(userId);
        }

        [HttpGet("masterList/{id}")]
        public async Task<List<MastersService>> GetMastersList(int id) {

            return await _getMasterSpecialListService.Init(id);
        }

        [HttpGet("masterID/{id}")]
        public int GetMastersCount(int id) {

           return _getMasterListByIdService.Init( id);
        }

        [HttpGet("masterName/{name, adminId}")]
        public async Task<ActionResult<Master>> GetMasterByName(string name, int adminId) {

            return await _getMasterByNameService.Init( name, adminId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Master>> GetMaster(int id) {

            return await _getMasterByIdService.Init( id);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Master>> PostMaster(Master master) {

            var obj = _postMasterService.Init( master);
            if(obj == null) {
                return Problem("Entity set 'MyDatabaseContext.Admins'  is null.");
            }

            return CreatedAtAction("GetMaster", new { id = obj.Id }, obj);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaster(int id, Master master) {

            return await _putMasterService.Init( id, master);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaster(int id) {

            return await _deleteMasterService.Init( id);
        }

    }
}
