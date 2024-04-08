using MongoDB.Bson;
using MongoDB.Driver;
using SimpleSelfEmploy.Models.Mongo;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;

namespace SimpleSelfEmploy.Data
{
    public class MongoDbRepository<TMongoDbDocument> : IMongoDbRepository<TMongoDbDocument>
        where TMongoDbDocument : IMongoDbDocument
    {
        private IMongoCollection<TMongoDbDocument> _collection;
        private readonly IMongoDatabase _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public MongoDbRepository(IMongoDbSettings settings, IHttpContextAccessor httpContextAccessor)
        {
            Trace.TraceError($"The connection string starts with {settings?.ConnectionString?.Substring(0, 5)}");
            _database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCollection(string collectionName)
        {
            // Retrieve the JWT token from the Authorization header
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Decode the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Retrieve the user ID from the 'sub' claim
            var userId = jwtToken?.Subject;

            if (string.IsNullOrEmpty(userId))
                throw new Exception();

            _collection = _database.GetCollection<TMongoDbDocument>(userId + " - " + collectionName);
        }

        public void OnUpdate(TMongoDbDocument document, bool isNew)
        {
            if (isNew)
                document.EntryDate = DateTime.UtcNow;
            else
                document.EntryDate = FindById(document.Id.ToString()).EntryDate;

            document.ModifiedDate = DateTime.UtcNow;
        }

        public void OnUpdate(IEnumerable<TMongoDbDocument> documents, bool isNew)
        {
            foreach (var document in documents)
            {
                OnUpdate(document, isNew);
            }
        }

        public virtual IQueryable<TMongoDbDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TMongoDbDocument> FilterBy(
            Expression<Func<TMongoDbDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TMongoDbDocument, bool>> filterExpression,
            Expression<Func<TMongoDbDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TMongoDbDocument FindOne(Expression<Func<TMongoDbDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TMongoDbDocument> FindOneAsync(Expression<Func<TMongoDbDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TMongoDbDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TMongoDbDocument>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TMongoDbDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TMongoDbDocument>.Filter.Eq(doc => doc.Id, objectId);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual void InsertOne(TMongoDbDocument document)
        {
            OnUpdate(document, true);
            _collection.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TMongoDbDocument document)
        {
            OnUpdate(document, true);
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TMongoDbDocument> documents)
        {
            OnUpdate(documents, true);
            _collection.InsertMany(documents);
        }

        public virtual async Task InsertManyAsync(ICollection<TMongoDbDocument> documents)
        {
            OnUpdate(documents, true);
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TMongoDbDocument document)
        {
            OnUpdate(document, false);
            var filter = Builders<TMongoDbDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TMongoDbDocument document)
        {
            OnUpdate(document, false);
            var filter = Builders<TMongoDbDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TMongoDbDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TMongoDbDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TMongoDbDocument>.Filter.Eq(doc => doc.Id, objectId);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TMongoDbDocument>.Filter.Eq(doc => doc.Id, objectId);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TMongoDbDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TMongoDbDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }
    }
}
