﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.UserModelViews
{
    public class PUTuserforcustomer
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public GETUserInfoforcustomerModelView? UserInfo { get; set; }
    }
}