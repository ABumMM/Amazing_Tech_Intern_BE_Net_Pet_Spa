﻿using Microsoft.VisualBasic;
using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Packages:BaseEntity
    {
        [Key]
        public Guid ID {  get; set; }
        public string Name { get; set; }=string.Empty;
        public string? Image {  get; set; }
        public double? Price { get; set; }
        public string? Information {  get; set; }
        public string? Experiences { get; set; }

        public Guid ServiceID { get; set; }
    }
}
