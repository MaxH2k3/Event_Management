
namespace Event_Management.Application.ExternalServices
{
    public interface IAvatarApiClient
    {
        string GetRandomAvatarUrl();
        string GetRandomBoyAvatarUrl();
        string GetRandomGirlAvatarUrl();
        string GetAvatarUrlWithName(string firstName, string lastName);
    }
}
