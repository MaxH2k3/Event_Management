namespace Event_Management.Application.Message
{
	public class MessageParticipant
	{
		public const string CheckInUserSuccess = "Check in success!";
		public const string CheckInUserFailed = "QR Code not valid!";
		public const string NotOwner = "You are not owner of this event";
		public const string ProcessParticipant = "Process participant success!";
        public const string ProcessParticipantFailed = "Process participant failed!";
		public const string ExistedOnEvent = "You already registered on event!";
        public const string AcceptInvite = "Accept invite success!";
		public const string AcceptInviteFailed = "Accept invite failed!";
		public const string HostCannotRegister = "Host cannot register on event!";
    }
}
