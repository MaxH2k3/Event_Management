using AutoMapper;
using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.UnitOfWork;

namespace Event_Management.Application.Service
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
        public async Task<PagedList<FeedbackEvent>?> GetEventFeedbacks(Guid eventId, int? numOfStar, int pageNo, int elementEachPage)
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetFeedbackByEventIdAndStar(eventId, numOfStar, pageNo, elementEachPage);
            
            return _mapper.Map<PagedList<FeedbackEvent>>(feedbacks);

        }
        private async Task<CreatedByUserDto> getUserInfo(Guid userId)
        {
            var userInfo = await _unitOfWork.UserRepository.GetById(userId);
            CreatedByUserDto response = new CreatedByUserDto();
            response.avatar = userInfo!.Avatar;
            response.Name = userInfo.FullName;
            response.Id = userInfo.UserId;
            return response;
        }
        private async Task<FeedbackView> ToFeebackView(Feedback feedback)
        {
            return new FeedbackView
            {
                EventId = feedback.EventId,
                Content = feedback.Content,
                Rating = feedback.Rating,
                CreatedBy = await getUserInfo(feedback.UserId)
            };
        }
        public async Task<FeedbackView> GetUserFeedback(Guid eventId, Guid userId)
        {
            var result = await _unitOfWork.FeedbackRepository.GetUserEventFeedback(eventId, userId);
            return new FeedbackView
            {
                EventId = eventId,
                Content = result.Content,
                Rating = result.Rating,
                CreatedBy = await getUserInfo(result.UserId)
            };
        }
        public async Task<PagedList<FeedbackView>> GetAllUserFeebacks(Guid userId, int page, int eachPage)
        {
            var result = await _unitOfWork.FeedbackRepository.GetAllUserFeebacks(userId, page, eachPage);
            List<FeedbackView> response  = new List<FeedbackView>();
            foreach(var item in result!)
            {
                response.Add(await ToFeebackView(item));
            }
            return new PagedList<FeedbackView>
            {
                Items = response,
                TotalItems = response.Count,
                CurrentPage = page,
                EachPage = eachPage
            };
        }
    }
}
