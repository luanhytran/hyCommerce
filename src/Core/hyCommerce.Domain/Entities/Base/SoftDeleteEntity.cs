namespace hyCommerce.Domain.Entities.Base;

public interface ISoftDelete
{
    public string? DeletedBy { get; set; } 
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}

public class SoftDeleteEntity : AuditEntity, ISoftDelete
{
    public string? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}