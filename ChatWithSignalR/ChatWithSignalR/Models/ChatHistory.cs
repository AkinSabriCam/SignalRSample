using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ChatWithSignalR.Models
{
    public class ChatHistory
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public string Message { get; set; }

        public DateTime MessageTime { get; set; }

        public ChatHistory()
        {
            Id = ObjectId.GenerateNewId();
        }
    }
}
