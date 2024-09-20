using PetSpa.Core.Base;

namespace PetSpa.Contract.Repositories.Entity
{
    public class UserInfo:BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public double? Salary { get; set; }
        public string? BankAccount { get; set; }
        public string? BankAccountName { get; set; }
        public string? Bank { get; set; }
    }
}
