using System;
using System.Linq;
using System.Threading.Tasks;
using CustomInventoryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Controllers
{
    [Authorize(Roles = "Admin")] 
    public class AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await userManager.Users.OrderBy(u => u.UserName).ToListAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockUsers(string[] selectedUserIds)
        {
            if (selectedUserIds == null || selectedUserIds.Length == 0) return RedirectToAction(nameof(Users));

            foreach (var userId in selectedUserIds)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                    
                    await userManager.UpdateSecurityStampAsync(user);
                }
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnblockUsers(string[] selectedUserIds)
        {
            if (selectedUserIds == null || selectedUserIds.Length == 0) return RedirectToAction(nameof(Users));

            foreach (var userId in selectedUserIds)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await userManager.SetLockoutEndDateAsync(user, null);
                }
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUsers(string[] selectedUserIds)
        {
            if (selectedUserIds == null || selectedUserIds.Length == 0) return RedirectToAction(nameof(Users));

            foreach (var userId in selectedUserIds)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await userManager.DeleteAsync(user);
                }
            }
            
            var currentUserId = userManager.GetUserId(User);
            if (selectedUserIds.Contains(currentUserId))
            {
                await signInManager.SignOutAsync();
                return RedirectToAction("Index", "Inventory");
            }

            return RedirectToAction(nameof(Users));
        }
    }
}