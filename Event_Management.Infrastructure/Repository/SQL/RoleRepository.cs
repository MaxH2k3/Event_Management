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
    public class RoleRepository : SQLRepository<Role>, IRoleRepository
    {
        private readonly EventManagementContext _context;

        public RoleRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }
    }
}
