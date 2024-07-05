using System.ComponentModel.DataAnnotations;

namespace Event_Management.Application;

public class LoginInWithGoogleDto
{

    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage ="Access token is required")]
    public string? Token { get; set; }
    public string PhotoUrl { get; set; } = null!;
}
