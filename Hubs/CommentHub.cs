using System;
using System.Threading.Tasks;
using Itransition_Course_Project.Models;
using Itransition_Course_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Itransition_Course_Project.Hubs
{
    [Authorize]
    public class CommentHub : Hub
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentHub(ICommentService commentService, UserManager<ApplicationUser> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task JoinInventoryGroup(int inventoryId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"inventory-{inventoryId}");
        }

        public async Task LeaveInventoryGroup(int inventoryId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"inventory-{inventoryId}");
        }

        public async Task SendComment(int inventoryId, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            var user = await _userManager.GetUserAsync(Context.User!);
            if (user == null) return;

            var comment = new Comment
            {
                InventoryId = inventoryId,
                AuthorId = user.Id,
                Text = text.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _commentService.CreateAsync(comment);

            await Clients.Group($"inventory-{inventoryId}").SendAsync("ReceiveComment", new
            {
                authorName = user.UserName,
                text = comment.Text,
                createdAt = comment.CreatedAt.ToString("yyyy-MM-dd HH:mm")
            });
        }
    }
}
