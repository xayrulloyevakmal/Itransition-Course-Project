using System.ComponentModel.DataAnnotations;

namespace Itransition_Course_Project.ViewModels
{
    public class ItemFormViewModel
    {
        [Required]
        public int InventoryId { get; set; }
        public string InventoryTitle { get; set; } = string.Empty;
        
        public bool CustomString1_State { get; set; }
        public string CustomString1_Name { get; set; } = string.Empty;
        public string? CustomString1_Value { get; set; }

        public bool CustomString2_State { get; set; }
        public string CustomString2_Name { get; set; } = string.Empty;
        public string? CustomString2_Value { get; set; }

        public bool CustomString3_State { get; set; }
        public string CustomString3_Name { get; set; } = string.Empty;
        public string? CustomString3_Value { get; set; }

        public bool CustomInt1_State { get; set; }
        public string CustomInt1_Name { get; set; } = string.Empty;
        public int? CustomInt1_Value { get; set; }

        public bool CustomInt2_State { get; set; }
        public string CustomInt2_Name { get; set; } = string.Empty;
        public int? CustomInt2_Value { get; set; }

        public bool CustomInt3_State { get; set; }
        public string CustomInt3_Name { get; set; } = string.Empty;
        public int? CustomInt3_Value { get; set; }

        public bool CustomBool1_State { get; set; }
        public string CustomBool1_Name { get; set; } = string.Empty;
        public bool CustomBool1_Value { get; set; }

        public bool CustomBool2_State { get; set; }
        public string CustomBool2_Name { get; set; } = string.Empty;
        public bool CustomBool2_Value { get; set; }

        public bool CustomBool3_State { get; set; }
        public string CustomBool3_Name { get; set; } = string.Empty;
        public bool CustomBool3_Value { get; set; }
    }
}