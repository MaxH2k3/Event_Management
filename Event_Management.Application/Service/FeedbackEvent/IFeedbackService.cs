using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;

namespace Event_Management.Application.Service
{
    public interface IFeedbackService
    {
        Task<FeedbackDto> AddFeedback(FeedbackDto feedbackDto, Guid userId);
        Task<FeedbackDto> UpdateFeedback(FeedbackDto feedbackDto, Guid userId);
        Task<PagedList<FeedbackEvent>?> GetEventFeedbacks(Guid eventId, int? numOfStar, int pageNo, int elementEachPage);
        Task<FeedbackView?> GetUserFeedback(Guid eventId, Guid userId);
        Task<PagedList<FeedbackView>> GetAllUserFeebacks(Guid userId, int page, int eachPage);
    }
}
