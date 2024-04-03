using Microsoft.AspNetCore.Mvc;
using SimpleSelfEmployApi.Dtos;
using SimpleSelfEmployApi.Services;

namespace SimpleSelfEmployApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentService _PaymentService;

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentService PaymentService)
        {
            _logger = logger;
            _PaymentService = PaymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> Index(int? page, string? jobId, int limit = 10)
        {
            return Ok(await _PaymentService.Index(page, jobId, limit));
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreateNew(string? id, [FromBody] PaymentDto record)
        {
            return Ok(await Save(id, record));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentDto>> Update(string? id, [FromBody] PaymentDto record)
        {
            return Ok(await Save(id, record));
        }

        private async Task<ActionResult<PaymentDto>> Save(string? id, [FromBody] PaymentDto record)
        {
            if (!String.Equals(id ?? string.Empty, record.Id))
                return BadRequest("Id mismatch");

            return Ok(await _PaymentService.SavePayment(record));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> Detail(string id)
        {
            var Payment = await _PaymentService.GetPayment(id);

            if (Payment == null)
                return NotFound();

            return Ok(await _PaymentService.GetPayment(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentDto>> Delete(string id)
        {
            var Payment = await _PaymentService.GetPayment(id);

            if (Payment == null)
                return NotFound();

            bool success = await _PaymentService.DeletePayment(id);

            return Ok($"Delete {id} successful.");
        }
    }
}
