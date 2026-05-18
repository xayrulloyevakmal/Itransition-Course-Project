using System.ComponentModel.DataAnnotations;

namespace Itransition_Course_Project.ViewModels
{
    public class CreateInventoryViewModel
    {
        [Required(ErrorMessage = "Inventory name header title is required.")]
        [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "A description or purpose must be specified.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please assign a classification category.")]
        public string Category { get; set; } = string.Empty;

        public bool IsPublic { get; set; } = true;

        [Required(ErrorMessage = "Custom ID generation template string format is required.")]
        public string CustomIdFormat { get; set; } = "INV-{0:0000}";
        
        public bool CustomString1_State { get; set; }
        public string CustomString1_Name { get; set; } = "Custom String 1";
        public bool CustomString2_State { get; set; }
        public string CustomString2_Name { get; set; } = "Custom String 2";
        public bool CustomString3_State { get; set; }
        public string CustomString3_Name { get; set; } = "Custom String 3";
        
        public bool CustomInt1_State { get; set; }
        public string CustomInt1_Name { get; set; } = "Custom Integer 1";
        public bool CustomInt2_State { get; set; }
        public string CustomInt2_Name { get; set; } = "Custom Integer 2";
        public bool CustomInt3_State { get; set; }
        public string CustomInt3_Name { get; set; } = "Custom Integer 3";
        
        public bool CustomBool1_State { get; set; }
        public string CustomBool1_Name { get; set; } = "Custom Boolean 1";
        public bool CustomBool2_State { get; set; }
        public string CustomBool2_Name { get; set; } = "Custom Boolean 2";
        public bool CustomBool3_State { get; set; }
        public string CustomBool3_Name { get; set; } = "Custom Boolean 3";
    }
}