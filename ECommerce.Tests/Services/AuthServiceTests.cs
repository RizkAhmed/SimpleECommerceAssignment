
using System.Security.Claims;
using System.Text;
using ECommerce.Data.Interfaces;
using ECommerce.Business.Services;
using ECommerce.Business.Options;
using Moq;
using Microsoft.Extensions.Options;
using ECommerce.Data.Entities;
using ECommerce.Business.DTOs.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ECommerce.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly AuthService _service;
        private readonly JwtOptions _jwtOptions;

        public AuthServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _userRepoMock = new Mock<IUserRepository>();
            _uowMock.Setup(u => u.Users).Returns(_userRepoMock.Object);

            _jwtOptions = new JwtOptions
            {
                Key = "supersecretkey_for_tests_must_be_long_enough_123456",
                Issuer = "test",
                Audience = "test",
                AccessTokenExpirationMinutes = 60,
                RefreshTokenExpirationDays = 7
            };
            var options = Options.Create(_jwtOptions);

            _service = new AuthService(_uowMock.Object, options);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowUnauthorized_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetByUserNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                         .ReturnsAsync((User)null);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.LoginAsync(new Business.DTOs.Auth.LoginRequestDto("u", "p")));
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnTokens_WhenUserExists()
        {
            var user = new User { Id = 1, UserName = "u", Email = "e@e.com", Password = "p" };
            _userRepoMock.Setup(r => r.GetByUserNameAndPasswordAsync("u", "p")).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.Update(It.IsAny<User>()));
            _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.LoginAsync(new LoginRequestDto("u", "p"));

            Assert.False(string.IsNullOrEmpty(result.AccessToken));
            Assert.False(string.IsNullOrEmpty(result.RefreshToken));
            Assert.True(result.Expiration > DateTime.UtcNow);
            _userRepoMock.Verify(r => r.Update(It.Is<User>(x => x.Id == 1)), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnNewTokens_WhenValid()
        {
            var user = new User
            {
                Id = 2,
                UserName = "u2",
                Email = "e2@e.com",
                RefreshToken = "ref123",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1)
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(user);
            _uowMock.Setup(u => u.Users).Returns(_userRepoMock.Object);
            _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var token = CreateJwt(2);

            var result = await _service.RefreshTokenAsync(token, "ref123");

            Assert.False(string.IsNullOrEmpty(result.AccessToken));
            Assert.False(string.IsNullOrEmpty(result.RefreshToken));
            _userRepoMock.Verify(r => r.Update(It.Is<User>(x => x.Id == 2)), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        private string CreateJwt(int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var token = new JwtSecurityToken(_jwtOptions.Issuer, _jwtOptions.Audience, claims,
                expires: DateTime.UtcNow.AddMinutes(5), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
