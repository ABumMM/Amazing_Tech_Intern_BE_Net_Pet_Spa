﻿using Microsoft.AspNetCore.Identity;
using PetSpa.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string Password { get; set; } = string.Empty;
        public DateTime DayofBirth { get; set; }
        public string Address { get; set; }
        public virtual UserInfo? UserInfo { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        // Khóa ngoại đến ApplicationRole
        public string RoleId { get; set; }
        public virtual ApplicationRole Role { get; set; }

        // Mối quan hệ với bảng Bookings(một-nhiều)
        public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();

        // Mối quan hệ với bảng Orders (một-nhiều)
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

        public ApplicationUser()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}
