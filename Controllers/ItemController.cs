using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomInventoryApp.Data;
using CustomInventoryApp.Models;
using Itransition_Course_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Controllers
{
    [Authorize]
    public class ItemController(ApplicationDbContext context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Create(int inventoryId)
        {
            var inventory = await context.Inventories.FindAsync(inventoryId);
            if (inventory == null) return NotFound();

            var viewModel = new ItemFormViewModel
            {
                InventoryId = inventoryId,
                InventoryTitle = inventory.Title,
                
                CustomString1_State = inventory.CustomString1_State, CustomString1_Name = inventory.CustomString1_Name,
                CustomString2_State = inventory.CustomString2_State, CustomString2_Name = inventory.CustomString2_Name,
                CustomString3_State = inventory.CustomString3_State, CustomString3_Name = inventory.CustomString3_Name,
                
                CustomInt1_State = inventory.CustomInt1_State, CustomInt1_Name = inventory.CustomInt1_Name,
                CustomInt2_State = inventory.CustomInt2_State, CustomInt2_Name = inventory.CustomInt2_Name,
                CustomInt3_State = inventory.CustomInt3_State, CustomInt3_Name = inventory.CustomInt3_Name,
                
                CustomBool1_State = inventory.CustomBool1_State, CustomBool1_Name = inventory.CustomBool1_Name,
                CustomBool2_State = inventory.CustomBool2_State, CustomBool2_Name = inventory.CustomBool2_Name,
                CustomBool3_State = inventory.CustomBool3_State, CustomBool3_Name = inventory.CustomBool3_Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var inventory = await context.Inventories.FindAsync(model.InventoryId);
            if (inventory == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return Challenge();
            
            int nextIndexNumber = await context.Items.CountAsync(i => i.InventoryId == model.InventoryId) + 1;
            
            string generatedId = string.Format(inventory.CustomIdFormat, nextIndexNumber);

            var item = new Item
            {
                InventoryId = model.InventoryId,
                CreatedById = currentUserId,
                CreatedAt = DateTime.UtcNow,
                CustomId = generatedId,
                
                CustomString1_Value = model.CustomString1_Value ?? string.Empty,
                CustomString2_Value = model.CustomString2_Value ?? string.Empty,
                CustomString3_Value = model.CustomString3_Value ?? string.Empty,
                
                CustomInt1_Value = model.CustomInt1_Value,
                CustomInt2_Value = model.CustomInt2_Value,
                CustomInt3_Value = model.CustomInt3_Value,

                CustomBool1_Value = model.CustomBool1_Value,
                CustomBool2_Value = model.CustomBool2_Value,
                CustomBool3_Value = model.CustomBool3_Value,
                
                CustomText1_Value = string.Empty,
                CustomText2_Value = string.Empty,
                CustomText3_Value = string.Empty
            };

            try
            {
                context.Items.Add(item);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "A tracking asset with this exact ID sequence collision already exists inside this collection cluster.");
                return View(model);
            }

            return RedirectToAction("Details", "Inventory", new { id = model.InventoryId });
        }
    }
}