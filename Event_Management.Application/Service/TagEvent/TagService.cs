using AutoMapper;
using Event_Management.Application.Dto;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace Event_Management.Domain.Service.TagEvent
{
	public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddTag(TagDto tagDTO)
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

       

        public async Task<bool> UpdateTag(TagDto tagDTO)
        {
            var tagEntity = _mapper.Map<Tag>(tagDTO);
            await _unitOfWork.TagRepository.Update(tagEntity);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
