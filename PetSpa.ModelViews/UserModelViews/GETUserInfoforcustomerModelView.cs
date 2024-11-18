using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.UserModelViews
{
    public class GETUserInfoforcustomerModelView
    {
        public string FullName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? BankAccount { get; set; }
        public string? BankAccountName { get; set; }
        public string? Bank { get; set; }
    }
}
