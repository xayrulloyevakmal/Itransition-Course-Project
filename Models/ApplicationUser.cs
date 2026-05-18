using Microsoft.AspNetCore.Identity;

namespace CustomInventoryApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Language { get; set; } = "en";
        public string Theme { get; set; } = "light";
    }
}