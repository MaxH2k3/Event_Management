using AutoMapper;
using Event_Management.Application.Dto.ParticipantDTO;
using Event_Management.Application.Message;
using Event_Management.Domain;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using System.Net;
using System.Text.Json;

namespace Event_Management.Application.Service
{
	public class RegisterEventService : IRegisterEventService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RegisterEventService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<APIResponse> DeleteParticipant(Guid userId, Guid eventId)
		{
			if(!(await _unitOfWork.ParticipantRepository.IsExistedOnEvent(userId, eventId)))
			{
				return new APIResponse()
				{
					StatusResponse = HttpStatusCode.NotFound,
					Message = MessageCommon.NotFound,
					Data = JsonSerializer.Serialize(new { userId, eventId })
				};
			}

			await _unitOfWork.ParticipantRepository.Delete(userId, eventId);

			if(await _unitOfWork.SaveChangesAsync())
			{
				return new APIResponse()
				{
					StatusResponse = HttpStatusCode.OK,
					Message = MessageCommon.DeleteSuccessfully,
					Data = JsonSerializer.Serialize(new { userId, eventId })
				};
			}

			return new APIResponse()
			{
				StatusResponse = HttpStatusCode.NotModified,
				Message = MessageCommon.DeleteFailed,
				Data = JsonSerializer.Serialize(new { userId, eventId })
			};
		}

		public async Task<APIResponse> RegisterEvent(RegisterEventModel registerEventModel)
		{
			Participant participant = new()
			{
				UserId = registerEventModel.UserId,
				EventId = registerEventModel.EventId,
				RoleEventId = registerEventModel.RoleEventId,
				CreatedAt = DateTime.Now
			};


			await _unitOfWork.ParticipantRepository.Add(participant);

			if(await _unitOfWork.SaveChangesAsync())
			{
				return new APIResponse()
				{
					StatusResponse = HttpStatusCode.Created,
					Message = MessageCommon.SavingSuccesfully,
					Data = registerEventModel
				};
			}

			return new APIResponse()
			{
				StatusResponse = HttpStatusCode.NotModified,
				Message = MessageCommon.SavingFailed,
				Data = registerEventModel
			};
		}

		public async Task<APIResponse> UpdateRoleEvent(RegisterEventModel registerEventModel)
		{
			var participant = await _unitOfWork.ParticipantRepository.GetParticipant(registerEventModel.UserId, registerEventModel.EventId);

			if (participant == null)
			{
				return new APIResponse()
				{
					StatusResponse = HttpStatusCode.NotFound,
					Message = MessageCommon.NotFound,
					Data = registerEventModel
				};
			}

			participant.RoleEventId = registerEventModel.RoleEventId;

			await _unitOfWork.ParticipantRepository.Update(participant);

			if(await _unitOfWork.SaveChangesAsync())
			{
				return new APIResponse()
				{
					StatusResponse = HttpStatusCode.OK,
					Message = MessageCommon.UpdateSuccesfully,
					Data = registerEventModel
				};
			}

			return new APIResponse()
			{
				StatusResponse = HttpStatusCode.NotModified,
				Message = MessageCommon.UpdateFailed,
				Data = registerEventModel
			};
		}

		public async Task<APIResponse> CheckInParticipant(Guid userId, Guid eventId)
		{
			var participant = await _unitOfWork.ParticipantRepository.GetParticipant(userId, eventId);

			if (participant == null)
			{
				return new APIResponse()
				{
					StatusResponse = HttpStatusCode.NotFound,
					Message = MessageParticipant.CheckInUserFailed,
					Data = JsonSerializer.Serialize(new { userId, eventId })
				};
			}

			participant.CheckedIn = DateTime.Now;

			await _unitOfWork.ParticipantRepository.Update(participant);

			if (await _unitOfWork.SaveChangesAsync())
			{
				return new APIResponse()
				{
					StatusResponse = HttpStatusCode.OK,
					Message = MessageParticipant.CheckInUserSuccess,
				};
			}

			return new APIResponse()
			{
				StatusResponse = HttpStatusCode.InternalServerError,
				Message = MessageCommon.ServerError,
				Data = JsonSerializer.Serialize(new { userId, eventId })
			};
		}

	}
}
