using System.Text.Json.Serialization;

namespace hyCommerce.Domain.Entities;

public interface IAuditEntity
{
    string CreatedBy { get; set; }

    DateTime CreatedAt { get; set; }

    string? ModifiedBy { get; set; }

    DateTime? ModifiedAt { get; set; }
}

public class AuditEntity : IAuditEntity
{
    [JsonPropertyOrder(-1)]
    public int Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}