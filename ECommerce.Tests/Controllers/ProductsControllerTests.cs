using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ECommerce.Api.Controllers;
using ECommerce.Business.Interfaces;
using ECommerce.Business.DTOs.Product;

namespace ECommerce.Tests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetAll_ShouldReturnOkWithProducts()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ProductDto> { new ProductDto { Id = 1, Name = "Test", ProductCode = "c1" } });

            var controller = new ProductsController(mockService.Object);

            var result = await controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(ok.Value);
            Assert.Single(products);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var mockService = new Mock<IProductService>();
            var createdDto = new ProductDto { Id = 2, Name = "New" };
            mockService.Setup(s => s.CreateAsync(It.IsAny<CreateProductRequest>())).ReturnsAsync(createdDto);

            var controller = new ProductsController(mockService.Object);

            var req = new CreateProductRequest
            {
                Category = 1,
                ProductCode = "x1",
                Name = "x",
                Price = 10,
                MinimumQuantity = 1,
                DiscountRate = 0,
                ImageFile = null // service is mocked so it won't be used
            };

            var actionResult = await controller.Create(req);

            var created = Assert.IsType<CreatedAtActionResult>(actionResult);
            Assert.Equal(nameof(controller.Get), created.ActionName);
        }
    }
}
