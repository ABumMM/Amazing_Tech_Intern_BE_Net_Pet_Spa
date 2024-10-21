﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.BookingModelViews
{
    public class POSTBookingVM
    {
        public string? Description { get; set; }    
        public string? Status { get; set; }
        public DateTime Date { get; set; }
        public required string ApplicationUserId { get; set; }
    }
}
