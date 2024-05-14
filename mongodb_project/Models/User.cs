using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongodb_project.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("date")]
        public DateTime Date { get; set; }
        [BsonElement("is_deleted")]
        public bool Isdeleted { get; set; }
    }
}
