using ChatWithSignalR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatWithSignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        private ChatHistoryRepository _chatRepository;

        private readonly static ConnectionMapping<string> _connections =
           new ConnectionMapping<string>();

        public ChatHub(ChatHistoryRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var claims = Context.User.Claims;
            var userId = claims.FirstOrDefault(x => x.Type == "UserId");

            _connections.Add(userId.Value, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string message, string receiverUserId)
        {
            if (_connections.GetConnections(receiverUserId).Count() <= 0)
            {
                await Task.CompletedTask;
            }

            var targetUserConnectionId = _connections.GetConnections(receiverUserId).First();
            await Clients.Client(targetUserConnectionId).SendAsync("ChatChannel", message);

            await AddChatHistory(message, receiverUserId);
        }

        private async Task AddChatHistory(string message, string receiverUserId)
        {
            var claims = Context.User.Claims;
            var sernderUserId = claims.FirstOrDefault(x => x.Type == "UserId");

            await _chatRepository.AddAsync(new ChatHistory
            {
                SenderId = sernderUserId.Value,
                ReceiverId = receiverUserId,
                Message = message,
                MessageTime = DateTime.Now.Date
            });
        }
    }
}
