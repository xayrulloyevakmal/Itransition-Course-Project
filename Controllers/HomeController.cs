using System.Linq;
using System.Threading.Tasks;
using CustomInventoryApp.Data;
using Itransition_Course_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Inventory");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var viewModel = new SearchResultsViewModel { SearchQuery = query ?? "" };

            if (string.IsNullOrWhiteSpace(query))
            {
                return View(viewModel);
            }

            string lowerQuery = query.ToLower().Trim();
            
            viewModel.MatchedInventories = await _context.Inventories
                .Include(i => i.Creator)
                .Where(i => i.Title.ToLower().Contains(lowerQuery) || 
                            i.Description.ToLower().Contains(lowerQuery) || 
                            i.Category.ToLower().Contains(lowerQuery))
                .AsNoTracking()
                .ToListAsync();
            
            viewModel.MatchedItems = await _context.Items
                .Include(i => i.Inventory)
                .Include(i => i.CreatedBy)
                .Where(i => i.CustomId.ToLower().Contains(lowerQuery) ||
                            i.CustomString1_Value.ToLower().Contains(lowerQuery) ||
                            i.CustomString2_Value.ToLower().Contains(lowerQuery) ||
                            i.CustomString3_Value.ToLower().Contains(lowerQuery))
                .AsNoTracking()
                .ToListAsync();

            return View(viewModel);
        }
    }
}