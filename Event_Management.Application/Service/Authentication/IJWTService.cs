using Event_Management.Application.Dto.AuthenticationDTO;
using Event_Management.Domain;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.Models.User;

namespace Event_Management.Application.Service
{
	public interface IJWTService
    {
        Task<string> GenerateAccessToken(LoginUserDto user);
        string GenerateRefreshToken();
        Task<APIResponse> RefreshToken(TokenResponseDTO token);
    }
}
