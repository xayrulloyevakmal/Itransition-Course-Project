using System.Collections.Generic;
using System.Threading.Tasks;
using Itransition_Course_Project.Models;

namespace Itransition_Course_Project.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<Item>> GetByInventoryIdAsync(int inventoryId);
        Task<Item?> GetByIdAsync(int id);
        Task<Item> CreateAsync(Item item);
        Task<Item?> UpdateAsync(Item item, byte[] expectedVersion);
        Task<bool> DeleteAsync(int id);
    }
}
