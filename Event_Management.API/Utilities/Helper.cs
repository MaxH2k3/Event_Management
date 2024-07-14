using Event_Management.Domain.Enum;

namespace Event_Management.API.Utilities
{
    public class Helper
    {
        public static bool IsValidParticipantStatus(string status)
        {
            // Attempt to parse the string as a ParticipantStatus enum value
            return Enum.TryParse<ParticipantStatus>(status, out _);
        }
    }
}
