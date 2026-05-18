using System;
using System.ComponentModel.DataAnnotations;

namespace CustomInventoryApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        
        [Required]
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        [Required]
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string CustomId { get; set; } 
        
        public string CustomString1_Value { get; set; }
        public string CustomString2_Value { get; set; }
        public string CustomString3_Value { get; set; }

        public int? CustomInt1_Value { get; set; }
        public int? CustomInt2_Value { get; set; }
        public int? CustomInt3_Value { get; set; }

        public bool? CustomBool1_Value { get; set; }
        public bool? CustomBool2_Value { get; set; }
        public bool? CustomBool3_Value { get; set; }

        public string CustomText1_Value { get; set; }
        public string CustomText2_Value { get; set; }
        public string CustomText3_Value { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}