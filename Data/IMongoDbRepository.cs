using SimpleSelfEmploy.Models.Mongo;
using System.Linq.Expressions;

namespace SimpleSelfEmploy.Data
{
    public interface IMongoDbRepository<TMongoDbDocument> where TMongoDbDocument : IMongoDbDocument
    {
        IQueryable<TMongoDbDocument> AsQueryable();

        IEnumerable<TMongoDbDocument> FilterBy(
            Expression<Func<TMongoDbDocument, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TMongoDbDocument, bool>> filterExpression,
            Expression<Func<TMongoDbDocument, TProjected>> projectionExpression);

        TMongoDbDocument FindOne(Expression<Func<TMongoDbDocument, bool>> filterExpression);

        Task<TMongoDbDocument> FindOneAsync(Expression<Func<TMongoDbDocument, bool>> filterExpression);

        TMongoDbDocument FindById(string id);

        Task<TMongoDbDocument> FindByIdAsync(string id);

        void InsertOne(TMongoDbDocument document);

        Task InsertOneAsync(TMongoDbDocument document);

        void InsertMany(ICollection<TMongoDbDocument> documents);

        Task InsertManyAsync(ICollection<TMongoDbDocument> documents);

        void ReplaceOne(TMongoDbDocument document);

        Task ReplaceOneAsync(TMongoDbDocument document);

        void DeleteOne(Expression<Func<TMongoDbDocument, bool>> filterExpression);

        Task DeleteOneAsync(Expression<Func<TMongoDbDocument, bool>> filterExpression);

        void DeleteById(string id);

        Task DeleteByIdAsync(string id);

        void DeleteMany(Expression<Func<TMongoDbDocument, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<TMongoDbDocument, bool>> filterExpression);
    }
}
