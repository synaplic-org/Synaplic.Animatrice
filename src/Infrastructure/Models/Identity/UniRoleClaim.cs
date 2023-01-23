using System;
using Synaplic.Inventory.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Synaplic.Inventory.Infrastructure.Models.Identity
{
    public class UniRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
    {
        public string Description { get; set; }
        public string Group { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual UniRole Role { get; set; }

        public UniRoleClaim() : base()
        {
        }

        public UniRoleClaim(string roleClaimDescription = null, string roleClaimGroup = null) : base()
        {
            Description = roleClaimDescription;
            Group = roleClaimGroup;
        }
    }
}