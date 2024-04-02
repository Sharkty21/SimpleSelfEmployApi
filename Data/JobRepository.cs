using SimpleSelfEmploy.Data;
using SimpleSelfEmploy.Models.Mongo;
using SimpleSelfEmployApi.Models;

namespace SimpleSelfEmployApi.Data
{
    public class JobRepository : MongoDbRepository<Job>
    {
        public JobRepository(IMongoDbSettings settings) : base(settings)
        {
            SetCollection("Jobs");
        }

        public async Task<IQueryable<Job>> Index(int? pageNumber, int limit = 10)
        {
            int page = (pageNumber ?? 1) - 1;
            return AsQueryable().OrderBy(j => j.ModifiedDate).Skip(10 * page).Take(limit);
        }
    }
}
