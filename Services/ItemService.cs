using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itransition_Course_Project.Data;
using Itransition_Course_Project.Models;
using Itransition_Course_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Services
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _context;

        public ItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetByInventoryIdAsync(int inventoryId)
        {
            return await _context.Items
                .Include(i => i.CreatedBy)
                .Where(i => i.InventoryId == inventoryId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items
                .Include(i => i.Inventory)
                .Include(i => i.CreatedBy)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Item> CreateAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item?> UpdateAsync(Item item, byte[] expectedVersion)
        {
            var existing = await _context.Items.FindAsync(item.Id);
            if (existing == null) return null;

            _context.Entry(existing).Property(e => e.RowVersion).OriginalValue = expectedVersion;

            existing.CustomString1_Value = item.CustomString1_Value;
            existing.CustomString2_Value = item.CustomString2_Value;
            existing.CustomString3_Value = item.CustomString3_Value;
            existing.CustomInt1_Value = item.CustomInt1_Value;
            existing.CustomInt2_Value = item.CustomInt2_Value;
            existing.CustomInt3_Value = item.CustomInt3_Value;
            existing.CustomBool1_Value = item.CustomBool1_Value;
            existing.CustomBool2_Value = item.CustomBool2_Value;
            existing.CustomBool3_Value = item.CustomBool3_Value;
            existing.CustomText1_Value = item.CustomText1_Value;
            existing.CustomText2_Value = item.CustomText2_Value;
            existing.CustomText3_Value = item.CustomText3_Value;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return false;

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
