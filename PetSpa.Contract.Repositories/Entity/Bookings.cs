﻿using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Bookings : BaseEntity
    {
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }


        

        ////khóa ngoại oderid

        public string? OrdersId { get; set; }
        [JsonIgnore]
        public virtual Orders Orders { get; set; }

        //1 booking có nhiều gói
        //[JsonIgnore]
        //public virtual ICollection<Packages> Packages { get; set; } = new List<Packages>();
        [JsonIgnore]
        public ICollection<BookingPackage>? BookingPackages { get; set; } = new HashSet<BookingPackage>();

    }
}
