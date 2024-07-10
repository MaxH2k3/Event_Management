using AutoMapper;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Application.Message;
using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.FeedbackEvent
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Feedback> AddFeedback(FeedbackDto feedbackDto, string userId)
        {
            var feedbackEntity = _mapper.Map<Feedback>(feedbackDto);

            if (!string.IsNullOrEmpty(userId))
            {
                feedbackEntity.UserId = Guid.Parse(userId);
            }
            feedbackEntity.CreatedAt = DateTimeHelper.GetDateTimeNow();
            await _unitOfWork.FeedbackRepository.Add(feedbackEntity);
            await _unitOfWork.SaveChangesAsync();
            return feedbackEntity;
        }

        public async Task<Feedback> UpdateFeedback(FeedbackDto feedbackDto, string userId)
        {
            var feedbackEntity = _mapper.Map<Feedback>(feedbackDto);
            if (!string.IsNullOrEmpty(userId))
            {
                feedbackEntity.UserId = Guid.Parse(userId);
            }
            feedbackEntity.CreatedAt = DateTimeHelper.GetDateTimeNow();
            await _unitOfWork.FeedbackRepository.Update(feedbackEntity);
            await _unitOfWork.SaveChangesAsync();
            return feedbackEntity;
        }
        public async Task<APIResponse> GetEventFeedbacks(Guid eventId)
        {
            var eventInfo = await _unitOfWork.EventRepository.getAllEventInfo(eventId);
            List<FeedbackDto> response  = _mapper.Map<List<FeedbackDto>>(eventInfo.Feedbacks);
            return new APIResponse
            {
                Message = MessageCommon.Complete,
                StatusResponse = HttpStatusCode.OK,
                Data = response
            };
        }
    }
}
