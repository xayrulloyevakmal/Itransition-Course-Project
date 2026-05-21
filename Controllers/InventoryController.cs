using System.Security.Claims;
using System.Threading.Tasks;
using Itransition_Course_Project.Models;
using Itransition_Course_Project.Services.Interfaces;
using Itransition_Course_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly IItemService _itemService;
        private readonly ICommentService _commentService;
        private readonly ICloudinaryService _cloudinaryService;

        public InventoryController(
            IInventoryService inventoryService,
            IItemService itemService,
            ICommentService commentService,
            ICloudinaryService cloudinaryService)
        {
            _inventoryService = inventoryService;
            _itemService = itemService;
            _commentService = commentService;
            _cloudinaryService = cloudinaryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var inventories = await _inventoryService.GetAllAsync();
            return View(inventories);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var inventory = await _inventoryService.GetByIdAsync(id);
            if (inventory == null) return NotFound();

            var items = await _itemService.GetByInventoryIdAsync(id);
            var comments = await _commentService.GetByInventoryIdAsync(id);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool canEdit = currentUserId == inventory.CreatorId || User.IsInRole("Admin");

            var viewModel = new InventoryDetailsViewModel
            {
                Inventory = inventory,
                Items = items,
                Comments = comments,
                CanEdit = canEdit,
                IsAdmin = User.IsInRole("Admin")
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateInventoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInventoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return Challenge();

            string? imageUrl = null;
            if (model.ImageFile != null)
            {
                imageUrl = await _cloudinaryService.UploadImageAsync(model.ImageFile);
            }

            var inventory = new Inventory
            {
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                ImageUrl = imageUrl,
                IsPublic = model.IsPublic,
                CreatorId = currentUserId,
                CustomIdFormat = model.CustomIdFormat,

                CustomString1_State = model.CustomString1_State,
                CustomString1_Name = model.CustomString1_Name,
                CustomString2_State = model.CustomString2_State,
                CustomString2_Name = model.CustomString2_Name,
                CustomString3_State = model.CustomString3_State,
                CustomString3_Name = model.CustomString3_Name,

                CustomInt1_State = model.CustomInt1_State,
                CustomInt1_Name = model.CustomInt1_Name,
                CustomInt2_State = model.CustomInt2_State,
                CustomInt2_Name = model.CustomInt2_Name,
                CustomInt3_State = model.CustomInt3_State,
                CustomInt3_Name = model.CustomInt3_Name,

                CustomBool1_State = model.CustomBool1_State,
                CustomBool1_Name = model.CustomBool1_Name,
                CustomBool2_State = model.CustomBool2_State,
                CustomBool2_Name = model.CustomBool2_Name,
                CustomBool3_State = model.CustomBool3_State,
                CustomBool3_Name = model.CustomBool3_Name,

                CustomText1_State = model.CustomText1_State,
                CustomText1_Name = model.CustomText1_Name,
                CustomText2_State = model.CustomText2_State,
                CustomText2_Name = model.CustomText2_Name,
                CustomText3_State = model.CustomText3_State,
                CustomText3_Name = model.CustomText3_Name,
            };

            await _inventoryService.CreateAsync(inventory);
            return RedirectToAction("Details", new { id = inventory.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var inventory = await _inventoryService.GetByIdAsync(id);
            if (inventory == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != inventory.CreatorId && !User.IsInRole("Admin"))
                return Forbid();

            var viewModel = new EditInventoryViewModel
            {
                Id = inventory.Id,
                Title = inventory.Title,
                Description = inventory.Description ?? string.Empty,
                Category = inventory.Category ?? string.Empty,
                ImageUrl = inventory.ImageUrl,
                IsPublic = inventory.IsPublic,
                CustomIdFormat = inventory.CustomIdFormat ?? "INV-{0:0000}",
                RowVersion = inventory.RowVersion,

                CustomString1_State = inventory.CustomString1_State,
                CustomString1_Name = inventory.CustomString1_Name ?? "Custom String 1",
                CustomString2_State = inventory.CustomString2_State,
                CustomString2_Name = inventory.CustomString2_Name ?? "Custom String 2",
                CustomString3_State = inventory.CustomString3_State,
                CustomString3_Name = inventory.CustomString3_Name ?? "Custom String 3",

                CustomInt1_State = inventory.CustomInt1_State,
                CustomInt1_Name = inventory.CustomInt1_Name ?? "Custom Integer 1",
                CustomInt2_State = inventory.CustomInt2_State,
                CustomInt2_Name = inventory.CustomInt2_Name ?? "Custom Integer 2",
                CustomInt3_State = inventory.CustomInt3_State,
                CustomInt3_Name = inventory.CustomInt3_Name ?? "Custom Integer 3",

                CustomBool1_State = inventory.CustomBool1_State,
                CustomBool1_Name = inventory.CustomBool1_Name ?? "Custom Boolean 1",
                CustomBool2_State = inventory.CustomBool2_State,
                CustomBool2_Name = inventory.CustomBool2_Name ?? "Custom Boolean 2",
                CustomBool3_State = inventory.CustomBool3_State,
                CustomBool3_Name = inventory.CustomBool3_Name ?? "Custom Boolean 3",

                CustomText1_State = inventory.CustomText1_State,
                CustomText1_Name = inventory.CustomText1_Name ?? "Custom Text 1",
                CustomText2_State = inventory.CustomText2_State,
                CustomText2_Name = inventory.CustomText2_Name ?? "Custom Text 2",
                CustomText3_State = inventory.CustomText3_State,
                CustomText3_Name = inventory.CustomText3_Name ?? "Custom Text 3",
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditInventoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return Challenge();

            string? imageUrl = model.ImageUrl;
            if (model.ImageFile != null)
            {
                imageUrl = await _cloudinaryService.UploadImageAsync(model.ImageFile);
            }

            var inventory = new Inventory
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                ImageUrl = imageUrl,
                IsPublic = model.IsPublic,
                CustomIdFormat = model.CustomIdFormat,

                CustomString1_State = model.CustomString1_State,
                CustomString1_Name = model.CustomString1_Name,
                CustomString2_State = model.CustomString2_State,
                CustomString2_Name = model.CustomString2_Name,
                CustomString3_State = model.CustomString3_State,
                CustomString3_Name = model.CustomString3_Name,

                CustomInt1_State = model.CustomInt1_State,
                CustomInt1_Name = model.CustomInt1_Name,
                CustomInt2_State = model.CustomInt2_State,
                CustomInt2_Name = model.CustomInt2_Name,
                CustomInt3_State = model.CustomInt3_State,
                CustomInt3_Name = model.CustomInt3_Name,

                CustomBool1_State = model.CustomBool1_State,
                CustomBool1_Name = model.CustomBool1_Name,
                CustomBool2_State = model.CustomBool2_State,
                CustomBool2_Name = model.CustomBool2_Name,
                CustomBool3_State = model.CustomBool3_State,
                CustomBool3_Name = model.CustomBool3_Name,

                CustomText1_State = model.CustomText1_State,
                CustomText1_Name = model.CustomText1_Name,
                CustomText2_State = model.CustomText2_State,
                CustomText2_Name = model.CustomText2_Name,
                CustomText3_State = model.CustomText3_State,
                CustomText3_Name = model.CustomText3_Name,
            };

            try
            {
                var result = await _inventoryService.UpdateAsync(inventory, model.RowVersion!);
                if (result == null) return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError(string.Empty,
                    "The record was modified by another user. Please reload and try again.");
                return View(model);
            }

            return RedirectToAction("Details", new { id = model.Id });
        }

        // Auto-save endpoint for inventory settings (AJAX, every 8 seconds)
        [HttpPost]
        public async Task<IActionResult> AutoSave([FromBody] EditInventoryViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Validation failed." });

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existing = await _inventoryService.GetByIdAsync(model.Id);
            if (existing == null)
                return Json(new { success = false, message = "Inventory not found." });

            if (currentUserId != existing.CreatorId && !User.IsInRole("Admin"))
                return Json(new { success = false, message = "Access denied." });

            var inventory = new Inventory
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                ImageUrl = model.ImageUrl,
                IsPublic = model.IsPublic,
                CustomIdFormat = model.CustomIdFormat,
                CustomString1_State = model.CustomString1_State,
                CustomString1_Name = model.CustomString1_Name,
                CustomString2_State = model.CustomString2_State,
                CustomString2_Name = model.CustomString2_Name,
                CustomString3_State = model.CustomString3_State,
                CustomString3_Name = model.CustomString3_Name,
                CustomInt1_State = model.CustomInt1_State,
                CustomInt1_Name = model.CustomInt1_Name,
                CustomInt2_State = model.CustomInt2_State,
                CustomInt2_Name = model.CustomInt2_Name,
                CustomInt3_State = model.CustomInt3_State,
                CustomInt3_Name = model.CustomInt3_Name,
                CustomBool1_State = model.CustomBool1_State,
                CustomBool1_Name = model.CustomBool1_Name,
                CustomBool2_State = model.CustomBool2_State,
                CustomBool2_Name = model.CustomBool2_Name,
                CustomBool3_State = model.CustomBool3_State,
                CustomBool3_Name = model.CustomBool3_Name,
                CustomText1_State = model.CustomText1_State,
                CustomText1_Name = model.CustomText1_Name,
                CustomText2_State = model.CustomText2_State,
                CustomText2_Name = model.CustomText2_Name,
                CustomText3_State = model.CustomText3_State,
                CustomText3_Name = model.CustomText3_Name,
            };

            try
            {
                var result = await _inventoryService.UpdateAsync(inventory, model.RowVersion!);
                if (result == null)
                    return Json(new { success = false, message = "Not found." });

                return Json(new
                {
                    success = true,
                    newVersion = Convert.ToBase64String(result.RowVersion!)
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new
                {
                    success = false,
                    message = "Conflict: Another user has modified this inventory. Please reload."
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _inventoryService.GetByIdAsync(id);
            if (inventory == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != inventory.CreatorId && !User.IsInRole("Admin"))
                return Forbid();

            await _inventoryService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}