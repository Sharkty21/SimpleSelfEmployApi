using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleSelfEmploy.Models.Mongo
{
    public interface IMongoDbDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }

        public DateTime? EntryDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
