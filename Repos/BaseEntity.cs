using MongoDB.Bson.Serialization.Attributes;

namespace ReservationApi.Repos
{
    public abstract class BaseEntity
    {
        [BsonId]
        public string Id { get; set; }
    }
}