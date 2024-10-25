using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.PetsModelViews;

namespace PetSpa.ModelViews.UserModelViews
{
    public class GETUserModelView
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string RoleName { get; set; }

        public List<GETPetsModelView>? Pets { get; set; }

        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
