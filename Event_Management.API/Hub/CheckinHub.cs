using Event_Management.Application.Service;
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
				await Clients.Groups(eventId.ToString()).SendNotification(new Domain.Models.SocketResponse()
				{
					StatusResponse = false,
                    Message = response.Message,
					Data = response.Data
                });
			}

			await Clients.Groups(eventId.ToString()).SendNotification(new Domain.Models.SocketResponse()
            {
                StatusResponse = false,
                Message = response.Message,
                Data = response.Data
            });
		}

	}
}
