using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatWithSignalR.Models
{
    public class ChatHistoryRepository
    {
        private readonly MongoDbContext _mongoDbContext;

        public ChatHistoryRepository(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }


        public async Task AddAsync(ChatHistory chatHistory)
        {
            await _mongoDbContext.ChatHistories.InsertOneAsync(chatHistory);
        }

        public async Task<IList<ChatHistory>> GetList()
        {
            var chatHistories = _mongoDbContext.ChatHistories.AsQueryable();

            return await chatHistories.ToListAsync();
        }

        public async Task<ChatHistory> GetById(string Id)
        {
            return await _mongoDbContext.ChatHistories
                        .Find(x => x.Id.ToString() == Id).FirstOrDefaultAsync();
        }
    }
}
