
using Event_Management.Domain.Models.System;

namespace Event_Management.Application.ExternalServices
{
    public interface IGoogleTokenValidation
    {
        Task<APIResponse> ValidateGoogleTokenAsync(string token);
    }
}
