using Microsoft.AspNetCore.Http;

namespace Event_Management.Application.Service.Account
{

    public class CurrentUserService : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        //public string UserId => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


        public string? IpAddress => _httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();
    }
}
