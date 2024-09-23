using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.UserModelViews
{
    public class PUTUserModelView
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber {  get; set; } = string.Empty;
        public DateTime DayofBirth { get; set; }
        public string? Address { get; set; }
        public decimal? Salary { get; set; }
        public string? BankAccount { get; set; }
        public string? BankAccountName { get; set; }
        public string? Bank { get; set; }

        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
