﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.OrderModelViews
{
    public class GetOrderViewModel
    {
        public string? OrderID { get; set; }
        public DateTime? Date { get; set; }
        public string? PaymentMethod { get; set; }
        public double? Total { get; set; }
    }
}
