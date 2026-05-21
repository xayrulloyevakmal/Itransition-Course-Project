using System.Collections.Generic;
using System.Threading.Tasks;
using Itransition_Course_Project.Models;

namespace Itransition_Course_Project.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetByInventoryIdAsync(int inventoryId);
        Task<Comment> CreateAsync(Comment comment);
    }
}
