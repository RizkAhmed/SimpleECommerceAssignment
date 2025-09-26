using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using ECommerce.Business.Services;
using ECommerce.Data.Interfaces;
using ECommerce.Data.Entities;
using ECommerce.Business.Interfaces;
using ECommerce.Business.DTOs.Product;
using System.Collections.Generic;

namespace ECommerce.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _productRepoMock = new Mock<IProductRepository>();
            _fileServiceMock = new Mock<IFileService>();

            _uowMock.Setup(u => u.Products).Returns(_productRepoMock.Object);

            _service = new ProductService(_uowMock.Object, _fileServiceMock.Object);
        }

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
        public async Task CreateAsync_ShouldThrow_WhenCodeExists()
        {
            var req = new CreateProductRequest
            {
                Category = 1,
                ProductCode = "P1",
                Name = "N",
                Price = 10,
                MinimumQuantity = 1,
                DiscountRate = 0,
                ImageFile = CreateFormFile()
            };

            _productRepoMock.Setup(r => r.GetByCodeAsync("P1")).ReturnsAsync(new Product { Id = 5 });

            await Assert.ThrowsAsync<System.Exception>(() => _service.CreateAsync(req));
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnProduct_WhenValid()
        {
            var req = new CreateProductRequest
            {
                Category = 1,
                ProductCode = "P2",
                Name = "N2",
                Price = 20,
                MinimumQuantity = 1,
                DiscountRate = 0,
                ImageFile = CreateFormFile()
            };

            _productRepoMock.Setup(r => r.GetByCodeAsync("P2")).ReturnsAsync((Product)null);

            _fileServiceMock.Setup(f => f.SaveFileAsync(It.IsAny<IFormFile>()))
                            .ReturnsAsync("images/p2.jpg");

            _productRepoMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                            .Callback<Product>(p => p.Id = 100)
                            .Returns(Task.CompletedTask);

            _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.CreateAsync(req);

            Assert.Equal(100, result.Id);
            Assert.Equal("P2", result.ProductCode);
            _fileServiceMock.Verify(f => f.SaveFileAsync(It.IsAny<IFormFile>()), Times.Once);
            _productRepoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveAndSave()
        {
            var product = new Product { Id = 50 };
            _productRepoMock.Setup(r => r.GetByIdAsync(50)).ReturnsAsync(product);
            _productRepoMock.Setup(r => r.Remove(It.IsAny<Product>()));
            _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            await _service.DeleteAsync(50);

            _productRepoMock.Verify(r => r.Remove(It.Is<Product>(p => p.Id == 50)), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnDtos()
        {
            var list = new List<Product> { new Product { Id = 1, Name = "A", ProductCode = "c1" } };
            _productRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
        }
    }
}
