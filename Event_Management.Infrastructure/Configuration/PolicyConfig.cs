using Event_Management.Domain.Constants.System;
using Event_Management.Domain.Constants.User;
using Event_Management.Domain.Enum.User;
using Microsoft.Extensions.DependencyInjection;

namespace Event_Management.Infrastructure.Configuration
{
	public static class PolicyConfig
	{
		public static void AddPolicies(this IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				options.AddPolicy(PolicyType.User, policy => policy.RequireClaim(UserClaimType.Role, UserRole.User.ToString()));
				options.AddPolicy(PolicyType.Admin, policy => policy.RequireClaim(UserClaimType.Role, UserRole.Admin.ToString()));
				options.AddPolicy(PolicyType.Guest, policy => policy.RequireClaim(UserClaimType.Role, UserRole.Guest.ToString()));
			});
		}
	}
}
