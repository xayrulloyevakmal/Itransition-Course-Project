using System;
using System.Linq;
using System.Threading.Tasks;
using Itransition_Course_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .ToListAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockUsers(string[] selectedUserIds)
        {
            if (selectedUserIds == null || selectedUserIds.Length == 0)
                return RedirectToAction(nameof(Users));

            foreach (var userId in selectedUserIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) continue;

                user.IsBlocked = true;
                await _userManager.UpdateAsync(user);
                await _userManager.UpdateSecurityStampAsync(user);
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnblockUsers(string[] selectedUserIds)
        {
            if (selectedUserIds == null || selectedUserIds.Length == 0)
                return RedirectToAction(nameof(Users));

            foreach (var userId in selectedUserIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) continue;

                user.IsBlocked = false;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUsers(string[] selectedUserIds)
        {
            if (selectedUserIds == null || selectedUserIds.Length == 0)
                return RedirectToAction(nameof(Users));

            var currentUserId = _userManager.GetUserId(User);
            bool deletedSelf = selectedUserIds.Contains(currentUserId);

            foreach (var userId in selectedUserIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                    await _userManager.DeleteAsync(user);
            }

            if (deletedSelf)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Inventory");
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
                await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Users));
        }
    }
}