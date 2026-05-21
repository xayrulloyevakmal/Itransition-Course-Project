using System.Threading.Tasks;
using Itransition_Course_Project.Services.Interfaces;
using Itransition_Course_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Itransition_Course_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchService _searchService;

        public HomeController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Inventory");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? query)
        {
            var viewModel = await _searchService.SearchAsync(query ?? "");
            return View(viewModel);
        }
    }
}