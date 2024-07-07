﻿using AutoMapper;
using Event_Management.Application.BackgroundTask;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Application.ServiceTask;
using Event_Management.Domain.Constants;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using System.Net;
using System.Text.Json;

namespace Event_Management.Domain.Service
{
    public class RegisterEventService : IRegisterEventService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly SendMailTask _sendMailTask;
        private object currentEvent;

        public RegisterEventService(IUnitOfWork unitOfWork, IMapper mapper,
			SendMailTask sendMailTask)
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
                    _sendMailTask.SendMail(registerEventModel);
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
                Status = ParticipantStatus.Pending.ToString()
            };

            await _unitOfWork.ParticipantRepository.UpSert(participant);

            if (await _unitOfWork.SaveChangesAsync())
            {
                _sendMailTask.SendMail(registerEventModel);

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

		public async Task<PagedList<ParticipantEventModel>> GetParticipantOnEvent(FilterParticipant filter)
		{
			var participants = await _unitOfWork.ParticipantRepository.FilterDataParticipant(filter);

			return _mapper.Map<PagedList<ParticipantEventModel>>(participants);
		}

		public async Task<APIResponse> AcceptRegisterEvent(Guid eventId, Guid userId)
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

            participant.Status = ParticipantStatus.Confirmed.ToString();

            await _unitOfWork.ParticipantRepository.Update(participant);

            if (await _unitOfWork.SaveChangesAsync())
			{
                return new APIResponse()
				{
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageParticipant.AcceptParticipant
                };
            }

            return new APIResponse()
			{
                StatusResponse = HttpStatusCode.NotModified,
                Message = MessageParticipant.AcceptParticipantFailed
            };
        }

	}
}
