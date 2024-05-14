using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace mongodb_project.Models
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("date")]
        public DateTime Date { get; set; }
        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }
        [BsonElement("is_deleted")]
        public bool Isdeleted { get; set; }
    }
}
