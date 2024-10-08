using System.ComponentModel.DataAnnotations;

namespace PetSpa.ModelViews.AuthModelViews
{
    public class SignUpAuthModelView
    {
        public required string FullName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
