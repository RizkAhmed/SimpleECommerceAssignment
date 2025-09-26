using ECommerce.Business.DTOs.Auth;
using ECommerce.Business.DTOs.User;
using ECommerce.Business.Interfaces;
using ECommerce.Business.Options;
using ECommerce.Data.Entities;
using ECommerce.Data.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtOptions _jwtOptions;

        public AuthService(IUnitOfWork unitOfWork, IOptions<JwtOptions> jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _unitOfWork.Users.GetByUserNameAndPasswordAsync(request.UserName, request.Password);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            user.LastLoginTime = DateTime.UtcNow;

            return await GenerateTokensAsync(user);
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string token, string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (!int.TryParse(jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
                throw new UnauthorizedAccessException("Invalid token");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null
                || user.RefreshToken != refreshToken   
                || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            return await GenerateTokensAsync(user);
        }

        public async Task<UserDto> Register(RegisterRequestDto request)
        {
            var existingEmail = await _unitOfWork.Users.GetByEmailAsync(request.Email);

            if (existingEmail != null)
                throw new Exception("User email already exists");

            var existingUserName = await _unitOfWork.Users.GetByUserNameAsync(request.UserName);

            if (existingUserName != null)
                throw new Exception("User name already exists");



            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
            };


            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return UserDto.Map(user);
        }

        private async Task<TokenResponseDto> GenerateTokensAsync(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var accessTokenExpires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: accessTokenExpires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // refresh token regeneration
            user.RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return new TokenResponseDto(accessToken, user.RefreshToken, accessTokenExpires);
        }
    }
}
