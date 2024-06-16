using Microsoft.AspNetCore.Http;

namespace Event_Management.Application.Helper
{
    public class SystemHelper
    {


        public static string GetIpAddress()
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            return httpContextAccessor.HttpContext.Connection?.LocalIpAddress?.ToString() ?? string.Empty;
        }
    }
}
