﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PetSpa.Core.Base;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Customers : BaseEntity
    {
<<<<<<< HEAD
=======
  

>>>>>>> a1247fff7a39cde2f6ecd1b258c154411790114c
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        //public Guid MemberShipID { get; set; }

        //// public virtual MemberShip MemberShip { get; set; }
        ////ae nào làm bảng membership thì làm đi nhé 

<<<<<<< HEAD
        // Mối quan hệ với bảng Bookings(một-nhiều)
        /*public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();

        // Khóa ngoại
        public Guid ApplicationRoleId { get; set; }
        public virtual ApplicationRole ApplicationRole { get; set; }*/
=======
        //// Mối quan hệ với bảng Bookings(một-nhiều)
        //public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();

        //// Khóa ngoại
        //public Guid ApplicationRoleId { get; set; }
        //public virtual ApplicationRole ApplicationRole { get; set; }
>>>>>>> a1247fff7a39cde2f6ecd1b258c154411790114c
    }
}
