using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);
        void DeleteFile(string relativePath);
    }
}
