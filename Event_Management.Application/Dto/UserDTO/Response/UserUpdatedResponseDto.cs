namespace Event_Management.Application.Dto.UserDTO.Response;

public class UserUpdatedResponseDto
{
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
}
