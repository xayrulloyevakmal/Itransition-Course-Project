using System.Collections.Generic;
using Itransition_Course_Project.Models;

namespace Itransition_Course_Project.ViewModels
{
    public class SearchResultsViewModel
    {
        public string SearchQuery { get; set; }
        public List<Inventory> MatchedInventories { get; set; } = new List<Inventory>();
        public List<Item> MatchedItems { get; set; } = new List<Item>();
    }
}