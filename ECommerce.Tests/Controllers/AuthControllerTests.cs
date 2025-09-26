using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ECommerce.Api.Controllers;
using ECommerce.Business.Interfaces;
using ECommerce.Business.DTOs.Auth;

namespace ECommerce.Tests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_ShouldReturnOk_WhenServiceReturns()
        {
            var mockAuth = new Mock<IAuthService>();
            mockAuth.Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                    .ReturnsAsync(new TokenResponseDto("a", "r", System.DateTime.UtcNow.AddMinutes(5)));

            var controller = new AuthController(mockAuth.Object);

            var res = await controller.Login(new LoginRequestDto("u", "p"));

            var ok = Assert.IsType<OkObjectResult>(res);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task Register_ShouldReturnOk()
        {
            var mockAuth = new Mock<IAuthService>();
            mockAuth.Setup(s => s.Register(It.IsAny<RegisterRequestDto>()))
                    .ReturnsAsync(new ECommerce.Business.DTOs.User.UserDto { Id = 1, UserName = "u" });

            var controller = new AuthController(mockAuth.Object);

            var res = await controller.Register(new RegisterRequestDto { UserName = "u", Email = "e@e.com", Password = "p" });

            var ok = Assert.IsType<OkObjectResult>(res);
            Assert.NotNull(ok.Value);
        }
    }
}
