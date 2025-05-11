using System.ComponentModel.DataAnnotations.Schema;

namespace hyCommerce.Domain.Entities;

[Table("RefreshTokens")]
public class RefreshToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsExpires => DateTime.UtcNow >= Expires;
    public bool IsActive => Revoked == null && !IsExpires;
    public User User { get; set; }
}