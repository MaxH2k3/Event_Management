﻿using Event_Management.Domain;
using Event_Management.Domain.Repository;
using Event_Management.Infrastructure.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class SponsorMethodRepository : SQLRepository<SponsorMethod>, ISponsorMethodRepository
    {
        private readonly EventManagementContext _context;

        public SponsorMethodRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }
    }
}