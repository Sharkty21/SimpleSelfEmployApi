using SimpleSelfEmploy.Data;
using SimpleSelfEmploy.Models.Mongo;
using SimpleSelfEmployApi.Models;

namespace SimpleSelfEmployApi.Data
{
    public class PaymentRepository : MongoDbRepository<Payment>
    {
        public PaymentRepository(IMongoDbSettings settings, IHttpContextAccessor httpContextAccessor, ILogger logger) : base(settings, httpContextAccessor, logger)
        {
            SetCollection("Payments");
        }

        public async Task<IQueryable<Payment>> Index(int? page, string? jobId, int limit = 10)
        {
            int pageNum = (page ?? 1) - 1;
            var query = AsQueryable();

            if (!string.IsNullOrEmpty(jobId))
            {
                query = query.Where(p => p.JobId == jobId);
            }

            return query.OrderBy(j => j.ModifiedDate).Skip(10 * pageNum).Take(limit);
        }
    }
}
