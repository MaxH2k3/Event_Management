using AutoMapper;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Application.Message;
using Event_Management.Domain;
using Event_Management.Domain.Entity;
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
    public class FeedbackSevice : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackSevice(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddFeedback(FeedbackDto feedbackDto, string userId)
        {
            var feedbackEntity = _mapper.Map<Feedback>(feedbackDto);

            if (!string.IsNullOrEmpty(userId))
            {
                feedbackEntity.UserId = Guid.Parse(userId);
            }

            await _unitOfWork.FeedbackRepository.Add(feedbackEntity);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateFeedback(FeedbackDto feedbackDto, string userId)
        {
            var feedbackEntity = _mapper.Map<Feedback>(feedbackDto);
            if (!string.IsNullOrEmpty(userId))
            {
                feedbackEntity.UserId = Guid.Parse(userId);
            }
            await _unitOfWork.FeedbackRepository.Update(feedbackEntity);
            return await _unitOfWork.SaveChangesAsync();
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
