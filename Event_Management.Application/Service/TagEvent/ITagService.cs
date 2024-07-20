using Event_Management.Application.Dto;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;

namespace Event_Management.Application.Service
{
    public interface ITagService
    {
        //Get all Tag for Event
        Task<PagedList<Tag>> GetAllTag(int page, int eachPage);
        
        Task<Tag> AddTag(TagDto tagDTO);
        Task<bool> DeleteTag(int TagId);
        Task<Tag> GetById(int TagId);
        //Task<bool> UpdateTag(TagDto tagDTO);
        Task<List<TagDto>> SearchTag(string searchTerm);

        Task<List<TagDto>> TrendingsTags();

    }
}
