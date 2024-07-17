using AutoMapper;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Application.Message;
using Event_Management.Application.ServiceTask;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.ParticipantDTO;
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
		private readonly ISendMailTask _sendMailTask;

        public RegisterEventService(IUnitOfWork unitOfWork, IMapper mapper,
			ISendMailTask sendMailTask)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
            _sendMailTask = sendMailTask;
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
			var isExistedOnEvent = await _unitOfWork.ParticipantRepository.IsExistedOnEvent(registerEventModel.UserId, registerEventModel.EventId);

			if(isExistedOnEvent)
			{
				return new APIResponse()
				{
					StatusResponse = HttpStatusCode.Conflict,
					Message = MessageParticipant.ExistedOnEvent,
                    Data = new { registerEventModel.UserId, registerEventModel.EventId }
                };
			}

            var currentEvent = await _unitOfWork.EventRepository.GetById(registerEventModel.EventId);

            Participant participant = new()
			{
				UserId = registerEventModel.UserId,
				EventId = registerEventModel.EventId,
				RoleEventId = registerEventModel.RoleEventId,
				CreatedAt = DateTime.Now,
				IsCheckedMail = false,
                Status = currentEvent!.Approval ? ParticipantStatus.Pending.ToString() : ParticipantStatus.Confirmed.ToString()
            };

			await _unitOfWork.ParticipantRepository.Add(participant);

			if(await _unitOfWork.SaveChangesAsync())
			{
				if(!currentEvent!.Approval)
				{
                    _sendMailTask.SendMailTicket(registerEventModel);
                }
				
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

		public async Task<APIResponse> AddToEvent(RegisterEventModel registerEventModel)
		{
            Participant participant = new()
            {
                UserId = registerEventModel.UserId,
                EventId = registerEventModel.EventId,
                RoleEventId = registerEventModel.RoleEventId,
                CreatedAt = DateTime.Now,
                IsCheckedMail = false,
                Status = ParticipantStatus.Confirmed.ToString()
            };

            await _unitOfWork.ParticipantRepository.UpSert(participant);

            if (await _unitOfWork.SaveChangesAsync())
            {
                _sendMailTask.SendMailTicket(registerEventModel);

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
                    Data = JsonSerializer.Serialize(new { userId, eventId })
                };
			}

			return new APIResponse()
			{
				StatusResponse = HttpStatusCode.InternalServerError,
				Message = MessageCommon.ServerError,
				Data = JsonSerializer.Serialize(new { userId, eventId })
			};
		}

		public async Task<PagedList<ParticipantEventModel>> GetParticipantOnEvent(FilterParticipant filter)
		{
			var participants = await _unitOfWork.ParticipantRepository.FilterDataParticipant(filter);

			return _mapper.Map<PagedList<ParticipantEventModel>>(participants);
		}

		public async Task<APIResponse> ProcessingTicket(Guid eventId, Guid userId, string status)
		{
            var participant = await _unitOfWork.ParticipantRepository.GetParticipant(userId, eventId);

            if (participant == null)
			{
                return new APIResponse()
				{
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                };
            }

            participant.Status = status;

            await _unitOfWork.ParticipantRepository.Update(participant);

            if (await _unitOfWork.SaveChangesAsync())
			{
                return new APIResponse()
				{
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageParticipant.ProcessParticipant
                };
            }

            return new APIResponse()
			{
                StatusResponse = HttpStatusCode.NotModified,
                Message = MessageParticipant.ProcessParticipantFailed
            };
        }

        public async Task<APIResponse> GetEventParticipants(Guid eventId)
        {
            var eventInfo = await _unitOfWork.EventRepository.getAllEventInfo(eventId);
            List<ParticipantInfo> participantInfos = _mapper.Map<List<ParticipantInfo>>(eventInfo.Participants);
            return new APIResponse
            {
                Message = MessageCommon.Complete,
                StatusResponse = HttpStatusCode.OK,
                Data = participantInfos
            };
        }

        public async Task<APIResponse> UserRegisterStatus(Guid eventId, string? userId)
        {
            var eventInfo = await _unitOfWork.EventRepository.getAllEventInfo(eventId);
            ParticipantEventModel currentUser = _mapper.Map<ParticipantEventModel>
                (eventInfo.Participants.FirstOrDefault(ep => ep.UserId == Guid.Parse(userId!)));
            return new APIResponse
            {
                Message = MessageCommon.Complete,
                StatusResponse = HttpStatusCode.OK,
                Data = currentUser
            };
        }

        public async Task<ParticipantEventModel> GetCurrentUser(Guid userId, Guid eventId)
        {
            var user = await _unitOfWork.ParticipantRepository.GetDetailParticipant(userId, eventId);

            return _mapper.Map<ParticipantEventModel>(user);
        }

		public async Task<PagedList<ParticipantModel>> GetParticipantOnEvent(int page, int eachPage, Guid eventId)
		{
			var participants = await _unitOfWork.ParticipantRepository.GetAll(p => p.EventId.Equals(eventId), page, eachPage, ParticipantSortBy.CheckedIn.ToString());

            return _mapper.Map<PagedList<ParticipantModel>>(participants);
        }

        public async Task<bool> IsRole(Guid userId, Guid eventId, EventRole role)
        {
            return await _unitOfWork.ParticipantRepository.IsRole(userId, eventId, role);
        }

    }
}
