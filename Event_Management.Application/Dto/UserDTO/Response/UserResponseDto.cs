namespace Event_Management.Application.Dto.UserDTO.Response;

public class UserResponseDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = null!;
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? Avatar { get; set; }
}
