using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.PetsModelViews;

namespace PetSpa.ModelViews.UserModelViews
{
    public class GETUserModelView
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public GETUserInfoModelView? UserInfo { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public List<GETPetsModelView> Pets { get; set; } = new List<GETPetsModelView>();
    }
}
