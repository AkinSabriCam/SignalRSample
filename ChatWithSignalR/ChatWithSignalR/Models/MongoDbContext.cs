using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatWithSignalR.Models
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _db;

        public MongoDbContext(IMongoClient mongoClient,string databaseName)
        {
            _db = mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<ChatHistory> ChatHistories => _db.GetCollection<ChatHistory>("ChatHistories");
    }
}
