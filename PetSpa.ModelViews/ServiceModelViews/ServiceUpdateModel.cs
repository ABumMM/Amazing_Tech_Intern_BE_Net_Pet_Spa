﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.ServiceModelViews
{
    public class ServiceUpdateModel
    {
        public required string Name { get; set; }
       
        public string? Description { get; set; }

    }
}
