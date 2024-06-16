using Event_Management.Domain.Repository.Common;
using Event_Management.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Entity;

namespace Event_Management.Domain.Repository
{
    public interface ITagRepository : IExtendRepository<Tag>
    {
        //Task<PagedList<Tag>> GetAllTagByEventId(Guid eventId);
    }
}
