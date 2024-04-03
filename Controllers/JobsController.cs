using Microsoft.AspNetCore.Mvc;
using SimpleSelfEmployApi.Dtos;
using SimpleSelfEmployApi.Services;

namespace SimpleSelfEmployApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly ILogger<JobsController> _logger;
        private readonly IJobService _jobService;

        public JobsController(ILogger<JobsController> logger, IJobService jobService)
        {
            _logger = logger;
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDto>>> Index(int? page, int limit = 10)
        {
            return Ok(await _jobService.Index(page, limit));
        }

        [HttpPost]
        public async Task<ActionResult<JobDto>> CreateNew(string? id, [FromBody] JobDto record)
        {
            return Ok(await Save(id, record));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JobDto>> Update(string? id, [FromBody] JobDto record)
        {
            return Ok(await Save(id, record));
        }

        private async Task<ActionResult<JobDto>> Save(string? id, [FromBody] JobDto record)
        {
            if (!String.Equals(id ?? string.Empty, record.Id))
                return BadRequest("Id mismatch");

            return Ok(await _jobService.SaveJob(record));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobDto>> Detail(string id)
        {
            var job = await _jobService.GetJob(id);

            if (job == null)
                return NotFound();

            return Ok(await _jobService.GetJob(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<JobDto>> Delete(string id)
        {
            var job = await _jobService.GetJob(id);

            if (job == null)
                return NotFound();

            bool success = await _jobService.DeleteJob(id);

            return Ok($"Delete {id} successful.");
        }
    }
}
