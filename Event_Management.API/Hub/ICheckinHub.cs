namespace Event_Management.API.Hub
{
	public interface ICheckinHub
	{
		Task SendNotification(string message);
	}
}
