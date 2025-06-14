﻿using hyCommerce.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace hyCommerce.Domain.Entities
{
    public class User : IdentityUser<int>, IAuditEntity, ISoftDelete
    {
        public UserAddress Address { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
