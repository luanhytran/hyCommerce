namespace hyCommerce.Application.DTOs;

public class ResetPasswordDto
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}