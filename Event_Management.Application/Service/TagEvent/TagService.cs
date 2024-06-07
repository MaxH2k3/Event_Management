using AutoMapper;
using Event_Management.Application.Dto;
using Event_Management.Application.Message;
using Event_Management.Domain;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.TagEvent
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<bool> AddTag(TagDTO tagDTO)
        {
            var tagEntity = _mapper.Map<Tag>(tagDTO);

            await _unitOfWork.TagRepository.Add(tagEntity);
            return await _unitOfWork.SaveChangesAsync();
           
            
             
        }

        public async Task<bool> DeleteTag(int TagId)
        {
            await _unitOfWork.TagRepository.Delete(TagId);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedList<Tag>> GetAllTag(int page, int eachPage)
        {
            return await _unitOfWork.TagRepository.GetAll(page, eachPage, "TagName");

            
        }



        public async Task<bool> UpdateTag(TagDTO tagDTO)
        {
            var tagEntity = _mapper.Map<Tag>(tagDTO);
            await _unitOfWork.TagRepository.Update(tagEntity);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
