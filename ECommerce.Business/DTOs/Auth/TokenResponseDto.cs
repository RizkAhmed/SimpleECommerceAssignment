namespace ECommerce.Business.DTOs.Auth
{
    public record TokenResponseDto(string AccessToken, string RefreshToken, DateTime Expiration);
}

