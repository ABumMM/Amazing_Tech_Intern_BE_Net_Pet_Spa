﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    internal class Review
    {
        public DateTime ReviewDate { get; set; }
        public string? Description { get; set; }
    }
}