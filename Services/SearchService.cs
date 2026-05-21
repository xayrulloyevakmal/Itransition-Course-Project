using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itransition_Course_Project.Data;
using Itransition_Course_Project.Models;
using Itransition_Course_Project.Services.Interfaces;
using Itransition_Course_Project.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Services
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SearchResultsViewModel> SearchAsync(string query)
        {
            var viewModel = new SearchResultsViewModel { SearchQuery = query };

            if (string.IsNullOrWhiteSpace(query))
                return viewModel;

            string pattern = $"%{query.Trim()}%";

            viewModel.MatchedInventories = await _context.Inventories
                .Include(i => i.Creator)
                .Where(i =>
                    EF.Functions.ILike(i.Title, pattern) ||
                    (i.Description != null && EF.Functions.ILike(i.Description, pattern)) ||
                    (i.Category != null && EF.Functions.ILike(i.Category, pattern)))
                .AsNoTracking()
                .ToListAsync();

            viewModel.MatchedItems = await _context.Items
                .Include(i => i.Inventory)
                .Include(i => i.CreatedBy)
                .Where(i =>
                    (i.CustomId != null && EF.Functions.ILike(i.CustomId, pattern)) ||
                    (i.CustomString1_Value != null && EF.Functions.ILike(i.CustomString1_Value, pattern)) ||
                    (i.CustomString2_Value != null && EF.Functions.ILike(i.CustomString2_Value, pattern)) ||
                    (i.CustomString3_Value != null && EF.Functions.ILike(i.CustomString3_Value, pattern)) ||
                    (i.CustomText1_Value != null && EF.Functions.ILike(i.CustomText1_Value, pattern)) ||
                    (i.CustomText2_Value != null && EF.Functions.ILike(i.CustomText2_Value, pattern)) ||
                    (i.CustomText3_Value != null && EF.Functions.ILike(i.CustomText3_Value, pattern)))
                .AsNoTracking()
                .ToListAsync();

            return viewModel;
        }
    }
}
