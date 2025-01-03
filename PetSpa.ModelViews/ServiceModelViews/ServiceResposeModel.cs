﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.ServiceModelViews
{
    public class ServiceResposeModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        
        public string? Description { get; set; }

        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
