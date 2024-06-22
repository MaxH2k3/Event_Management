﻿using Event_Management.Domain.Models.JWT;
using Event_Management.Domain.Models.Payment.VnpayPayment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Configuration
{
    public static class VnpayConfig
    {
        public static void AddVnpay(this IServiceCollection services, IConfiguration configuration)
        {
            var vnpaySetting = configuration.GetSection("Vnpay").Get<VnpaySetting>();

           

        }
    }
}
