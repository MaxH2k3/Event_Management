﻿using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Repository;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class LogoRepository: SQLExtendRepository<Logo>, ILogoRepository
    {
        private readonly EventManagementContext _context;

        public LogoRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }
    }
}
