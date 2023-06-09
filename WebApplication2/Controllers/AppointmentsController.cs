using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services.Cache;
using WebApplication2.ServicesList.Appointments.Controllers.Delete;
using WebApplication2.ServicesList.Appointments.Controllers.Get;
using WebApplication2.ServicesList.Appointments.Controllers.Post;
using WebApplication2.ServicesList.Appointments.Controllers.Put;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {

        private string cacheAllAppointmentsKey = "appointments_cache_key";

        private string cacheAllCostumersKey = "costumers_cache_key";

        private readonly CacheService _cacheService;

        private readonly GetAppListService _getAppListService;

        private readonly GetByAdminIdService _getByAdminIdService;

        private readonly GetListByMasterIdService _getByMasterIdService;

        private readonly PutAppService _putAppService;

        private readonly PostAppService _postAppService;

        private readonly DeleteAppService _deleteAppService;

        public AppointmentsController(
            CacheService cacheService,
            GetAppListService getAppListService,
            GetByAdminIdService getByAdminIdService,
            GetListByMasterIdService getListByMasterIdService,
            PutAppService putAppService,
            PostAppService postAppService,
            DeleteAppService deleteAppService)
        {

            _cacheService = cacheService;

            _getAppListService = getAppListService;

            _getByAdminIdService = getByAdminIdService;

            _getByMasterIdService = getListByMasterIdService;

            _putAppService = putAppService;

            _postAppService = postAppService;

            _deleteAppService = deleteAppService;
        }

        // GET: api/Appointments
        [HttpGet("allAppointments/{currentAdminId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments(int currentAdminId)
        {
            return await _getAppListService.InitList( _cacheService, cacheAllAppointmentsKey, currentAdminId);
        }

        [HttpGet("appointmentsByMasterID/{id}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByMaster(int id) {

            return await _getByMasterIdService.InitGetList( id);
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            return await _getByAdminIdService.Init( id);
        }


        // PUT: api/Appointments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment, int adminId)
        {
            return await _putAppService.Init( _cacheService, cacheAllAppointmentsKey, id, adminId, cacheAllCostumersKey,appointment);
        }

        // POST: api/Appointments
        [HttpPost("{id}")]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment, int id)
        {
            var obj = _postAppService.Init(appointment,_cacheService, cacheAllAppointmentsKey, id, cacheAllCostumersKey);
            if(obj == null) {
                return Problem("Entity set 'MyDatabaseContext.Admins'  is null.");
            }

            return CreatedAtAction("GetAppointment", new { id = obj.Id }, obj);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id, int adminId)
        {
            return await _deleteAppService.Init(id,_cacheService,cacheAllAppointmentsKey, adminId, cacheAllCostumersKey);
        }

    }
}
