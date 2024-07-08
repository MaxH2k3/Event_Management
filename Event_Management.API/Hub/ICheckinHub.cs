using Event_Management.Domain.Models;

namespace Event_Management.API.Hub
{
	public interface ICheckinHub
	{
		Task SendNotification(SocketResponse socketResponse);

	}
}
