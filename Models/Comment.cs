using System;
using System.ComponentModel.DataAnnotations;

namespace Itransition_Course_Project.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public int InventoryId { get; set; }
        public Inventory? Inventory { get; set; }

        [Required]
        public string AuthorId { get; set; } = string.Empty;
        public ApplicationUser? Author { get; set; }

        [Required]
        [StringLength(2000)]
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
