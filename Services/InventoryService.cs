using System.Collections.Generic;
using System.Threading.Tasks;
using Itransition_Course_Project.Data;
using Itransition_Course_Project.Models;
using Itransition_Course_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Inventory>> GetAllAsync()
        {
            return await _context.Inventories
                .Include(i => i.Creator)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Inventory?> GetByIdAsync(int id)
        {
            return await _context.Inventories
                .Include(i => i.Creator)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory?> GetByIdWithItemsAsync(int id)
        {
            return await _context.Inventories
                .Include(i => i.Creator)
                .Include(i => i.Items)
                    .ThenInclude(item => item.CreatedBy)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory> CreateAsync(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task<Inventory?> UpdateAsync(Inventory inventory, byte[] expectedVersion)
        {
            var existing = await _context.Inventories.FindAsync(inventory.Id);
            if (existing == null) return null;

            _context.Entry(existing).Property(e => e.RowVersion).OriginalValue = expectedVersion;

            existing.Title = inventory.Title;
            existing.Description = inventory.Description;
            existing.Category = inventory.Category;
            existing.ImageUrl = inventory.ImageUrl;
            existing.IsPublic = inventory.IsPublic;
            existing.CustomIdFormat = inventory.CustomIdFormat;

            existing.CustomString1_State = inventory.CustomString1_State;
            existing.CustomString1_Name = inventory.CustomString1_Name;
            existing.CustomString2_State = inventory.CustomString2_State;
            existing.CustomString2_Name = inventory.CustomString2_Name;
            existing.CustomString3_State = inventory.CustomString3_State;
            existing.CustomString3_Name = inventory.CustomString3_Name;

            existing.CustomInt1_State = inventory.CustomInt1_State;
            existing.CustomInt1_Name = inventory.CustomInt1_Name;
            existing.CustomInt2_State = inventory.CustomInt2_State;
            existing.CustomInt2_Name = inventory.CustomInt2_Name;
            existing.CustomInt3_State = inventory.CustomInt3_State;
            existing.CustomInt3_Name = inventory.CustomInt3_Name;

            existing.CustomBool1_State = inventory.CustomBool1_State;
            existing.CustomBool1_Name = inventory.CustomBool1_Name;
            existing.CustomBool2_State = inventory.CustomBool2_State;
            existing.CustomBool2_Name = inventory.CustomBool2_Name;
            existing.CustomBool3_State = inventory.CustomBool3_State;
            existing.CustomBool3_Name = inventory.CustomBool3_Name;

            existing.CustomText1_State = inventory.CustomText1_State;
            existing.CustomText1_Name = inventory.CustomText1_Name;
            existing.CustomText2_State = inventory.CustomText2_State;
            existing.CustomText2_Name = inventory.CustomText2_Name;
            existing.CustomText3_State = inventory.CustomText3_State;
            existing.CustomText3_Name = inventory.CustomText3_Name;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null) return false;

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
