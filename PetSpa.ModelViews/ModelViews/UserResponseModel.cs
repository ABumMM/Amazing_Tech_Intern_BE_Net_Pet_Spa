using PetSpa.Contract.Repositories.Entity;

namespace PetSpa.ModelViews.UserResponseModel
{
    public class UserResponseModel
    {
        public string? Id { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string? BankAccount { get; set; }
        public string? BankAccountName { get; set; }
        public string? Bank { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
