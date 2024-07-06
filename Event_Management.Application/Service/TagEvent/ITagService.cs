using Event_Management.Application.Dto;
using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Service.TagEvent
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

    }
}
