using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoDotNet.Models
{
    public class Todo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
