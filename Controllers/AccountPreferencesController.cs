using System.Threading.Tasks;
using CustomInventoryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Itransition_Course_Project.Controllers
{
    public class AccountPreferencesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public AccountPreferencesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> SaveTheme([FromBody] ThemeSelectionModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Theme))
            {
                return BadRequest(new { success = false, message = "Invalid theme payload." });
            }
            
            string selectedTheme = model.Theme.ToLower() == "dark" ? "dark" : "light";

            if (User.Identity is { IsAuthenticated: true })
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.Theme = selectedTheme;
                    await _userManager.UpdateAsync(user);
                    
                    await _signInManager.RefreshSignInAsync(user);
                }
            }

            return Json(new { success = true, currentTheme = selectedTheme });
        }
    }

    public class ThemeSelectionModel
    {
        public string Theme { get; set; } = "light";
    }
}