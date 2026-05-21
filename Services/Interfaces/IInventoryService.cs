using System.Collections.Generic;
using System.Threading.Tasks;
using Itransition_Course_Project.Models;

namespace Itransition_Course_Project.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<List<Inventory>> GetAllAsync();
        Task<Inventory?> GetByIdAsync(int id);
        Task<Inventory?> GetByIdWithItemsAsync(int id);
        Task<Inventory> CreateAsync(Inventory inventory);
        Task<Inventory?> UpdateAsync(Inventory inventory, byte[] expectedVersion);
        Task<bool> DeleteAsync(int id);
    }
}
