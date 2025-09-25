using ECommerce.Business.DTOs.Auth;
using ECommerce.Business.DTOs.User;
using ECommerce.Data.Entities;

namespace ECommerce.Business.Interfaces
{

    public interface IAuthService
    {
        Task<UserDto> Register(RegisterRequestDto request);
        Task<TokenResponseDto> LoginAsync(LoginRequestDto request);
        Task<TokenResponseDto> RefreshTokenAsync(string token, string refreshToken);
    }
}
