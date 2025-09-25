using System.ComponentModel.DataAnnotations;

namespace ECommerce.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = null!; 

        [Required]
        public string Password { get; set; } = null!; 

        [Required]
        public string Email { get; set; } = null!;

        public DateTime? LastLoginTime { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
