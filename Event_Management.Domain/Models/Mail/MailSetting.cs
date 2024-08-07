﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.Mail
{
    public class MailSetting
    {
        public string DisplayName { get; set; } = null!;
        public string SmtpServer { get; set; } = null!;
        public int Port { get; set; }
        public string Mail { get; set; } = null!;
        public string Password { get; set; } = null!;

        public MailSetting()
        {
            IConfiguration config = new ConfigurationBuilder()

                    .SetBasePath(Directory.GetCurrentDirectory())

                    .AddJsonFile("appsettings.json", true, true)

                    .Build();

            config.GetSection("GmailSetting");

            DisplayName = config["GmailSetting:DisplayName"]!;
            SmtpServer = config["GmailSetting:SmtpServer"]!;
            Port = int.Parse(config["GmailSetting:Port"]!);
            Mail = config["GmailSetting:Mail"]!;
            Password = config["GmailSetting:Password"]!;
        }
    }
}
