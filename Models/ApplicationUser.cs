using Microsoft.AspNetCore.Identity;

namespace Itransition_Course_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Language { get; set; } = "en";
        public string Theme { get; set; } = "light";
        public bool IsBlocked { get; set; }
    }
}