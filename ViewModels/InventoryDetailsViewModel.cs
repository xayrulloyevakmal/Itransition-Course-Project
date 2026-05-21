using System.Collections.Generic;
using Itransition_Course_Project.Models;

namespace Itransition_Course_Project.ViewModels
{
    public class InventoryDetailsViewModel
    {
        public Inventory Inventory { get; set; } = null!;
        public IEnumerable<Item> Items { get; set; } = new List<Item>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public bool CanEdit { get; set; }
        public bool IsAdmin { get; set; }
    }
}