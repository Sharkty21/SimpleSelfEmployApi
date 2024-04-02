using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SimpleSelfEmployApi.Dtos;
using SimpleSelfEmployApi.Models;
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
        public async Task<ActionResult<IEnumerable<JobDto>>> Index(int? pageNumber, int limit = 10)
        {
            return Ok(await _jobService.Index(pageNumber, limit));
        }

        [HttpPost]
        public async Task<ActionResult<JobDto>> Save(string? id, [FromBody] JobDto record)
        {
            if (!String.Equals(id ?? string.Empty, record.Id))
                return BadRequest("Id mismatch");

            return Ok(await _jobService.SaveJob(record));
        }
    }
}
