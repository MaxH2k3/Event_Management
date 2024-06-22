using AutoMapper;
using Azure;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service
{
    public class SponsorEventService : ISponsorEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public SponsorEventService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
        }

        public async Task<PagedList<SponsorEvent>> GetSponsorByEventId(Expression<Func<Guid, bool>> eventId, int page, int eachPage)
        {
            return await _unitOfWork.SponsorEventRepository.GetAll(page, eachPage);
        }
    }
}
