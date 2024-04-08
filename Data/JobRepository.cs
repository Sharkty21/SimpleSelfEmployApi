using SimpleSelfEmploy.Data;
using SimpleSelfEmploy.Models.Mongo;
using SimpleSelfEmployApi.Models;

namespace SimpleSelfEmployApi.Data
{
    public class JobRepository : MongoDbRepository<Job>
    {
        public JobRepository(IMongoDbSettings settings, IHttpContextAccessor httpContextAccessor, ILogger logger) : base(settings, httpContextAccessor, logger)
        {
            SetCollection("Jobs");
        }

        public async Task<IQueryable<Job>> Index(int? page, int limit = 10)
        {
            int pageNum = (page ?? 1) - 1;
            return AsQueryable().OrderBy(j => j.ModifiedDate).Skip(10 * pageNum).Take(limit);
        }
    }
}
