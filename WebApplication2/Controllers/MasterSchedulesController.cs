using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication2.Models;
using WebApplication2.ServicesList.MasterSchedules.Controllers.Delete;
using WebApplication2.ServicesList.MasterSchedules.Controllers.Get;
using WebApplication2.ServicesList.MasterSchedules.Controllers.Post;
using WebApplication2.ServicesList.MasterSchedules.Controllers.Put;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterSchedulesController : ControllerBase
    {

        private readonly GetShedulesByIdService _getShedulesByIdService;

        private readonly GetShedulesListService _getShedulesListService;

        private readonly PutShedulesService _putShedulesService;

        private readonly PostShedulesService _postShedulesService;

        private readonly DeleteShedulesService _deleteShedulesService;

        public MasterSchedulesController(
            GetShedulesByIdService getShedulesByIdService,
            GetShedulesListService getShedulesListService,
            PutShedulesService putShedulesService,
            PostShedulesService postShedulesService,
            DeleteShedulesService deleteShedulesService) {
            _getShedulesByIdService = getShedulesByIdService;
            _getShedulesListService = getShedulesListService;
            _putShedulesService = putShedulesService;
            _postShedulesService = postShedulesService;
            _deleteShedulesService = deleteShedulesService;
        }

        // GET: api/MasterSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MasterSchedule>>> GetMasterSchedules()
        {
            return await _getShedulesListService.Init();
        }

        // GET: api/MasterSchedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MasterSchedule>> GetMasterSchedule(int id)
        {
            return await _getShedulesByIdService.Init( id);
        }

        // PUT: api/MasterSchedules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMasterSchedule(int id, MasterSchedule masterSchedule)
        {
            return await _putShedulesService.Init(id, masterSchedule);
        }

        // POST: api/MasterSchedules
        [HttpPost]
        public async Task<ActionResult<MasterSchedule>> PostMasterSchedule(MasterSchedule masterSchedule)
        {
            var obj = _postShedulesService.Init( masterSchedule);
            if(obj == null) {
                return Problem("Entity set 'MyDatabaseContext.Admins'  is null.");
            }

            return CreatedAtAction("GetMasterSchedule", new { id = masterSchedule.Id }, masterSchedule);
        }

        [HttpDelete("{masterId}")]
        public async Task<IActionResult> DeleteByCondition(int masterId) {

            return await _deleteShedulesService.Init( masterId);
        }

    }
}
