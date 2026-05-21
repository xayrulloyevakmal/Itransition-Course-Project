using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Itransition_Course_Project.ViewModels
{
    public class EditInventoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }

        public bool IsPublic { get; set; } = true;

        [Required(ErrorMessage = "Custom ID format is required.")]
        public string CustomIdFormat { get; set; } = "INV-{0:0000}";

        public byte[]? RowVersion { get; set; }

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

        public bool CustomText1_State { get; set; }
        public string CustomText1_Name { get; set; } = "Custom Text 1";
        public bool CustomText2_State { get; set; }
        public string CustomText2_Name { get; set; } = "Custom Text 2";
        public bool CustomText3_State { get; set; }
        public string CustomText3_Name { get; set; } = "Custom Text 3";
    }
}
