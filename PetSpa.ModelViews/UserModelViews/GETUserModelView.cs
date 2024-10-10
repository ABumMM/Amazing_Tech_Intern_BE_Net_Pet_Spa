using PetSpa.Contract.Repositories.Entity;

namespace PetSpa.ModelViews.UserModelViews
{
    public class GETUserModelView
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
