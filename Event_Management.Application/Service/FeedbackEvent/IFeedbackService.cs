﻿using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Domain;
using Event_Management.Domain.Models.System;

namespace Event_Management.Application.Service.FeedbackEvent
{
    public interface IFeedbackService
    {
        Task<bool> AddFeedback(FeedbackDto feedbackDto, string userId);
        Task<bool> UpdateFeedback(FeedbackDto feedbackDto, string userId);
        Task<APIResponse> GetEventFeedbacks(Guid eventId);
    }
}
