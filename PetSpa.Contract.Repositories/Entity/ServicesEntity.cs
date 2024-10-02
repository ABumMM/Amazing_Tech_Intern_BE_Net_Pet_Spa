
using System;
﻿using PetSpa.Core.Base;
using System.Text.Json.Serialization;


namespace PetSpa.Contract.Repositories.Entity
{
    public class ServicesEntity : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public ICollection<PackageServiceEntity>? PackageServices { get; set; } = new HashSet<PackageServiceEntity>();

    }
}

