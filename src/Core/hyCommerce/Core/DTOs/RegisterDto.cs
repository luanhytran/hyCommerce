using System.ComponentModel.DataAnnotations;

namespace hyCommerce.Core.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        public required string Password { get; set; }
    }
}
