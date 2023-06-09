using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.ServicesList.Statistics.Controllers.Delete;
using WebApplication2.ServicesList.Statistics.Controllers.Get;
using WebApplication2.ServicesList.Statistics.Controllers.Post;
using WebApplication2.ServicesList.Statistics.Controllers.Put;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {

        private readonly GetStatListService _statListService;

        private readonly GetStatByIdService _statByIdService;

        private readonly PostStatService _postStatService;

        private readonly DeleteStatService _deleteStatService;

        private readonly PutStatService _putStatService;

        public StatisticsController(
            GetStatListService getStatListService,
            GetStatByIdService statByIdService,
            PostStatService postStatService,
            DeleteStatService deleteStatService,
            PutStatService putStatService) {

            _statListService = getStatListService;
            _statByIdService = statByIdService;
            _postStatService = postStatService;
            _deleteStatService = deleteStatService;
            _putStatService = putStatService;
        }

        // GET: api/Statistics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Statistic>>> GetStatistics()
        {
            return await _statListService.Init();
        }

        // GET: api/Statistics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Statistic>> GetStatistic(int id)
        {
            return await _statByIdService.Init(id);
        }

        // PUT: api/Statistics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatistic(int id, Statistic statistic)
        {
            return await _putStatService.Init(id, statistic);
        }

        // POST: api/Statistics
        [HttpPost]
        public async Task<ActionResult<Statistic>> PostStatistic(Statistic statistic)
        {

            var obj = _postStatService.Init(statistic);
            if (obj == null) {
                return Problem("Entity set 'MyDatabaseContext.Admins'  is null.");
            }

            return CreatedAtAction("GetStatistic", new { id = statistic.Id }, statistic);
        }

        // DELETE: api/Statistics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatistic(int id)
        {

            return await _deleteStatService.Init(id);
        }

    }
}
