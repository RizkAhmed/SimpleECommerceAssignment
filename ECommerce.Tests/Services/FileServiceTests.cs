using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using ECommerce.Business.Services;
using ECommerce.Business.Options;

namespace ECommerce.Tests.Services
{
    public class FileServiceTests
    {
        private IFormFile CreateFormFile(string fileName = "img.jpg", string content = "hello")
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),  
                ContentType = "image/jpeg"
            };
        }

        [Fact]
        public async Task SaveFileAsync_ShouldSaveAndReturnRelativePath()
        {
            var tempRoot = Path.Combine(Path.GetTempPath(), "fs_tests_" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempRoot);

            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(e => e.WebRootPath).Returns(tempRoot);

            var settings = new FileSettings
            {
                ImageFolderPath = "uploads/images",
                FileFolderPath = "uploads/files",
                AllowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" },
                AllowedFileExtensions = new[] { ".pdf" },
                MaxFileSize = 5 * 1024 * 1024
            };

            var service = new FileService(envMock.Object, Options.Create(settings));

            var formFile = CreateFormFile("img.jpg", "content");
            var relPath = await service.SaveFileAsync(formFile);

            var fullPath = Path.Combine(tempRoot, relPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            Assert.True(System.IO.File.Exists(fullPath));

            // cleanup
            if (Directory.Exists(tempRoot)) Directory.Delete(tempRoot, true);
        }

        [Fact]
        public void DeleteFile_ShouldDeleteExistingFile()
        {
            var tempRoot = Path.Combine(Path.GetTempPath(), "fs_tests_" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempRoot);
            var uploads = Path.Combine(tempRoot, "uploads/images");
            Directory.CreateDirectory(uploads);
            var filePath = Path.Combine(uploads, "a.jpg");
            System.IO.File.WriteAllText(filePath, "x");

            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(e => e.WebRootPath).Returns(tempRoot);

            var settings = new FileSettings
            {
                ImageFolderPath = "uploads/images",
                FileFolderPath = "uploads/files",
                AllowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" },
                AllowedFileExtensions = new[] { ".pdf" },
                MaxFileSize = 5 * 1024 * 1024
            };

            var service = new FileService(envMock.Object, Options.Create(settings));

            service.DeleteFile(Path.Combine("uploads/images", "a.jpg").Replace("\\", "/"));

            Assert.False(System.IO.File.Exists(filePath));

            if (Directory.Exists(tempRoot)) Directory.Delete(tempRoot, true);
        }
    }
}
