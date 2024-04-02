using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SimpleSelfEmploy.Models.Mongo
{
    public abstract class MongoDbDocument : IMongoDbDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }

        public DateTime? EntryDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
