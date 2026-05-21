using System.Threading.Tasks;
using Itransition_Course_Project.Models;
using Itransition_Course_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Itransition_Course_Project.Controllers
{
    public class AccountPreferencesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountPreferencesController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> SaveTheme([FromBody] ThemeSelectionModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Theme))
                return BadRequest(new { success = false, message = "Invalid theme." });

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

            Response.Cookies.Append("theme", selectedTheme, new CookieOptions
            {
                Expires = System.DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true
            });

            return Json(new { success = true, currentTheme = selectedTheme });
        }

        [HttpPost]
        public async Task<IActionResult> SaveLanguage([FromBody] LanguageSelectionModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Language))
                return BadRequest(new { success = false, message = "Invalid language." });

            string selectedLang = model.Language.ToLower() == "uz" ? "uz" : "en";

            if (User.Identity is { IsAuthenticated: true })
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.Language = selectedLang;
                    await _userManager.UpdateAsync(user);
                }
            }

            Response.Cookies.Append(".AspNetCore.Culture", $"c={selectedLang}|uic={selectedLang}", new CookieOptions
            {
                Expires = System.DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true
            });

            return Json(new { success = true, currentLanguage = selectedLang });
        }
    }
}