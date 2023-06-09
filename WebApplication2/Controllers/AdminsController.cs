using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services.Cache;
using WebApplication2.Services;
using WebApplication2.ServicesList.Admins.Controllers.Get;
using WebApplication2.ServicesList.Admins.Controllers.Put;
using WebApplication2.ServicesList.Admins.Controllers.Post;
using WebApplication2.ServicesList.Admins.Controllers.Delete;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {

        private readonly GeneratingSaltForPasswordHashing _generationSalt;

        private readonly HashingPassword _hash;

        private readonly GetByIdService _getByAdminIdService;

        private readonly PutByIdService _putByIdService;

        private readonly PostAdminService _postAdminService;

        private readonly DeleteAdminService _deleteAdminService;

        private readonly GetListService _listService;

        public AdminsController(
            GeneratingSaltForPasswordHashing generatingSaltForPasswordHashing,
            HashingPassword hashingPassword,
            GetByIdService getByAdminIdService,
            PutByIdService putByIdService,
            PostAdminService postAdminService,
            DeleteAdminService deleteAdminService,
            GetListService getListService)
        {
            _hash = hashingPassword;
            _generationSalt = generatingSaltForPasswordHashing;
            _putByIdService = putByIdService;
            _getByAdminIdService = getByAdminIdService;
            _postAdminService = postAdminService;
            _deleteAdminService = deleteAdminService;
            _listService = getListService;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            return await _listService.InitList();
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            return await _getByAdminIdService.initId( id);
        }

        // PUT: api/Admins/5
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(int id, Admin admin)
        {
            return await _putByIdService.Init(_hash,  id, admin, _generationSalt);
        }

        // POST: api/Admins
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {

            var obj = _postAdminService.InitPost(admin);
            if(obj != null) {
                return Problem("Entity set 'MyDatabaseContext.Admins'  is null.");
            }

            return CreatedAtAction("GetAdmin", new { id = admin.Id }, admin);
        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            return await _deleteAdminService.InitDel( id);
        }

    }
}
