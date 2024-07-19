

using System.ComponentModel.DataAnnotations;

namespace Event_Management.Application.Dto.UserDTO.Request
{
    public class UpdateDeleteUserDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        [Required (ErrorMessage = "FullName is required")]
        public string? FullName { get; set; }
        [Required]
        [MinLength(9, ErrorMessage = "Phone number must be at least 10 digits.")]
        [MaxLength(11, ErrorMessage = "Phone number must be at most 11 digits.")]
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
    }
}
