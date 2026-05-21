using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Itransition_Course_Project.Data;
using Itransition_Course_Project.Models;
using Itransition_Course_Project.Services.Interfaces;
using Itransition_Course_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IInventoryService _inventoryService;
        private readonly ICustomIdGeneratorService _idGenerator;

        public ItemController(
            IItemService itemService,
            IInventoryService inventoryService,
            ICustomIdGeneratorService idGenerator)
        {
            _itemService = itemService;
            _inventoryService = inventoryService;
            _idGenerator = idGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int inventoryId)
        {
            var inventory = await _inventoryService.GetByIdAsync(inventoryId);
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
                CustomBool3_State = inventory.CustomBool3_State, CustomBool3_Name = inventory.CustomBool3_Name,

                CustomText1_State = inventory.CustomText1_State, CustomText1_Name = inventory.CustomText1_Name,
                CustomText2_State = inventory.CustomText2_State, CustomText2_Name = inventory.CustomText2_Name,
                CustomText3_State = inventory.CustomText3_State, CustomText3_Name = inventory.CustomText3_Name,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var inventory = await _inventoryService.GetByIdAsync(model.InventoryId);
            if (inventory == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return Challenge();

            string generatedId = await _idGenerator.GenerateAsync(
                model.InventoryId, inventory.CustomIdFormat ?? "INV-{0:0000}");

            var item = new Item
            {
                InventoryId = model.InventoryId,
                CreatedById = currentUserId,
                CreatedAt = DateTime.UtcNow,
                CustomId = generatedId,

                CustomString1_Value = model.CustomString1_Value,
                CustomString2_Value = model.CustomString2_Value,
                CustomString3_Value = model.CustomString3_Value,

                CustomInt1_Value = model.CustomInt1_Value,
                CustomInt2_Value = model.CustomInt2_Value,
                CustomInt3_Value = model.CustomInt3_Value,

                CustomBool1_Value = model.CustomBool1_Value,
                CustomBool2_Value = model.CustomBool2_Value,
                CustomBool3_Value = model.CustomBool3_Value,

                CustomText1_Value = model.CustomText1_Value,
                CustomText2_Value = model.CustomText2_Value,
                CustomText3_Value = model.CustomText3_Value,
            };

            try
            {
                await _itemService.CreateAsync(item);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "An item with this ID already exists. Please try again.");
                return View(model);
            }

            return RedirectToAction("Details", "Inventory", new { id = model.InventoryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int inventoryId)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != item.CreatedById && !User.IsInRole("Admin"))
                return Forbid();

            await _itemService.DeleteAsync(id);
            return RedirectToAction("Details", "Inventory", new { id = inventoryId });
        }
    }
}