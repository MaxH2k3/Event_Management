using AutoMapper;
using Event_Management.Application.Dto;
using Event_Management.Application.Helper;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.UnitOfWork;

namespace Event_Management.Application.Service
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

        public async Task<Tag> AddTag(TagDto tagDTO)
        {
            var existTag = await _unitOfWork.TagRepository.GetTagByName(tagDTO.TagName);
            if(existTag == null)
            {
				var tagEntity = _mapper.Map<Tag>(tagDTO);

				await _unitOfWork.TagRepository.Add(tagEntity);
				await _unitOfWork.SaveChangesAsync();
                var addedTag = await _unitOfWork.TagRepository.GetTagByName(tagDTO.TagName);
                return addedTag;

  
			}
            return null;
 
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

        public async Task<List<TagDto>> SearchTag(string searchTerm)
        {

            var filteredTags = await _unitOfWork.TagRepository.SearchTag(searchTerm);

            // Convert filtered tags to TagDto using AutoMapper
            var tagDtos = _mapper.Map<List<Tag>, List<TagDto>>(filteredTags);


            return tagDtos;
        }

        
        public async Task<Tag> GetById(int TagId)
        {
            var result = await _unitOfWork.TagRepository.GetById(TagId);
            return result!;
        }




        /*public async Task<bool> UpdateTag(TagDto tagDTO)
        {
           var tagEntity = _mapper.Map<Tag>(tagDTO);
           await _unitOfWork.TagRepository.Update(tagEntity);
            return await _unitOfWork.SaveChangesAsync();
        }*/
    }
}
