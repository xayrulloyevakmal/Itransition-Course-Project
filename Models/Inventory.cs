using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomInventoryApp.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        
        public string Description { get; set; } 
        public string ImageUrl { get; set; } 
        
        [Required]
        public string Category { get; set; } 
        
        [Required]
        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }
        
        public bool IsPublic { get; set; }
        public string CustomIdFormat { get; set; } 

        // Custom Fields Mapping Slots (Max 3 per explicit type required)
        public bool CustomString1_State { get; set; }
        public string CustomString1_Name { get; set; }
        public bool CustomString2_State { get; set; }
        public string CustomString2_Name { get; set; }
        public bool CustomString3_State { get; set; }
        public string CustomString3_Name { get; set; }

        public bool CustomInt1_State { get; set; }
        public string CustomInt1_Name { get; set; }
        public bool CustomInt2_State { get; set; }
        public string CustomInt2_Name { get; set; }
        public bool CustomInt3_State { get; set; }
        public string CustomInt3_Name { get; set; }

        public bool CustomBool1_State { get; set; }
        public string CustomBool1_Name { get; set; }
        public bool CustomBool2_State { get; set; }
        public string CustomBool2_Name { get; set; }
        public bool CustomBool3_State { get; set; }
        public string CustomBool3_Name { get; set; }

        public bool CustomText1_State { get; set; }
        public string CustomText1_Name { get; set; }
        public bool CustomText2_State { get; set; }
        public string CustomText2_Name { get; set; }
        public bool CustomText3_State { get; set; }
        public string CustomText3_Name { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}