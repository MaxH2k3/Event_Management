using Event_Management.Application.Dto.AuthenticationDTO;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Domain;
using Event_Management.Domain.Models.System;

namespace Event_Management.Application.Service
{
    public interface IJWTService
    {
        Task<string> GenerateAccessToken(string email);
        string GenerateRefreshToken();
        Task<APIResponse> RefreshToken(TokenResponseDTO token);
    }
}
