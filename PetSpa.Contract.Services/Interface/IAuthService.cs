using PetSpa.ModelViews.AuthModelViews;

public interface IAuthService
{
    Task<string?> SignUpAsync(SignUpAuthModelView signup);
    Task<string?> SignInAsync(SignInAuthModelView signin);
    Task<bool> ChangePasswordAsync(ChangePasswordAuthModelView changepass, Guid UserId);
}
