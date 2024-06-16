using Event_Management.Application.Message;
using Event_Management.Domain.Service;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace Event_Management.API.Hub
{
	public class CheckinHub : Hub<ICheckinHub>
	{
		private readonly IRegisterEventService _registerEventService;

		public CheckinHub(IRegisterEventService registerEventService)
		{
			_registerEventService = registerEventService;
		}

		public async Task CheckinUser(Guid userId, Guid eventId)
		{
			var response = await _registerEventService.CheckInParticipant(userId, eventId);

			if(response.StatusResponse != HttpStatusCode.OK)
			{
				await Clients.Groups(eventId.ToString()).SendNotification(MessageParticipant.CheckInUserSuccess);
			}

			await Clients.Groups(eventId.ToString()).SendNotification(MessageParticipant.CheckInUserFailed);
		}

	}
}
