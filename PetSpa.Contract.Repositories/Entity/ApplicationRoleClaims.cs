﻿using Microsoft.AspNetCore.Identity;
using PetSpa.Core.Utils;
using System.ComponentModel.DataAnnotations;

namespace PetSpa.Contract.Repositories.Entity
{
    public class ApplicationRoleClaims: IdentityRoleClaim<Guid>
    {
        [Key]
        public string Id { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public ApplicationRoleClaims()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}
