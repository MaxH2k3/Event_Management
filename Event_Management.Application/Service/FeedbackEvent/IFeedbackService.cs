using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Domain;

namespace Event_Management.Application.Service.FeedbackEvent
{
    public interface IFeedbackService
    {
        Task<bool> AddFeedback(FeedbackDto feedbackDto);
        Task<bool> UpdateFeedback(FeedbackDto feedbackDto);


    }
}
