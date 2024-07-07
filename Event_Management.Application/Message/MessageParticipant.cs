namespace Event_Management.Application.Message
{
	public class MessageParticipant
	{
		public const string CheckInUserSuccess = "Check in success!";
		public const string CheckInUserFailed = "QR Code not valid!";
		public const string NotOwner = "You are not owner of this event";
		public const string AcceptParticipant = "Accept participant success!";
        public const string AcceptParticipantFailed = "Accept participant failed!";
		public const string ExistedOnEvent = "You already registered on event!";
        public const string AcceptInvite = "Accept invite success!";
		public const string AcceptInviteFailed = "Accept invite failed!";
    }
}
