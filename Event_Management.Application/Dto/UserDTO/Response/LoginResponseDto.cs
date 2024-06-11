
using Event_Management.Application.Dto.AuthenticationDTO;

namespace Event_Management.Application.Dto.UserDTO.Response
{
    public class LoginResponseDto
    {
        public UserResponseDto? User { get; set; }
        public TokenResponseDTO? Token { get; set; }
    }
}
