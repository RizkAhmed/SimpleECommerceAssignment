using System.ComponentModel.DataAnnotations;

namespace ECommerce.Business.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, ErrorMessage = "User name cannot be longer than 50 characters.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please re-enter your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ReWritePassword { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = null!;
    }

}
