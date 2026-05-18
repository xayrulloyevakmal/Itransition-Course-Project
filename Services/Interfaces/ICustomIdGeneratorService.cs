using System.Threading.Tasks;

namespace CustomInventoryApp.Services.Interfaces
{
    public interface ICustomIdGeneratorService
    {
        Task<string> GenerateAsync(int inventoryId, string format);
    }
}