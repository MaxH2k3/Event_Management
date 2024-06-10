using Event_Management.Application.Service;
using Event_Management.Application.Validators;
using Event_Management.Domain.Repository.Common;
using Event_Management.Domain.Service;
using Event_Management.Domain.Service.TagEvent;
using Event_Management.Domain.UnitOfWork;
using Event_Management.Infrastructure.Configuration;
using Event_Management.Infrastructure.Repository;
using Event_Management.Infrastructure.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text.Json;
//using Event_Management.Application.Service.SQL;

namespace Event_Management.Infastructure.Configuration
{
	public static class DependencyInjection
    {
        public static void AddInfrastructure(this WebApplicationBuilder builder)
        {
			// Set up AutoMapper
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			// Set cache redis
			builder.Services.AddStackExchangeRedisCache(options =>
            {
                var redisConnection = builder.Configuration["Redis:HostName"];
				var redisPassword = builder.Configuration["Redis:Password"];
                options.Configuration = $"{redisConnection},password={redisPassword}";
			});
			builder.Services.AddDistributedMemoryCache();

            // Set up logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
				.CreateLogger();

			builder.Host.UseSerilog();

			// Set up FluentMail
			builder.Services.AddFluentEmail(builder.Configuration);

			//set up configuration JWT
			builder.Services.AddJWT(builder.Configuration);

			// Set up policies
			builder.Services.AddPolicies();

			// Set up Swagger
			builder.Services.AddSwagger();

			//Setup FluentValidation
			builder.Services.AddValidatorsFromAssemblyContaining<EventRequestDtoValidator>();

            //Setup VNP
            //services.Configure<VNPAYPaymentRequest>(configuration.GetSection("VNPAY"));

			// Set up SignalR
			builder.Services
				.AddSignalR(option =>
				{
					option.EnableDetailedErrors = true;
					option.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
					option.MaximumReceiveMessageSize = 5 * 1024 * 1024; // 5MB
				})
				.AddJsonProtocol(option =>
				{
					option.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				});

			// Set up services to the container.
			builder.Services.AddScoped<IRegisterEventService, RegisterEventService>();
			builder.Services.AddScoped<ITagService, TagService>();
			builder.Services.AddScoped<IEventService, EventService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IJWTService, JWTService>();
			builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddScoped<ICacheRepository, CacheRepository>();
			//builder.Services.AddScoped<ISqlService, SqlService>();
			//builder.Services.AddScoped<ICurrentUser, CurrentUserService>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

		}
	}
}
