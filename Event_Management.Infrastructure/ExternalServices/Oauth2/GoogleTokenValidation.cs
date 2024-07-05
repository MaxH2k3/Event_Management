
using Event_Management.Application.ExternalServices;
using Event_Management.Domain.Models.System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;

namespace Event_Management.Infrastructure.ExternalServices.Oauth2
{
    public class GoogleTokenValidation : IGoogleTokenValidation
    {
        private readonly IConfiguration _configuration;

        public GoogleTokenValidation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<APIResponse> ValidateGoogleTokenAsync(string token)
        {
            var tokenInfoUrl = $"https://www.googleapis.com/oauth2/v3/tokeninfo?access_token={token}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(tokenInfoUrl);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.Unauthorized,
                        Message = "Invalid Google token",
                        Data = null
                    };
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var tokenInfo = JsonConvert.DeserializeObject<GoogleTokenInfo>(responseString);

                if (tokenInfo == null || tokenInfo.Audience != _configuration["GoogleToken:Audience"])
                {
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.Unauthorized,
                        Message = "Invalid Google token",
                        Data = null
                    };
                }

                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = "Valid Google token",
                    Data = tokenInfo.Email,
                };
            }
        }
    }

    public class GoogleTokenInfo
    {
        [JsonProperty("azp")]
        public string? AuthorizedParty { get; set; }

        [JsonProperty("aud")]
        public string? Audience { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }
    }


}
