using System.Collections.Generic;
using System.Threading.Tasks;
using Itransition_Course_Project.Models;
using Itransition_Course_Project.ViewModels;

namespace Itransition_Course_Project.Services.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResultsViewModel> SearchAsync(string query);
    }
}
