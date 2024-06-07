using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.JWT
{
	public class JWTSetting
	{
		public string SecurityKey { get; set; } = null!;
		public string Issuer { get; set; } = null!;
		public string Audience { get; set; } = null!;
		public int TokenExpiry { get; set; }

        //public JWTSetting()
        //{
        //    GetSettingConfig();
        //}

        //private void GetSettingConfig()
        //{
        //    IConfiguration config = new ConfigurationBuilder()

        //    .SetBasePath(Directory.GetCurrentDirectory())

        //    .AddJsonFile("appsettings.json", true, true)

        //    .Build();

        //    this.SecurityKey = config["JWTSetting:Securitykey"];
        //    this.Issuer = config["JWTSetting:Issuer"];
        //    this.Audience = config["JWTSetting:Audience"];
        //    this.TokenExpiry = Convert.ToDouble(config["JWTSetting:TokenExpiry"]);

        //}
    }


}
