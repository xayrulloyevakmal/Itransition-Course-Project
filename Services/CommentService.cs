using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itransition_Course_Project.Data;
using Itransition_Course_Project.Models;
using Itransition_Course_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetByInventoryIdAsync(int inventoryId)
        {
            return await _context.Comments
                .Include(c => c.Author)
                .Where(c => c.InventoryId == inventoryId)
                .OrderByDescending(c => c.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}
