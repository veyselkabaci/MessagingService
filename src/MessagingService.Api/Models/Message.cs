using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MessagingService.Api.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string SenderUsername { get; set; } = null!;
    public string ReceiverUsername { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime Date { get; set; }
}