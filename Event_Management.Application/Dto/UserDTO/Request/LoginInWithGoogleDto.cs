namespace Event_Management.Application;

public class LoginInWithGoogleDto
{
    public string? gguid { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhotoUrl { get; set; } = null!;
}
