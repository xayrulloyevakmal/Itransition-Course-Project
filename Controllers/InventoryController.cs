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
    [Authorize] // Only authenticated registered users can manipulate inventory templates
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var publicInventories = await _context.Inventories
                .Include(i => i.Creator)
                .AsNoTracking()
                .ToListAsync();
            return View(publicInventories);
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

            var inventory = new Inventory
            {
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
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
                CustomBool3_Name = model.CustomBool3_Name
            };

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = inventory.Id });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Creator)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null) return NotFound();

            var items = await _context.Items
                .Include(i => i.CreatedBy)
                .Where(i => i.InventoryId == id)
                .AsNoTracking()
                .ToListAsync();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool canEdit = currentUserId == inventory.CreatorId || User.IsInRole("Admin");

            var viewModel = new InventoryDetailsViewModel
            {
                Inventory = inventory,
                Items = items,
                CanEdit = canEdit,
                IsAdmin = User.IsInRole("Admin")
            };

            return View(viewModel);
        }
    }
}