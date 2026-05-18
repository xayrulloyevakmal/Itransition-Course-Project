using System.Collections.Generic;
using CustomInventoryApp.Models;

namespace Itransition_Course_Project.ViewModels
{
    public class InventoryDetailsViewModel
    {
        public Inventory Inventory { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public bool CanEdit { get; set; }
        public bool IsAdmin { get; set; }
    }
}