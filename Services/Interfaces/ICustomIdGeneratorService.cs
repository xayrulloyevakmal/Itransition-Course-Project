using System.Threading.Tasks;

namespace Itransition_Course_Project.Services.Interfaces
{
    public interface ICustomIdGeneratorService
    {
        Task<string> GenerateAsync(int inventoryId, string format);
    }
}