using ChatApi.Context;
using ChatApi.Dtos;
using ChatApi.Hubs;
using ChatApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class ChatController(ApplicationDbContext context, IHubContext<ChatHub> hubContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetChat(Guid userId, Guid toUserId, CancellationToken cancellation)
        {
            List<Chat> chats = await context.Chats.Where(p => p.UserId == userId && p.ToUserId == toUserId || p.ToUserId == userId && p.UserId == toUserId).OrderBy(p => p.Date).ToListAsync(cancellation);

            return Ok(chats);
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(SendeMessageDto request, CancellationToken cancellationToken)
        {
            Chat chat = new()
            {
                UserId = request.UserId,
                ToUserId = request.ToUserId,
                Message = request.mesage,
                Date = DateTime.Now
            };
            await context.AddAsync(chat, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            string connectionId = ChatHub.Users.First(p => p.Value == chat.ToUserId).Key;
            await hubContext.Clients.Client(connectionId).SendAsync("Messages", chat);

            return Ok();
        }
    }
}
