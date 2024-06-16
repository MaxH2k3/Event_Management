
using Event_Management.Application.ExternalServices;

namespace Event_Management.Infrastructure.ExternalServices.ApiClients
{
   
    public class AvatarApiClient : IAvatarApiClient
    {
        private readonly string _baseUrl = "https://avatar.iran.liara.run";

        public string GetRandomAvatarUrl()
        {
            return $"{_baseUrl}/public";
        }

        public string GetRandomBoyAvatarUrl()
        {
            return $"{_baseUrl}/public/boy";
        }

        public string GetRandomGirlAvatarUrl()
        {
            return $"{_baseUrl}/public/girl";
        }

        public string GetAvatarUrlWithName(string fullName)
        {
            return $"{_baseUrl}/username?username={fullName}";
        }
    }
}
