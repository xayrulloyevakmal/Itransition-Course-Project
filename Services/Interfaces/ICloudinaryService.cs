using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Itransition_Course_Project.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string?> UploadImageAsync(IFormFile file);
        Task<bool> DeleteImageAsync(string publicId);
    }
}
