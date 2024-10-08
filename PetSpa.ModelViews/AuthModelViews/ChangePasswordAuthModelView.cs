using System.ComponentModel.DataAnnotations;

public class ChangePasswordAuthModelView
{
    public required string CurrentPassword { get; set; }

    public required string NewPassword { get; set; }

    public required string ConfirmPassword { get; set; }
}
