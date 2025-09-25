using ECommerce.Business.Interfaces;
using ECommerce.Business.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ECommerce.Business.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly FileSettings _fileSettings;

        public FileService(IWebHostEnvironment environment, IOptions<FileSettings> fileSettings)
        {
            _environment = environment;
            _fileSettings = fileSettings.Value;
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            // Determine folder and validate extension
            string targetFolder;
            if (_fileSettings.AllowedImageExtensions.Contains(fileExtension))
            {
                targetFolder = _fileSettings.ImageFolderPath;
            }
            else if (_fileSettings.AllowedFileExtensions.Contains(fileExtension))
            {
                targetFolder = _fileSettings.FileFolderPath;
            }
            else
            {
                throw new ArgumentException($"Unsupported file type: {fileExtension}. Allowed types are: " +
                    $"{string.Join(", ", _fileSettings.AllowedImageExtensions.Concat(_fileSettings.AllowedFileExtensions))}");
            }

            // Validate file size
            if (file.Length > _fileSettings.MaxFileSize)
            {
                throw new ArgumentException($"File size cannot exceed {_fileSettings.MaxFileSize / 1024 / 1024} MB.");
            }

            // Generate unique file name
            var fileName = $"{Path.GetRandomFileName()}-{file.FileName}";

            // Define upload path
            var uploadsPath = Path.Combine(_environment.WebRootPath, targetFolder);
            Directory.CreateDirectory(uploadsPath); // Ensure directory exists

            var filePath = Path.Combine(uploadsPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path
            return Path.Combine(targetFolder, fileName).Replace("\\", "/");
        }

        public void DeleteFile(string relativePath)
        {
                var fullPath = Path.Combine(_environment.WebRootPath, relativePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            else
            {
                throw new ArgumentException($"File not founded");
            }
        }
    }
}
