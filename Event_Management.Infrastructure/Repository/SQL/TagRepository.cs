﻿using Event_Management.Domain;
using Event_Management.Domain.Repository;

using Event_Management.Infrastructure.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event_Management.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using Event_Management.Domain.Entity;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class TagRepository : SQLExtendRepository<Tag>, ITagRepository
    {
        private readonly EventManagementContext _context;

        public TagRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }

		public async Task<Tag> GetTagByName(string name)
		{
			var tag = await _context.Tags.FirstOrDefaultAsync(t => t.TagName.ToUpper().Trim().Equals(name.ToUpper().Trim()));
            if(tag != null)
            {
                return tag;
            }
			return null;
		}

        public async Task<List<Tag>> SearchTag(string searchTerm)
        {
            return await _context.Tags.Where(p => p.TagName.Contains(searchTerm)).ToListAsync();
        }
    }
}
