using AutoMapper;
using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> AddFeedback(FeedbackDto feedbackDto)
        {
            var feedbackEntity = _mapper.Map<Feedback>(feedbackDto);
            await _unitOfWork.FeedbackRepository.Add(feedbackEntity);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateFeedback(FeedbackDto feedbackDto)
        {
            var feedbackEntity = _mapper.Map<Feedback>(feedbackDto);
            await _unitOfWork.FeedbackRepository.Update(feedbackEntity);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
